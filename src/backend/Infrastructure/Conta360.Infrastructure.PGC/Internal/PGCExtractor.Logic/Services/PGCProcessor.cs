using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using Conta360.Core.Interfaces;
using Conta360.Domain.Entities;
using Conta360.Persistence.Contexts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Conta360.Persistence;
using Microsoft.EntityFrameworkCore;
using Conta360.Infrastructure.Sqlite;

namespace PGCExtractor.Logic.Services
{
    /// <inheritdoc />
    public class PgcProcessor : IPgcProcessor
    {
        private readonly IPgcTaxonomyDownloader _downloader;
        private readonly ILogger<PgcProcessor> _logger;
        private readonly PgcExtractorOptions _options;
        private readonly IApplicationDbContext _dbContext;

        public PgcProcessor(
            IPgcTaxonomyDownloader downloader,
            IOptions<PgcExtractorOptions> options,
            ILogger<PgcProcessor> logger,
            IApplicationDbContext dbContext)
        {
            _downloader = downloader;
            _options = options.Value;
            _logger = logger;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Flujo completo: descarga, descompresión, parseo y persistencia de PGC.
        /// </summary>
        public async Task SystemProcessAsync(CancellationToken cancellationToken = default)
        {
            if (_options.EnableStartupDownload)
            {
                await _downloader.DownloadAndExtractAsync(cancellationToken);
            }

            var sourceDir = _options.ExtractDirectory;
            var allAccounts = await ParsePgcAccountsFromDirectoryAsync(sourceDir, cancellationToken);
            await PersistPgcAccountsAsync(allAccounts, cancellationToken);
        }

        /// <summary>
        /// Lee todos los archivos linkbase de sourceDir y genera la lista de PgcAccount.
        /// </summary>
        private async Task<List<PgcAccount>> ParsePgcAccountsFromDirectoryAsync(
            string sourceDir,
            CancellationToken cancellationToken)
        {
            var result = new List<PgcAccount>();

            // Espacios de nombres XBRL comunes
            XNamespace nsLoc = "http://www.xbrl.org/2003/instance";
            XNamespace nsLabel = "http://www.xbrl.org/2003/role/label";
            XNamespace nsPresentation = "http://www.xbrl.org/2003/linkbase";

            // Diccionario para nombres (label → texto)
            var labels = new Dictionary<string, string>();

            // 1) Leer todos los labelLinkbase*.xml para poblar etiquetas
            var labelFiles = Directory.GetFiles(sourceDir, "*labelLinkbase*.xml", SearchOption.AllDirectories);
            foreach (var file in labelFiles)
            {
                var xdoc = XDocument.Load(file);
                foreach (var lab in xdoc.Descendants(nsLabel + "label"))
                {
                    var labelName = lab.Attribute("label")?.Value;
                    var text = lab.Value;
                    if (!string.IsNullOrEmpty(labelName) && !labels.ContainsKey(labelName))
                    {
                        labels[labelName] = text;
                    }
                }
            }

            // Diccionario para relaciones padre-hijo (childCode → parentCode)
            var parentLookup = new Dictionary<string, string>();

            // 2) Procesar cada presentationLinkbase*.xml
            var presFiles = Directory.GetFiles(sourceDir, "*presentationLinkbase*.xml", SearchOption.AllDirectories);
            foreach (var file in presFiles)
            {
                var xdoc = XDocument.Load(file);

                // a) Extraer cada <loc>
                foreach (var loc in xdoc.Descendants(nsLoc + "loc"))
                {
                    var lbl = loc.Attribute("label")?.Value;
                    var href = loc.Attribute("href")?.Value;
                    if (string.IsNullOrEmpty(lbl) || string.IsNullOrEmpty(href))
                        continue;

                    // Extraer el fragmento "pgc_XXX" antes de "#"
                    var fragment = href.Split('#')[0];      // "pgc_570"
                    var fileName = Path.GetFileNameWithoutExtension(fragment); // "pgc_570"
                    if (!fileName.StartsWith("pgc_"))
                        continue;

                    var code = fileName[4..]; // "570"
                    if (string.IsNullOrEmpty(code) || result.Any(r => r.Code == code))
                        continue;

                    int level = code.Length switch
                    {
                        1 => 1,
                        2 => 2,
                        3 => 3,
                        _ => 4
                    };
                    bool isMovable = level >= 3;
                    labels.TryGetValue(lbl, out var name);

                    result.Add(new PgcAccount
                    {
                        Code = code,
                        Name = name ?? code,
                        Level = level,
                        ParentCode = null,
                        IsMovable = isMovable
                    });
                }

                // b) Extraer relaciones <presentationArc>
                foreach (var arc in xdoc.Descendants(nsPresentation + "presentationArc"))
                {
                    var fromLabel = arc.Attribute("from")?.Value;
                    var toLabel = arc.Attribute("to")?.Value;
                    if (string.IsNullOrEmpty(fromLabel) || string.IsNullOrEmpty(toLabel))
                        continue;

                    // Obtener códigos a partir de los labels
                    if (!labels.TryGetValue(fromLabel, out var parentName)) 
                        continue;
                    if (!labels.TryGetValue(toLabel, out var childName)) 
                        continue;

                    var parent = result.FirstOrDefault(r => r.Name == parentName);
                    var child = result.FirstOrDefault(r => r.Name == childName);
                    if (parent != null && child != null)
                    {
                        parentLookup[child.Code] = parent.Code;
                    }
                }
            }

            // 3) Asignar ParentCode a cada PgcAccount
            foreach (var acct in result)
            {
                if (parentLookup.TryGetValue(acct.Code, out var pCode))
                {
                    acct.ParentCode = pCode;
                }
            }

            return await Task.FromResult(result);
        }

        /// <summary>
        /// Persiste en SQLite la lista de cuentas PGC, eliminando registros anteriores.
        /// </summary>
        private async Task PersistPgcAccountsAsync(
            List<PgcAccount> accounts,
            CancellationToken cancellationToken)
        {
            var existing = _dbContext.PgcAccounts.AsQueryable();
            _dbContext.PgcAccounts.RemoveRange(existing);

            await _dbContext.PgcAccounts.AddRangeAsync(accounts, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "[PgcProcessor] Persistidos {Count} cuentas PGC en SQLite.",
                accounts.Count);
        }
    }
}

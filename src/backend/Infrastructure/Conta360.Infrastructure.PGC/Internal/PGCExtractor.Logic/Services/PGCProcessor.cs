using PGCExtractor.Core.Models;
using PGCExtractor.Data.Services;

namespace PGCExtractor.Logic.Services
{
    public class PGCProcessor
    {
        private readonly PGCDataExtractor _dataExtractor;

        public PGCProcessor(PGCDataExtractor dataExtractor)
        {
            _dataExtractor = dataExtractor;
        }

        public async Task<IEnumerable<PGCEntity>> ProcessRawPGCDataAsync(string rawData, string format)
        {
            // Placeholder: Orchestrate data extraction and apply logic
            IEnumerable<PGCEntity> entities;
            if (format == "html")
            {
                entities = await _dataExtractor.ExtractFromHtmlAsync(rawData);
            }
            else if (format == "json")
            {
                entities = await _dataExtractor.ExtractFromJsonAsync(rawData);
            }
            else
            {
                throw new ArgumentException("Unsupported format", nameof(format));
            }

            // Add any processing/classification logic here
            return entities;
        }
    }
}
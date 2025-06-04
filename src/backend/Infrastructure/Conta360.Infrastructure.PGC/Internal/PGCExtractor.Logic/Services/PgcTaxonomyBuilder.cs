using Conta360.Application;
using Conta360.Core.Interfaces;
using PGCExtractor.Logic.Services;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;


namespace PGCExtractor.Logic.Services
{
    public class PGCStructureService : IPGCStructureService
    {
        private readonly PgcProcessor _pgcProcessor;

        public PGCStructureService(PgcProcessor pgcProcessor)
        {
            _pgcProcessor = pgcProcessor;
        }

        public async Task<OperationResult<string>> GetPGCStructureAsync(string year)
        {
            // Simulate fetching raw data (e.g., from an external API or file)
            string rawData = $"{{ \"code\": \"100\", \"description\": \"Example PGC for {year}\" }}";
            return OperationResult<string>.Success(rawData);
        }

        public async Task<OperationResult> ProcessPGCStructureAsync(string rawData)
        {
            // Example of using the internal PGC extractor logic
            var processedEntities = await _pgcProcessor.ProcessRawPGCDataAsync(rawData, "json"); // Assuming JSON format
            if (!processedEntities.Any())
            {
                return OperationResult.Failure(new Error("PGC.ProcessingFailed", "No entities processed from raw data."));
            }

            // Further logic to store or use processedEntities
            return OperationResult.Success();
        }
    }
}
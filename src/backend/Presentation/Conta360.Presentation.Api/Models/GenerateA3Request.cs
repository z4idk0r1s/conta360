namespace Conta360.Presentation.Api.Models
{
    public record GenerateA3Request(string ExcelFilePath, string A3OutputFilename = "SUENLACE_FGLD.DAT");
}
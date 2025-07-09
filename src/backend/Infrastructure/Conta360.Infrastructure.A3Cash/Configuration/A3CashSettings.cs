namespace Conta360.Infrastructure.A3Cash.Configuration
{
    public class A3CashSettings
    {
        public string CompanyCode { get; set; } = string.Empty;
        public string DefaultDebitAccount { get; set; } = string.Empty;
        public string DefaultDebitAccountDescription { get; set; } = string.Empty;
        public string DefaultCreditAccount { get; set; } = string.Empty;
        public string DefaultCreditAccountDescription { get; set; } = string.Empty;
        public string OutputDirectory { get; set; } = "./Data/A3/ficheros_a3";
        public string DefaultDocumentReference { get; set; } = "";
        public string BaseEntryDescription { get; set; } = "Apunte Diario DEFAULT";
    }
}
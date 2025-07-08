namespace Conta360.Infrastructure.Excel.Configuration
{
    public class ExcelSettings
    {
        public string RutaExcel { get; set; } = string.Empty;
        public string HojaResumen { get; set; } = "Sheet1";
        public int FilaInicioResumen { get; set; } = 24;
    }
}
namespace Conta360.Infrastructure.Excel.Models.ResumenFiscal
{
    public record TotalesGenerales
    {
        public decimal BaseImponible4 { get; init; }
        public decimal CuotaIva4 { get; init; }
        public decimal TotalIva4 => BaseImponible4 + CuotaIva4;

        public decimal BaseImponible10 { get; init; }
        public decimal CuotaIva10 { get; init; }
        public decimal TotalIva10 => BaseImponible10 + CuotaIva10;

        public decimal BaseImponible21 { get; init; }
        public decimal CuotaIva21 { get; init; }
        public decimal TotalIva21 => BaseImponible21 + CuotaIva21;

        public decimal BaseImponibleTotal => 
            BaseImponible4 + BaseImponible10 + BaseImponible21;
        public decimal CuotaIvaTotal => 
            CuotaIva4 + CuotaIva10 + CuotaIva21;
        public decimal ImporteTotal => 
            BaseImponibleTotal + CuotaIvaTotal;
    }
}
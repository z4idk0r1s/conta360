namespace Conta360.Infrastructure.Adapters.PGCExtractor.PGCExtractor.Data.Services
{
    public static class AccountUtils
    {
        public static string GetTypeFromCode(string code)
        {
            return code.Length switch
            {
                1 => "Clase",
                2 => "Grupo",
                3 => "Subgrupo",
                4 => "Cuenta",
                _ => "Desconocido"
            };
        }
    }
}

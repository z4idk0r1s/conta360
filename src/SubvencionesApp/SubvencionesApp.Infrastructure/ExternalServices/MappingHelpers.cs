using System;

namespace SubvencionesApp.Infrastructure.ExternalServices
{
    public static class MappingHelpers
    {
        public static Guid StringToGuid(string source)
        {
            return Guid.TryParse(source, out var guid) ? guid : Guid.Empty;
        }

        public static Guid? StringToNullableGuid(string source)
        {
            return Guid.TryParse(source, out var guid) ? guid : (Guid?)null;
        }

        public static DateTime StringToDateTime(string source)
        {
            return DateTime.TryParse(source, out var date) ? date : default;
        }
        
        public static DateTime? StringToNullableDateTime(string source)
        {
            return DateTime.TryParse(source, out var date) ? date : (DateTime?)null;
        }
    }
}
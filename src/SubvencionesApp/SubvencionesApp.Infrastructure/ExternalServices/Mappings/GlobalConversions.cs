using AutoMapper;
using System;

namespace SubvencionesApp.Infrastructure.ExternalServices.Mappings
{
    public partial class MappingProfile : Profile
    {
        // ===========================================
        // 1. Conversiones globales seguras
        // ===========================================
        private void ConfigureGlobalConversions()
        {
            // Estos mapeos son seguros y robustos, ya que usan TryParse
            // para evitar excepciones con datos externos malformados.
            CreateMap<Guid, string>().ConvertUsing(src => src.ToString());
            CreateMap<Guid?, string>().ConvertUsing(src => src.HasValue ? src.Value.ToString() : string.Empty);
            CreateMap<string, Guid>().ConvertUsing(src => Guid.TryParse(src, out var guid) ? guid : Guid.Empty);
            CreateMap<string, Guid?>().ConvertUsing(src => Guid.TryParse(src, out var guid) ? guid : (Guid?)null);

            CreateMap<DateTime, string>().ConvertUsing(src => src.ToString("yyyy-MM-dd"));
            CreateMap<DateTime?, string>().ConvertUsing(src => src.HasValue ? src.Value.ToString("yyyy-MM-dd") : string.Empty);
            CreateMap<string, DateTime>().ConvertUsing(src => DateTime.TryParse(src, out var date) ? date : default);
            CreateMap<string, DateTime?>().ConvertUsing(src => DateTime.TryParse(src, out var date) ? date : (DateTime?)null);
            
            CreateMap<decimal, decimal?>().ConvertUsing(src => src);
            CreateMap<decimal?, decimal>().ConvertUsing(src => src ?? 0m);
            CreateMap<int, int?>().ConvertUsing(src => src);
            CreateMap<int?, int>().ConvertUsing(src => src ?? 0);
        }
    }
}
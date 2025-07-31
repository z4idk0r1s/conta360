using SubvencionesApp.Application.DTOs;
using SubvencionesApp.Application.Interfaces;
using SubvencionesApp.Infrastructure.ExternalServices.Models;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SubvencionesApp.Infrastructure.ExternalServices
{
    public class ExternalSubvencionesService : IExternalSubvencionesService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://www.infosubvenciones.es/bdnstrans/";

        public ExternalSubvencionesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ConvocatoriaDto>> GetConvocatoriasAsync()
        {
            var convocatoriasApi = await GetConvocatoriasFromApiAsync();
            return convocatoriasApi.Select(c => new ConvocatoriaDto
            {
                Id = c.Id,
                Objeto = c.Objeto,
                Extracto = c.Extracto,
                Enlace = c.Enlace,
                ReferenciaBDNS = c.ReferenciaBDNS,
                Ejercicio = c.Ejercicio,
                FechaPublicacion = c.FechaPublicacion,
                TipoConvocatoriaId = c.TipoConvocatoria?.Id,
                TipoSubvencionId = c.TipoSubvencion?.Id,
                OrganismoId = c.Organismo?.Id,
                SituacionEntornoId = c.SituacionEntorno?.Id
            }).ToList();
        }

        private async Task<List<ConvocatoriaApiModel>> GetConvocatoriasFromApiAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}snpsap-api.json");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var jsonDocument = JsonDocument.Parse(jsonString);
            if (jsonDocument.RootElement.TryGetProperty("convocatorias", out var convocatoriasElement))
            {
                var convocatorias = JsonSerializer.Deserialize<List<ConvocatoriaApiModel>>(
                    convocatoriasElement.GetRawText(), options);
                return convocatorias ?? new List<ConvocatoriaApiModel>();
            }

            return new List<ConvocatoriaApiModel>();
        }

        // Implementar métodos similares para otras entidades...
    }
}
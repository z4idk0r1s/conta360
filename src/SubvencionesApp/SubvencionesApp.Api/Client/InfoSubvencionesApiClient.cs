using SubvencionesApp.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace SubvencionesApp.Api.Client
{
    public class InfoSubvencionesApiClient
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://www.infosubvenciones.es/bdnstrans/";

        public InfoSubvencionesApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<AccionApiModel>> GetAccionesAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}snpsap-api.json");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var jsonDocument = JsonDocument.Parse(jsonString);
            if (jsonDocument.RootElement.TryGetProperty("acciones", out var accionesElement))
            {
                var acciones = JsonSerializer.Deserialize<List<AccionApiModel>>(accionesElement.GetRawText(), options);
                return acciones ?? new List<AccionApiModel>();
            }

            return new List<AccionApiModel>();
        }

        public async Task<List<AgrupacionApiModel>> GetAgrupacionesAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}snpsap-api.json");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var jsonDocument = JsonDocument.Parse(jsonString);
            if (jsonDocument.RootElement.TryGetProperty("agrupaciones", out var agrupacionesElement))
            {
                var agrupaciones = JsonSerializer.Deserialize<List<AgrupacionApiModel>>(agrupacionesElement.GetRawText(), options);
                return agrupaciones ?? new List<AgrupacionApiModel>();
            }

            return new List<AgrupacionApiModel>();
        }

        public async Task<List<AreaApiModel>> GetAreasAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}snpsap-api.json");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var jsonDocument = JsonDocument.Parse(jsonString);
            if (jsonDocument.RootElement.TryGetProperty("areas", out var areasElement))
            {
                var areas = JsonSerializer.Deserialize<List<AreaApiModel>>(areasElement.GetRawText(), options);
                return areas ?? new List<AreaApiModel>();
            }

            return new List<AreaApiModel>();
        }
        
        public async Task<List<BeneficiarioApiModel>> GetBeneficiariosAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}snpsap-api.json");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var jsonDocument = JsonDocument.Parse(jsonString);
            if (jsonDocument.RootElement.TryGetProperty("beneficiarios", out var beneficiariosElement))
            {
                var beneficiarios = JsonSerializer.Deserialize<List<BeneficiarioApiModel>>(beneficiariosElement.GetRawText(), options);
                return beneficiarios ?? new List<BeneficiarioApiModel>();
            }

            return new List<BeneficiarioApiModel>();
        }
        
        public async Task<List<ConcesionApiModel>> GetConcesionesAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}snpsap-api.json");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var jsonDocument = JsonDocument.Parse(jsonString);
            if (jsonDocument.RootElement.TryGetProperty("concesiones", out var concesionesElement))
            {
                var concesiones = JsonSerializer.Deserialize<List<ConcesionApiModel>>(concesionesElement.GetRawText(), options);
                return concesiones ?? new List<ConcesionApiModel>();
            }

            return new List<ConcesionApiModel>();
        }

        public async Task<List<ConvocatoriaApiModel>> GetConvocatoriasAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}snpsap-api.json");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var jsonDocument = JsonDocument.Parse(jsonString);
            if (jsonDocument.RootElement.TryGetProperty("convocatorias", out var convocatoriasElement))
            {
                var convocatorias = JsonSerializer.Deserialize<List<ConvocatoriaApiModel>>(convocatoriasElement.GetRawText(), options);
                return convocatorias ?? new List<ConvocatoriaApiModel>();
            }

            return new List<ConvocatoriaApiModel>();
        }

        public async Task<List<DatosEstadisticosApiModel>> GetDatosEstadisticosAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}snpsap-api.json");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var jsonDocument = JsonDocument.Parse(jsonString);
            if (jsonDocument.RootElement.TryGetProperty("datosEstadisticos", out var datosEstadisticosElement))
            {
                var datosEstadisticos = JsonSerializer.Deserialize<List<DatosEstadisticosApiModel>>(datosEstadisticosElement.GetRawText(), options);
                return datosEstadisticos ?? new List<DatosEstadisticosApiModel>();
            }

            return new List<DatosEstadisticosApiModel>();
        }

        public async Task<List<EntidadApiModel>> GetEntidadesAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}snpsap-api.json");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var jsonDocument = JsonDocument.Parse(jsonString);
            if (jsonDocument.RootElement.TryGetProperty("entidades", out var entidadesElement))
            {
                var entidades = JsonSerializer.Deserialize<List<EntidadApiModel>>(entidadesElement.GetRawText(), options);
                return entidades ?? new List<EntidadApiModel>();
            }

            return new List<EntidadApiModel>();
        }

        public async Task<List<EstadoApiModel>> GetEstadosAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}snpsap-api.json");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var jsonDocument = JsonDocument.Parse(jsonString);
            if (jsonDocument.RootElement.TryGetProperty("estados", out var estadosElement))
            {
                var estados = JsonSerializer.Deserialize<List<EstadoApiModel>>(estadosElement.GetRawText(), options);
                return estados ?? new List<EstadoApiModel>();
            }

            return new List<EstadoApiModel>();
        }

        public async Task<List<FormaPagoApiModel>> GetFormasPagoAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}snpsap-api.json");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var jsonDocument = JsonDocument.Parse(jsonString);
            if (jsonDocument.RootElement.TryGetProperty("formasPago", out var formasPagoElement))
            {
                var formasPago = JsonSerializer.Deserialize<List<FormaPagoApiModel>>(formasPagoElement.GetRawText(), options);
                return formasPago ?? new List<FormaPagoApiModel>();
            }

            return new List<FormaPagoApiModel>();
        }

        public async Task<List<LineaApiModel>> GetLineasAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}snpsap-api.json");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var jsonDocument = JsonDocument.Parse(jsonString);
            if (jsonDocument.RootElement.TryGetProperty("lineas", out var lineasElement))
            {
                var lineas = JsonSerializer.Deserialize<List<LineaApiModel>>(lineasElement.GetRawText(), options);
                return lineas ?? new List<LineaApiModel>();
            }

            return new List<LineaApiModel>();
        }

        public async Task<List<MunicipioApiModel>> GetMunicipiosAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}snpsap-api.json");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var jsonDocument = JsonDocument.Parse(jsonString);
            if (jsonDocument.RootElement.TryGetProperty("municipios", out var municipiosElement))
            {
                var municipios = JsonSerializer.Deserialize<List<MunicipioApiModel>>(municipiosElement.GetRawText(), options);
                return municipios ?? new List<MunicipioApiModel>();
            }

            return new List<MunicipioApiModel>();
        }

        public async Task<List<OrganismoApiModel>> GetOrganismosAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}snpsap-api.json");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var jsonDocument = JsonDocument.Parse(jsonString);
            if (jsonDocument.RootElement.TryGetProperty("organismos", out var organismosElement))
            {
                var organismos = JsonSerializer.Deserialize<List<OrganismoApiModel>>(organismosElement.GetRawText(), options);
                return organismos ?? new List<OrganismoApiModel>();
            }

            return new List<OrganismoApiModel>();
        }

        public async Task<List<ProgramaApiModel>> GetProgramasAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}snpsap-api.json");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var jsonDocument = JsonDocument.Parse(jsonString);
            if (jsonDocument.RootElement.TryGetProperty("programas", out var programasElement))
            {
                var programas = JsonSerializer.Deserialize<List<ProgramaApiModel>>(programasElement.GetRawText(), options);
                return programas ?? new List<ProgramaApiModel>();
            }

            return new List<ProgramaApiModel>();
        }

        public async Task<List<ProvinciaApiModel>> GetProvinciasAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}snpsap-api.json");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var jsonDocument = JsonDocument.Parse(jsonString);
            if (jsonDocument.RootElement.TryGetProperty("provincias", out var provinciasElement))
            {
                var provincias = JsonSerializer.Deserialize<List<ProvinciaApiModel>>(provinciasElement.GetRawText(), options);
                return provincias ?? new List<ProvinciaApiModel>();
            }

            return new List<ProvinciaApiModel>();
        }

        public async Task<List<SectorApiModel>> GetSectoresAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}snpsap-api.json");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var jsonDocument = JsonDocument.Parse(jsonString);
            if (jsonDocument.RootElement.TryGetProperty("sectores", out var sectoresElement))
            {
                var sectores = JsonSerializer.Deserialize<List<SectorApiModel>>(sectoresElement.GetRawText(), options);
                return sectores ?? new List<SectorApiModel>();
            }

            return new List<SectorApiModel>();
        }

        public async Task<List<SituacionEntornoApiModel>> GetSituacionesEntornoAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}snpsap-api.json");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var jsonDocument = JsonDocument.Parse(jsonString);
            if (jsonDocument.RootElement.TryGetProperty("situacionesEntorno", out var situacionesEntornoElement))
            {
                var situacionesEntorno = JsonSerializer.Deserialize<List<SituacionEntornoApiModel>>(situacionesEntornoElement.GetRawText(), options);
                return situacionesEntorno ?? new List<SituacionEntornoApiModel>();
            }

            return new List<SituacionEntornoApiModel>();
        }

        public async Task<List<SubtipoSubvencionApiModel>> GetSubtiposSubvencionAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}snpsap-api.json");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var jsonDocument = JsonDocument.Parse(jsonString);
            if (jsonDocument.RootElement.TryGetProperty("subtiposSubvencion", out var subtiposSubvencionElement))
            {
                var subtiposSubvencion = JsonSerializer.Deserialize<List<SubtipoSubvencionApiModel>>(subtiposSubvencionElement.GetRawText(), options);
                return subtiposSubvencion ?? new List<SubtipoSubvencionApiModel>();
            }

            return new List<SubtipoSubvencionApiModel>();
        }

        public async Task<List<TipoBeneficiarioApiModel>> GetTiposBeneficiarioAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}snpsap-api.json");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var jsonDocument = JsonDocument.Parse(jsonString);
            if (jsonDocument.RootElement.TryGetProperty("tiposBeneficiario", out var tiposBeneficiarioElement))
            {
                var tiposBeneficiario = JsonSerializer.Deserialize<List<TipoBeneficiarioApiModel>>(tiposBeneficiarioElement.GetRawText(), options);
                return tiposBeneficiario ?? new List<TipoBeneficiarioApiModel>();
            }

            return new List<TipoBeneficiarioApiModel>();
        }

        public async Task<List<TipoConvocatoriaApiModel>> GetTiposConvocatoriaAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}snpsap-api.json");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var jsonDocument = JsonDocument.Parse(jsonString);
            if (jsonDocument.RootElement.TryGetProperty("tiposConvocatoria", out var tiposConvocatoriaElement))
            {
                var tiposConvocatoria = JsonSerializer.Deserialize<List<TipoConvocatoriaApiModel>>(tiposConvocatoriaElement.GetRawText(), options);
                return tiposConvocatoria ?? new List<TipoConvocatoriaApiModel>();
            }

            return new List<TipoConvocatoriaApiModel>();
        }

        public async Task<List<TipoOrganismoApiModel>> GetTiposOrganismoAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}snpsap-api.json");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var jsonDocument = JsonDocument.Parse(jsonString);
            if (jsonDocument.RootElement.TryGetProperty("tiposOrganismo", out var tiposOrganismoElement))
            {
                var tiposOrganismo = JsonSerializer.Deserialize<List<TipoOrganismoApiModel>>(tiposOrganismoElement.GetRawText(), options);
                return tiposOrganismo ?? new List<TipoOrganismoApiModel>();
            }

            return new List<TipoOrganismoApiModel>();
        }

        public async Task<List<TipoSubvencionApiModel>> GetTiposSubvencionAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}snpsap-api.json");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var jsonDocument = JsonDocument.Parse(jsonString);
            if (jsonDocument.RootElement.TryGetProperty("tiposSubvencion", out var tiposSubvencionElement))
            {
                var tiposSubvencion = JsonSerializer.Deserialize<List<TipoSubvencionApiModel>>(tiposSubvencionElement.GetRawText(), options);
                return tiposSubvencion ?? new List<TipoSubvencionApiModel>();
            }

            return new List<TipoSubvencionApiModel>();
        }

        public async Task<List<TramoApiModel>> GetTramosAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}snpsap-api.json");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var jsonDocument = JsonDocument.Parse(jsonString);
            if (jsonDocument.RootElement.TryGetProperty("tramos", out var tramosElement))
            {
                var tramos = JsonSerializer.Deserialize<List<TramoApiModel>>(tramosElement.GetRawText(), options);
                return tramos ?? new List<TramoApiModel>();
            }

            return new List<TramoApiModel>();
        }

        public async Task<List<UnidadAdministrativaApiModel>> GetUnidadesAdministrativasAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}snpsap-api.json");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var jsonDocument = JsonDocument.Parse(jsonString);
            if (jsonDocument.RootElement.TryGetProperty("unidadesAdministrativas", out var unidadesAdministrativasElement))
            {
                var unidadesAdministrativas = JsonSerializer.Deserialize<List<UnidadAdministrativaApiModel>>(unidadesAdministrativasElement.GetRawText(), options);
                return unidadesAdministrativas ?? new List<UnidadAdministrativaApiModel>();
            }

            return new List<UnidadAdministrativaApiModel>();
        }
    }
}
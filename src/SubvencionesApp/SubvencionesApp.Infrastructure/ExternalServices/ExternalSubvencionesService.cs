/*
* // ----------------------------------------------
* // Mapeos (conversión ApiModel -> Dto)
* // ----------------------------------------------

private static ConvocatoriaDto MapConvocatoriaApiModelToDto(ConvocatoriaApiModel apiModel) => new() { Id = apiModel.Id, Nombre = apiModel.Nombre, FechaInicio = apiModel.FechaInicio, FechaFin = apiModel.FechaFin, Estado = apiModel.Estado, OrganismoId = apiModel.OrganismoId, RegionId = apiModel.RegionId, TipoBeneficiarioId = apiModel.TipoBeneficiarioId, InstrumentoId = apiModel.InstrumentoId };
private static ConvocatoriaDetalleDto MapConvocatoriaDetalleApiModelToDto(ConvocatoriaDetalleApiModel apiModel) => new() { Id = apiModel.Id, Nombre = apiModel.Nombre, Descripcion = apiModel.Descripcion, Detalles = apiModel.Detalles, Estado = apiModel.Estado, FechaInicio = apiModel.FechaInicio, FechaFin = apiModel.FechaFin, FechaPublicacion = apiModel.FechaPublicacion, OrganismoId = apiModel.OrganismoId, RegionId = apiModel.RegionId, TipoBeneficiarioId = apiModel.TipoBeneficiarioId, InstrumentoId = apiModel.InstrumentoId, Plazos = apiModel.Plazos?.Select(MapPlazoApiModelToDto).ToList() ?? new List<PlazoDto>(), Documentos = apiModel.Documentos, Beneficiarios = apiModel.Beneficiarios?.Select(MapBeneficiarioApiModelToDto).ToList() ?? new List<BeneficiarioDto>() };
private static ConcesionDto MapConcesionApiModelToDto(ConcesionApiModel apiModel) => new() { Id = apiModel.Id, Nombre = apiModel.Nombre, Estado = apiModel.Estado, OrganismoId = apiModel.OrganismoId, BeneficiarioId = apiModel.BeneficiarioId, Importe = apiModel.Importe, FechaResolucion = apiModel.FechaResolucion };
private static ConcesionDetalleDto MapConcesionDetalleApiModelToDto(ConcesionDetalleApiModel apiModel) => new() { Id = apiModel.Id, Nombre = apiModel.Nombre, Descripcion = apiModel.Descripcion, Detalles = apiModel.Detalles, Importe = apiModel.Importe, BeneficiarioId = apiModel.BeneficiarioId, OrganismoId = apiModel.OrganismoId, FechaResolucion = apiModel.FechaResolucion };
private static AyudaEstadoDto MapAyudaEstadoApiModelToDto(AyudaEstadoApiModel apiModel) => new() { Id = apiModel.Id, Nombre = apiModel.Nombre, Descripcion = apiModel.Descripcion, InstrumentoId = apiModel.InstrumentoId, TipoBeneficiarioId = apiModel.TipoBeneficiarioId, Estado = apiModel.Estado };
private static AyudaDto MapAyudaApiModelToDto(AyudaApiModel apiModel) => new() { Id = apiModel.Id, Nombre = apiModel.Nombre, Descripcion = apiModel.Descripcion, OrganismoId = apiModel.OrganismoId, RegionId = apiModel.RegionId, TipoBeneficiarioId = apiModel.TipoBeneficiarioId, InstrumentoId = apiModel.InstrumentoId };
private static PlazoDto MapPlazoApiModelToDto(PlazoApiModel apiModel) => new() { Id = apiModel.Id, Nombre = apiModel.Nombre, FechaInicio = apiModel.FechaInicio, FechaFin = apiModel.FechaFin, ConvocatoriaId = apiModel.ConvocatoriaId };
private static MinimisDto MapMinimisApiModelToDto(MinimisApiModel apiModel) => new() { Id = apiModel.Id, Nombre = apiModel.Nombre, Descripcion = apiModel.Descripcion, Estado = apiModel.Estado, FechaInicio = apiModel.FechaInicio, FechaFin = apiModel.FechaFin };
private static GrandeBeneficiarioDto MapGrandeBeneficiarioApiModelToDto(GrandeBeneficiarioApiModel apiModel) => new() { Id = apiModel.Id, Nombre = apiModel.Nombre, Tipo = apiModel.Tipo, Importe = apiModel.Importe };
private static PartidoPoliticoDto MapPartidoPoliticoApiModelToDto(PartidoPoliticoApiModel apiModel) => new() { Id = apiModel.Id, Nombre = apiModel.Nombre, Importe = apiModel.Importe, Fecha = apiModel.Fecha, OrganismoId = apiModel.OrganismoId };
private static PlanEstrategicoDto MapPlanEstrategicoApiModelToDto(PlanEstrategicoApiModel apiModel) => new() { Id = apiModel.Id, Nombre = apiModel.Nombre, Descripcion = apiModel.Descripcion, Estado = apiModel.Estado, FechaAprobacion = apiModel.FechaAprobacion };
private static PlanEstrategicoDetalleDto MapPlanEstrategicoDetalleApiModelToDto(PlanEstrategicoDetalleApiModel apiModel) => new() { Id = apiModel.Id, Nombre = apiModel.Nombre, Descripcion = apiModel.Descripcion, Estado = apiModel.Estado, FechaAprobacion = apiModel.FechaAprobacion, Objetivos = apiModel.Objetivos?.Select(MapObjetivoApiModelToDto).ToList() ?? new List<ObjetivoDto>() };
private static SancionDto MapSancionApiModelToDto(SancionApiModel apiModel) => new() { Id = apiModel.Id, Nombre = apiModel.Nombre, Motivo = apiModel.Motivo, SancionTexto = apiModel.SancionTexto, Estado = apiModel.Estado };
private static SancionDetalleDto MapSancionDetalleApiModelToDto(SancionDetalleApiModel apiModel) => new() { Id = apiModel.Id, Nombre = apiModel.Nombre, Motivo = apiModel.Motivo, SancionTexto = apiModel.SancionTexto, Estado = apiModel.Estado, Detalles = apiModel.Detalles, FechaResolucion = apiModel.FechaResolucion, OrganismoId = apiModel.OrganismoId };
private static TerceroDto MapTerceroApiModelToDto(TerceroApiModel apiModel) => new() { Id = apiModel.Id, Nombre = apiModel.Nombre, Nif = apiModel.Nif, Tipo = apiModel.Tipo };
private static RegionDto MapRegionApiModelToDto(RegionApiModel apiModel) => new() { Id = apiModel.Id, Nombre = apiModel.Nombre };
private static FinalidadDto MapFinalidadApiModelToDto(FinalidadApiModel apiModel) => new() { Id = apiModel.Id, Nombre = apiModel.Nombre, Descripcion = apiModel.Descripcion };
private static TipoBeneficiarioDto MapTipoBeneficiarioApiModelToDto(TipoBeneficiarioApiModel apiModel) => new() { Id = apiModel.Id, Nombre = apiModel.Nombre, Descripcion = apiModel.Descripcion };
private static InstrumentoDto MapInstrumentoApiModelToDto(InstrumentoApiModel apiModel) => new() { Id = apiModel.Id, Nombre = apiModel.Nombre, Descripcion = apiModel.Descripcion };
private static ReglamentoDto MapReglamentoApiModelToDto(ReglamentoApiModel apiModel) => new() { Id = apiModel.Id, Nombre = apiModel.Nombre, Tipo = apiModel.Tipo };
private static SectorProductoDto MapSectorProductoApiModelToDto(SectorProductoApiModel apiModel) => new() { Id = apiModel.Id, Nombre = apiModel.Nombre, Descripcion = apiModel.Descripcion };
private static ActividadDto MapActividadApiModelToDto(ActividadApiModel apiModel) => new() { Id = apiModel.Id, Nombre = apiModel.Nombre, Descripcion = apiModel.Descripcion };
private static ObjetivoDto MapObjetivoApiModelToDto(ObjetivoApiModel apiModel) => new() { Id = apiModel.Id, Nombre = apiModel.Nombre, Descripcion = apiModel.Descripcion };
private static OrganoDto MapOrganoApiModelToDto(OrganoApiModel apiModel) => new() { Id = apiModel.Id, Nombre = apiModel.Nombre };
private static OrganosCodigoAdminDto MapOrganosCodigoAdminApiModelToDto(OrganosCodigoAdminApiModel apiModel) => new() { Id = apiModel.Id, CodigoAdmin = apiModel.CodigoAdmin, Nombre = apiModel.Nombre };
private static ConfiguracionMicroportalDto MapConfiguracionMicroportalApiModelToDto(ConfiguracionMicroportalApiModel apiModel) => new() { Vpd = apiModel.Vpd, NombrePortal = apiModel.NombrePortal, Logo = apiModel.Logo };
private static EnlaceMicroVentanaDto MapEnlaceMicroVentanaApiModelToDto(EnlaceMicroVentanaApiModel apiModel) => new() { Id = apiModel.Id, Nombre = apiModel.Nombre, Url = apiModel.Url };
private static SuscripcionDto MapSuscripcionApiModelToDto(SuscripcionApiModel apiModel) => new() { Id = apiModel.Id, Nombre = apiModel.Nombre, Email = apiModel.Email, FechaInicio = apiModel.FechaInicio, Activa = apiModel.Activa };
private static BeneficiarioDto MapBeneficiarioApiModelToDto(BeneficiarioApiModel apiModel) => new() { Id = apiModel.Id, Nombre = apiModel.Nombre, Tipo = apiModel.Tipo };
*/

using AutoMapper;
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Infrastructure.ExternalServices.Models;
using SubvencionesApp.Application.Interfaces;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http.Headers;

namespace SubvencionesApp.Infrastructure.ExternalServices
{
    public class ExternalSubvencionesService : IExternalSubvencionesService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private const string BaseUrl = "https://www.infosubvenciones.es/bdnstrans/api/";

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public ExternalSubvencionesService(HttpClient httpClient, IMapper mapper)
        {
            _httpClient = httpClient;
            _mapper = mapper;
        }

        private async Task<List<TModel>> GetFromApiAsync<TModel>(
            string endpoint,
            Dictionary<string, string>? routeParams = null,
            Dictionary<string, string>? queryParams = null,
            string? rootProperty = null)
        {
            var finalEndpoint = BuildEndpoint(endpoint, routeParams, queryParams);
            var response = await _httpClient.GetAsync($"{BaseUrl}{finalEndpoint}");
            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync();

            if (string.IsNullOrEmpty(rootProperty))
            {
                return await JsonSerializer.DeserializeAsync<List<TModel>>(stream, JsonOptions) ?? new List<TModel>();
            }

            using var doc = await JsonDocument.ParseAsync(stream);
            if (doc.RootElement.TryGetProperty(rootProperty, out var element))
            {
                return JsonSerializer.Deserialize<List<TModel>>(element.GetRawText(), JsonOptions) ?? new List<TModel>();
            }

            return new List<TModel>();
        }

        private async Task<TModel?> GetSingleFromApiAsync<TModel>(
            string endpoint,
            Dictionary<string, string>? routeParams = null,
            Dictionary<string, string>? queryParams = null)
        {
            var finalEndpoint = BuildEndpoint(endpoint, routeParams, queryParams);
            var response = await _httpClient.GetAsync($"{BaseUrl}{finalEndpoint}");
            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<TModel>(stream, JsonOptions);
        }

        private async Task<byte[]> GetFileFromApiAsync(
            string endpoint,
            Dictionary<string, string>? routeParams = null,
            Dictionary<string, string>? queryParams = null)
        {
            var finalEndpoint = BuildEndpoint(endpoint, routeParams, queryParams);
            var response = await _httpClient.GetAsync($"{BaseUrl}{finalEndpoint}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync();
        }

        private async Task SendApiRequestAsync(HttpMethod method, string endpoint, object? body = null, string? token = null)
        {
            using var request = new HttpRequestMessage(method, $"{BaseUrl}{endpoint}");

            if (body != null)
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(body, JsonOptions), System.Text.Encoding.UTF8, "application/json");
                request.Content = jsonContent;
            }

            if (token != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        // Método auxiliar para construir endpoints
        private static string BuildEndpoint(string endpoint, Dictionary<string, string>? routeParams, Dictionary<string, string>? queryParams)
        {
            if (routeParams != null)
            {
                foreach (var param in routeParams)
                {
                    endpoint = endpoint.Replace($"{{{param.Key}}}", HttpUtility.UrlEncode(param.Value));
                }
            }

            if (queryParams != null && queryParams.Any())
            {
                var queryString = string.Join("&", queryParams.Select(kv => $"{HttpUtility.UrlEncode(kv.Key)}={HttpUtility.UrlEncode(kv.Value)}"));
                endpoint += endpoint.Contains("?") ? "&" + queryString : "?" + queryString;
            }

            return endpoint;
        }

        private static void AddIfNotNull(Dictionary<string, string> dict, string key, string? value)
        {
            if (value != null)
            {
                dict.Add(key, value);
            }
        }
        
        // ----------------------------------------------
        // CONVOCATORIAS
        // ----------------------------------------------

        public async Task<List<ConvocatoriaDto>> GetConvocatoriasPortalBusquedaAsync(Dictionary<string, string>? queryParams = null)
        {
            var results = await GetFromApiAsync<ConvocatoriaApiModel>("convocatorias/busqueda", null, queryParams, "content");
            return _mapper.Map<List<ConvocatoriaDto>>(results);
        }

        public async Task<List<ConvocatoriaDto>> BuscarConvocatoriasAvanzadoAsync(
            string? nombre = null, string? organismoId = null, string? regionId = null,
            string? tipoBeneficiarioId = null, string? instrumentoId = null, string? estado = null,
            string? fechaInicioDesde = null, string? fechaInicioHasta = null,
            string? fechaFinDesde = null, string? fechaFinHasta = null,
            int? pagina = null, int? tamanioPagina = null)
        {
            var filtros = new Dictionary<string, string>();
            AddIfNotNull(filtros, "nombre", nombre);
            AddIfNotNull(filtros, "organismo", organismoId);
            AddIfNotNull(filtros, "region", regionId);
            AddIfNotNull(filtros, "tipoBeneficiario", tipoBeneficiarioId);
            AddIfNotNull(filtros, "instrumento", instrumentoId);
            AddIfNotNull(filtros, "estado", estado);
            AddIfNotNull(filtros, "fechaInicioDesde", fechaInicioDesde);
            AddIfNotNull(filtros, "fechaInicioHasta", fechaInicioHasta);
            AddIfNotNull(filtros, "fechaFinDesde", fechaFinDesde);
            AddIfNotNull(filtros, "fechaFinHasta", fechaFinHasta);
            AddIfNotNull(filtros, "pagina", pagina?.ToString());
            AddIfNotNull(filtros, "tamanioPagina", tamanioPagina?.ToString());

            var results = await GetFromApiAsync<ConvocatoriaApiModel>("convocatorias/busqueda", null, filtros, "convocatorias");
            return _mapper.Map<List<ConvocatoriaDto>>(results);
        }

        public async Task<List<ConvocatoriaDto>> GetConvocatoriaUltimasPortalBusquedaAsync(Dictionary<string, string>? queryParams = null)
        {
            var results = await GetFromApiAsync<ConvocatoriaApiModel>("convocatorias/ultimas", null, queryParams, "content");
            return _mapper.Map<List<ConvocatoriaDto>>(results);
        }

        public async Task<ConvocatoriaDetalleDto?> GetConvocatoriaAsync(string numConv, string? vpd = null)
        {
            var queryParams = new Dictionary<string, string> { { "numConv", numConv } };
            AddIfNotNull(queryParams, "vpd", vpd);
            var result = await GetSingleFromApiAsync<ConvocatoriaDetalleApiModel>("convocatorias", null, queryParams);
            return result != null ? _mapper.Map<ConvocatoriaDetalleDto>(result) : null;
        }

        public async Task<byte[]> GetDocumentoConvocatoriaAsync(int idDocumento)
        {
            var queryParams = new Dictionary<string, string> { { "idDocumento", idDocumento.ToString() } };
            return await GetFileFromApiAsync("convocatorias/documentos", null, queryParams);
        }

        public async Task<byte[]> ExportarConvocatoriasAsync(string tipoDoc, Dictionary<string, string>? queryParams = null)
        {
            queryParams ??= new Dictionary<string, string>();
            queryParams["tipoDoc"] = tipoDoc;
            return await GetFileFromApiAsync("convocatorias/exportar", null, queryParams);
        }

        // ----------------------------------------------
        // CONCESIONES
        // ----------------------------------------------

        public async Task<List<ConcesionDto>> GetConcesionPortalBusquedaAsync(Dictionary<string, string>? queryParams = null)
        {
            var results = await GetFromApiAsync<ConcesionApiModel>("concesiones/busqueda", null, queryParams, "content");
            return _mapper.Map<List<ConcesionDto>>(results);
        }

        public async Task<ConcesionDetalleDto?> GetConcesionAsync(string idConcesion, string? vpd = null)
        {
            var queryParams = new Dictionary<string, string> { { "idConcesion", idConcesion } };
            AddIfNotNull(queryParams, "vpd", vpd);
            var result = await GetSingleFromApiAsync<ConcesionDetalleApiModel>("concesiones", null, queryParams);
            return result != null ? _mapper.Map<ConcesionDetalleDto>(result) : null;
        }

        public async Task<byte[]> GetDocumentoConcesionAsync(int idDocumento)
        {
            var queryParams = new Dictionary<string, string> { { "idDocumento", idDocumento.ToString() } };
            return await GetFileFromApiAsync("concesiones/documentos", null, queryParams);
        }

        public async Task<byte[]> ExportarConcesionesAsync(string tipoDoc, Dictionary<string, string>? queryParams = null)
        {
            queryParams ??= new Dictionary<string, string>();
            queryParams["tipoDoc"] = tipoDoc;
            return await GetFileFromApiAsync("concesiones/exportar", null, queryParams);
        }

        // ----------------------------------------------
        // AYUDAS DE ESTADO
        // ----------------------------------------------

        public async Task<List<AyudaEstadoDto>> GetAyudasEstadoAsync(Dictionary<string, string>? queryParams = null)
        {
            var results = await GetFromApiAsync<AyudaEstadoApiModel>("ayudasEstado", null, queryParams, "content");
            return _mapper.Map<List<AyudaEstadoDto>>(results);
        }

        public async Task<List<AyudaDto>> BuscarAyudasAvanzadoAsync(
            string? nombre = null, string? organismoId = null, string? regionId = null,
            string? tipoBeneficiarioId = null, string? instrumentoId = null, string? estado = null,
            string? fechaInicioDesde = null, string? fechaInicioHasta = null,
            string? fechaFinDesde = null, string? fechaFinHasta = null,
            int? pagina = null, int? tamanioPagina = null)
        {
            var filtros = new Dictionary<string, string>();
            AddIfNotNull(filtros, "nombre", nombre);
            AddIfNotNull(filtros, "organismo", organismoId);
            AddIfNotNull(filtros, "region", regionId);
            AddIfNotNull(filtros, "tipoBeneficiario", tipoBeneficiarioId);
            AddIfNotNull(filtros, "instrumento", instrumentoId);
            AddIfNotNull(filtros, "estado", estado);
            AddIfNotNull(filtros, "fechaInicioDesde", fechaInicioDesde);
            AddIfNotNull(filtros, "fechaInicioHasta", fechaInicioHasta);
            AddIfNotNull(filtros, "fechaFinDesde", fechaFinDesde);
            AddIfNotNull(filtros, "fechaFinHasta", fechaFinHasta);
            AddIfNotNull(filtros, "pagina", pagina?.ToString());
            AddIfNotNull(filtros, "tamanioPagina", tamanioPagina?.ToString());

            var results = await GetFromApiAsync<AyudaApiModel>("ayudas/busqueda", null, filtros, "ayudas");
            return _mapper.Map<List<AyudaDto>>(results);
        }

        public async Task<byte[]> ExportarAyudasEstadoAsync(string tipoDoc, Dictionary<string, string>? queryParams = null)
        {
            queryParams ??= new Dictionary<string, string>();
            queryParams["tipoDoc"] = tipoDoc;
            return await GetFileFromApiAsync("ayudasEstado/exportar", null, queryParams);
        }

        // ----------------------------------------------
        // MINIMIS
        // ----------------------------------------------

        public async Task<List<MinimisDto>> GetMinimisAsync(Dictionary<string, string>? queryParams = null)
        {
            var results = await GetFromApiAsync<MinimisApiModel>("minimis", null, queryParams, "content");
            return _mapper.Map<List<MinimisDto>>(results);
        }

        public async Task<byte[]> ExportarMinimisAsync(string tipoDoc, Dictionary<string, string>? queryParams = null)
        {
            queryParams ??= new Dictionary<string, string>();
            queryParams["tipoDoc"] = tipoDoc;
            return await GetFileFromApiAsync("minimis/exportar", null, queryParams);
        }

        // ----------------------------------------------
        // GRANDES BENEFICIARIOS
        // ----------------------------------------------

        public async Task<List<GrandeBeneficiarioDto>> GetGrandesBeneficiariosAsync(Dictionary<string, string>? queryParams = null)
        {
            var results = await GetFromApiAsync<GrandeBeneficiarioApiModel>("grandesBeneficiarios", null, queryParams, "content");
            return _mapper.Map<List<GrandeBeneficiarioDto>>(results);
        }

        public async Task<byte[]> ExportarGrandesBeneficiariosAsync(string tipoDoc, Dictionary<string, string>? queryParams = null)
        {
            queryParams ??= new Dictionary<string, string>();
            queryParams["tipoDoc"] = tipoDoc;
            return await GetFileFromApiAsync("grandesBeneficiarios/exportar", null, queryParams);
        }

        // ----------------------------------------------
        // PARTIDOS POLÍTICOS
        // ----------------------------------------------

        public async Task<List<PartidoPoliticoDto>> GetPartidosPoliticosAsync(Dictionary<string, string>? queryParams = null)
        {
            var results = await GetFromApiAsync<PartidoPoliticoApiModel>("partidosPoliticos", null, queryParams, "content");
            return _mapper.Map<List<PartidoPoliticoDto>>(results);
        }

        public async Task<byte[]> ExportarConcesionesPartidosPoliticosAsync(string tipoDoc, Dictionary<string, string>? queryParams = null)
        {
            queryParams ??= new Dictionary<string, string>();
            queryParams["tipoDoc"] = tipoDoc;
            return await GetFileFromApiAsync("partidosPoliticos/exportar", null, queryParams);
        }

        // ----------------------------------------------
        // PLANES ESTRATÉGICOS
        // ----------------------------------------------

        public async Task<List<PlanEstrategicoDto>> GetPlanesEstrategicosAsync(Dictionary<string, string>? queryParams = null)
        {
            var results = await GetFromApiAsync<PlanEstrategicoApiModel>("planesEstrategicos", null, queryParams, "content");
            return _mapper.Map<List<PlanEstrategicoDto>>(results);
        }

        public async Task<PlanEstrategicoDetalleDto?> GetPlanEstrategicoAsync(int idPlan)
        {
            var result = await GetSingleFromApiAsync<PlanEstrategicoDetalleApiModel>("planesEstrategicos/{idPlan}", new Dictionary<string, string> { { "idPlan", idPlan.ToString() } });
            return result != null ? _mapper.Map<PlanEstrategicoDetalleDto>(result) : null;
        }

        public async Task<byte[]> GetDocumentoPlanEstrategicoAsync(int idDocumento)
        {
            var queryParams = new Dictionary<string, string> { { "idDocumento", idDocumento.ToString() } };
            return await GetFileFromApiAsync("planesEstrategicos/documentos", null, queryParams);
        }

        // ----------------------------------------------
        // SANCIONES
        // ----------------------------------------------

        public async Task<List<SancionDto>> GetSancionesAsync(Dictionary<string, string>? queryParams = null)
        {
            var results = await GetFromApiAsync<SancionApiModel>("sanciones", null, queryParams, "content");
            return _mapper.Map<List<SancionDto>>(results);
        }

        public async Task<SancionDetalleDto?> GetSancionAsync(int idSancion, string? vpd = null)
        {
            var queryParams = new Dictionary<string, string> { { "idSancion", idSancion.ToString() } };
            AddIfNotNull(queryParams, "vpd", vpd);
            var result = await GetSingleFromApiAsync<SancionDetalleApiModel>("sanciones", null, queryParams);
            return result != null ? _mapper.Map<SancionDetalleDto>(result) : null;
        }

        // ----------------------------------------------
        // TERCEROS
        // ----------------------------------------------

        public async Task<List<TerceroDto>> GetTercerosVPDAsync(string? vpd = null, string? ambito = null, string? busqueda = null)
        {
            var queryParams = new Dictionary<string, string>();
            AddIfNotNull(queryParams, "vpd", vpd);
            AddIfNotNull(queryParams, "ambito", ambito);
            AddIfNotNull(queryParams, "busqueda", busqueda);

            var results = await GetFromApiAsync<TerceroApiModel>("terceros", null, queryParams, "content");
            return _mapper.Map<List<TerceroDto>>(results);
        }

        // ----------------------------------------------
        // CATÁLOGOS
        // ----------------------------------------------

        public async Task<List<RegionDto>> GetRegionesAsync() => _mapper.Map<List<RegionDto>>(await GetFromApiAsync<RegionApiModel>("regiones"));
        public async Task<List<FinalidadDto>> GetFinalidadesAsync() => _mapper.Map<List<FinalidadDto>>(await GetFromApiAsync<FinalidadApiModel>("finalidades"));
        public async Task<List<TipoBeneficiarioDto>> GetTiposBeneficiarioAsync() => _mapper.Map<List<TipoBeneficiarioDto>>(await GetFromApiAsync<TipoBeneficiarioApiModel>("tiposBeneficiario"));
        public async Task<List<InstrumentoDto>> GetInstrumentosAyudaAsync() => _mapper.Map<List<InstrumentoDto>>(await GetFromApiAsync<InstrumentoApiModel>("instrumentosAyuda"));
        public async Task<List<ReglamentoDto>> GetReglamentosAsync() => _mapper.Map<List<ReglamentoDto>>(await GetFromApiAsync<ReglamentoApiModel>("reglamentos"));
        public async Task<List<SectorProductoDto>> GetSectoresProductoAsync() => _mapper.Map<List<SectorProductoDto>>(await GetFromApiAsync<SectorProductoApiModel>("sectoresProducto"));
        public async Task<List<ActividadDto>> GetActividadesNACEAsync() => _mapper.Map<List<ActividadDto>>(await GetFromApiAsync<ActividadApiModel>("actividadesNACE"));
        public async Task<List<ObjetivoDto>> GetObjetivosAsync() => _mapper.Map<List<ObjetivoDto>>(await GetFromApiAsync<ObjetivoApiModel>("objetivos"));
        public async Task<List<OrganismoDto>> GetOrganosAsync() => _mapper.Map<List<OrganismoDto>>(await GetFromApiAsync<OrganismoApiModel>("organos"));

        public async Task<OrganosCodigoAdminDto?> GetOrganosByCodigoAdminAsync(string codigoAdmin)
        {
            var result = await GetSingleFromApiAsync<OrganosCodigoAdminApiModel>("organos/codigoAdmin", null, new Dictionary<string, string> { { "codigoAdmin", codigoAdmin } });
            return result != null ? _mapper.Map<OrganosCodigoAdminDto>(result) : null;
        }

        // ----------------------------------------------
        // MICROPOTAL
        // ----------------------------------------------

        public async Task<ConfiguracionMicroportalDto?> GetConfiguracionVPDAsync(string vpd)
        {
            var result = await GetSingleFromApiAsync<ConfiguracionMicroportalApiModel>("vpd/{vpd}/configuracion", new Dictionary<string, string> { { "vpd", vpd } });
            return result != null ? _mapper.Map<ConfiguracionMicroportalDto>(result) : null;
        }

        public async Task<List<EnlaceMicroVentanaDto>> GetEnlacesMicroVentanasAsync() => _mapper.Map<List<EnlaceMicroVentanaDto>>(await GetFromApiAsync<EnlaceMicroVentanaApiModel>("enlacesMicroVentanas"));

        // ----------------------------------------------
        // SUSCRIPCIONES
        // ----------------------------------------------

        public async Task<List<SuscripcionDto>> GetSuscripcionesByTokenAsync(string token)
        {
            var results = await GetFromApiAsync<SuscripcionApiModel>("suscripciones", null, new Dictionary<string, string> { { "token", token } }, "suscripciones");
            return _mapper.Map<List<SuscripcionDto>>(results);
        }

        public async Task ActivarSuscripcionAsync(string id, string token) => await SendApiRequestAsync(HttpMethod.Post, "suscripciones/activar", new { id, token });
        public async Task ModificarSuscripcionAsync(SuscripcionApiModel suscripcion, string token) => await SendApiRequestAsync(HttpMethod.Put, "suscripciones/modificar", suscripcion, token);
        public async Task CrearSuscripcionAsync(SuscripcionApiModel suscripcion) => await SendApiRequestAsync(HttpMethod.Post, "suscripciones/nueva", suscripcion);
        public async Task EliminarSuscripcionAsync(int idSuscripcion, string token) => await SendApiRequestAsync(HttpMethod.Delete, "suscripciones/eliminar", new { idSuscripcion }, token);

        public async Task<string> CrearJWTAsync(string user, string password)
        {
            var response = await _httpClient.PostAsync($"{BaseUrl}suscripciones/login", new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("user", user),
                new KeyValuePair<string, string>("password", password)
            }));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public Task<List<ConvocatoriaDto>> GetConvocatoriasAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<ConcesionDto>> GetConcesionesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<BeneficiarioDto>> GetBeneficiariosAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<AccionDto>> GetAccionesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<AgrupacionDto>> GetAgrupacionesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<AreaDto>> GetAreasAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<EntidadDto>> GetEntidadesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<EstadoDto>> GetEstadosAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<FormaPagoDto>> GetFormasPagoAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<LineaDto>> GetLineasAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<MunicipioDto>> GetMunicipiosAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<OrganismoDto>> GetOrganismosAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<ProgramaDto>> GetProgramasAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<ProvinciaDto>> GetProvinciasAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<SectorDto>> GetSectoresAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<SituacionEntornoDto>> GetSituacionesEntornoAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<SubtipoSubvencionDto>> GetSubtiposSubvencionAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<TipoConvocatoriaDto>> GetTiposConvocatoriaAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<TipoOrganismoDto>> GetTiposOrganismoAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<TipoSubvencionDto>> GetTiposSubvencionAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<TramoDto>> GetTramosAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<UnidadAdministrativaDto>> GetUnidadesAdministrativasAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<DatosEstadisticosDto>> GetDatosEstadisticosAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<AyudaDto>> GetAyudasAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<AyudaEstadoDto>> GetAyudasEstadosAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<ConcesionDetalleDto>> GetConcesionesDetalleAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<ConvocatoriaDetalleDto>> GetConvocatoriasDetalleAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<GrandeBeneficiarioDto>> GetGrandesBeneficiariosAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<InstrumentoDto>> GetInstrumentosAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<MinimisDto>> GetMinimisAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<OrganosCodigoAdminDto>> GetOrganosCodigoAdminAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<PartidoPoliticoDto>> GetPartidosPoliticosAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<PlanEstrategicoDto>> GetPlanesEstrategicosAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<PlanEstrategicoDetalleDto>> GetPlanesEstrategicosDetalleAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<PlazoDto>> GetPlazosAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<SancionDto>> GetSancionesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<SancionDetalleDto>> GetSancionesDetalleAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<SectorProductoDto>> GetSectoresProductosAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<SuscripcionDto>> GetSuscripcionesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<TerceroDto>> GetTercerosAsync()
        {
            throw new NotImplementedException();
        }
    }
}
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Application.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SubvencionesApp.Infrastructure.Interfaces;
using SubvencionesApp.Api.Clients;
using System;
using System.Reflection;
using AutoMapper;
using SubvencionesApp.Infrastructure.ExternalServices.Models;

namespace SubvencionesApp.Api.Clients
{
    public class SubvencionesClient : ISubvencionesService, ISubvencionSyncService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExternalSubvencionesService _apiClient;
        private readonly IMapper _mapper;

        public SubvencionesClient(IUnitOfWork unitOfWork, IExternalSubvencionesService apiClient, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _apiClient = apiClient;
            _mapper = mapper;
        }

        public async Task SyncConvocatoriasAsync()
        {
            var apiData = await _apiClient.GetConvocatoriasAsync();
            var dbData = await _unitOfWork.Convocatorias.GetAllAsync();
            var dbKeys = dbData.Select(c => c.Id).ToHashSet();
            var newEntities = new List<Convocatoria>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<Convocatoria>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }

            if (newEntities.Any())
            {
                await _unitOfWork.Convocatorias.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncConcesionesAsync()
        {
            var apiData = await _apiClient.GetConcesionesAsync();
            var dbData = await _unitOfWork.Concesiones.GetAllAsync();
            var dbKeys = dbData.Select(c => c.IdConcesion).ToHashSet();
            var newEntities = new List<Concesion>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<Concesion>(dto);
                if (!dbKeys.Contains(entity.IdConcesion))
                {
                    newEntities.Add(entity);
                }
            }

            if (newEntities.Any())
            {
                await _unitOfWork.Concesiones.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncAllDataAsync()
        {
            var syncTasks = new List<Task>
            {
                SyncAccionesAsync(),
                SyncAgrupacionesAsync(),
                SyncAreasAsync(),
                SyncBeneficiariosAsync(),
                SyncConcesionesAsync(),
                SyncConvocatoriasAsync(),
                SyncEntidadesAsync(),
                SyncEstadosAsync(),
                SyncFormasPagoAsync(),
                SyncLineasAsync(),
                SyncMunicipiosAsync(),
                SyncOrganismosAsync(),
                SyncProgramasAsync(),
                SyncProvinciasAsync(),
                SyncSectoresAsync(),
                SyncSituacionesEntornoAsync(),
                SyncSubtiposSubvencionAsync(),
                SyncTiposBeneficiarioAsync(),
                SyncTiposConvocatoriaAsync(),
                SyncTiposOrganismoAsync(),
                SyncTiposSubvencionAsync(),
                SyncTramosAsync(),
                SyncUnidadesAdministrativasAsync(),
                SyncAyudasAsync(),
                SyncAyudasEstadosAsync(),
                SyncConcesionDetalleAsync(),
                SyncConvocatoriaDetalleAsync(),
                SyncFinalidadesAsync(),
                SyncGrandesBeneficiariosAsync(),
                SyncInstrumentosAsync(),
                SyncMinimisAsync(),
                SyncObjetivosAsync(),
                SyncOrganosCodigoAdminAsync(),
                SyncPartidosPoliticosAsync(),
                SyncPlanesEstrategicosAsync(),
                SyncPlanesEstrategicosDetalleAsync(),
                SyncPlazosAsync(),
                SyncRegionesAsync(),
                SyncReglamentosAsync(),
                SyncSancionesAsync(),
                SyncSancionesDetalleAsync(),
                SyncSectoresProductosAsync(),
                SyncSuscripcionesAsync(),
                SyncTercerosAsync()
            };
            await Task.WhenAll(syncTasks);
        }

        public async Task SyncBeneficiariosAsync()
        {
            var apiData = await _apiClient.GetBeneficiariosAsync();
            var dbData = await _unitOfWork.Beneficiarios.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<Beneficiario>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<Beneficiario>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.Beneficiarios.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncAccionesAsync()
        {
            var apiData = await _apiClient.GetAccionesAsync();
            var dbData = await _unitOfWork.Acciones.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<Accion>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<Accion>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.Acciones.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncAgrupacionesAsync()
        {
            var apiData = await _apiClient.GetAgrupacionesAsync();
            var dbData = await _unitOfWork.Agrupaciones.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<Agrupacion>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<Agrupacion>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.Agrupaciones.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncAreasAsync()
        {
            var apiData = await _apiClient.GetAreasAsync();
            var dbData = await _unitOfWork.Areas.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<Area>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<Area>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.Areas.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }
        
        public async Task SyncEntidadesAsync()
        {
            var apiData = await _apiClient.GetEntidadesAsync();
            var dbData = await _unitOfWork.Entidades.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<Entidad>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<Entidad>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.Entidades.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncEstadosAsync()
        {
            var apiData = await _apiClient.GetEstadosAsync();
            var dbData = await _unitOfWork.Estados.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<Estado>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<Estado>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.Estados.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncFormasPagoAsync()
        {
            var apiData = await _apiClient.GetFormasPagoAsync();
            var dbData = await _unitOfWork.FormasPago.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<FormaPago>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<FormaPago>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.FormasPago.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncLineasAsync()
        {
            var apiData = await _apiClient.GetLineasAsync();
            var dbData = await _unitOfWork.Lineas.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<Linea>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<Linea>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.Lineas.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncMunicipiosAsync()
        {
            var apiData = await _apiClient.GetMunicipiosAsync();
            var dbData = await _unitOfWork.Municipios.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<Municipio>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<Municipio>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.Municipios.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncOrganismosAsync()
        {
            var apiData = await _apiClient.GetOrganismosAsync();
            var dbData = await _unitOfWork.Organismos.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<Organismo>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<Organismo>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.Organismos.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncProgramasAsync()
        {
            var apiData = await _apiClient.GetProgramasAsync();
            var dbData = await _unitOfWork.Programas.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<Programa>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<Programa>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.Programas.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }
        
        public async Task SyncProvinciasAsync()
        {
            var apiData = await _apiClient.GetProvinciasAsync();
            var dbData = await _unitOfWork.Provincias.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<Provincia>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<Provincia>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.Provincias.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncSectoresAsync()
        {
            var apiData = await _apiClient.GetSectoresAsync();
            var dbData = await _unitOfWork.Sectores.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<Sector>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<Sector>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.Sectores.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncSituacionesEntornoAsync()
        {
            var apiData = await _apiClient.GetSituacionesEntornoAsync();
            var dbData = await _unitOfWork.SituacionesEntorno.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<SituacionEntorno>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<SituacionEntorno>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.SituacionesEntorno.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncSubtiposSubvencionAsync()
        {
            var apiData = await _apiClient.GetSubtiposSubvencionAsync();
            var dbData = await _unitOfWork.SubtiposSubvencion.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<SubtipoSubvencion>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<SubtipoSubvencion>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.SubtiposSubvencion.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncTiposBeneficiarioAsync()
        {
            var apiData = await _apiClient.GetTiposBeneficiarioAsync();
            var dbData = await _unitOfWork.TiposBeneficiario.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<TipoBeneficiario>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<TipoBeneficiario>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.TiposBeneficiario.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncTiposConvocatoriaAsync()
        {
            var apiData = await _apiClient.GetTiposConvocatoriaAsync();
            var dbData = await _unitOfWork.TiposConvocatoria.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<TipoConvocatoria>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<TipoConvocatoria>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.TiposConvocatoria.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncTiposOrganismoAsync()
        {
            var apiData = await _apiClient.GetTiposOrganismoAsync();
            var dbData = await _unitOfWork.TiposOrganismo.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<TipoOrganismo>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<TipoOrganismo>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.TiposOrganismo.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncTiposSubvencionAsync()
        {
            var apiData = await _apiClient.GetTiposSubvencionAsync();
            var dbData = await _unitOfWork.TiposSubvencion.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<TipoSubvencion>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<TipoSubvencion>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.TiposSubvencion.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncTramosAsync()
        {
            var apiData = await _apiClient.GetTramosAsync();
            var dbData = await _unitOfWork.Tramos.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<Tramo>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<Tramo>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.Tramos.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncUnidadesAdministrativasAsync()
        {
            var apiData = await _apiClient.GetUnidadesAdministrativasAsync();
            var dbData = await _unitOfWork.UnidadesAdministrativas.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<UnidadAdministrativa>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<UnidadAdministrativa>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.UnidadesAdministrativas.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncAyudasAsync()
        {
            var apiData = await _apiClient.GetAyudasAsync();
            var dbData = await _unitOfWork.Ayudas.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<Ayuda>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<Ayuda>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.Ayudas.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncAyudasEstadosAsync()
        {
            var apiData = await _apiClient.GetAyudasEstadosAsync();
            var dbData = await _unitOfWork.AyudasEstados.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<AyudaEstado>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<AyudaEstado>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.AyudasEstados.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncConcesionDetalleAsync()
        {
            var apiData = await _apiClient.GetConcesionesDetalleAsync();
            var dbData = await _unitOfWork.ConcesionesDetalle.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<ConcesionDetalle>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<ConcesionDetalle>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.ConcesionesDetalle.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncConvocatoriaDetalleAsync()
        {
            var apiData = await _apiClient.GetConvocatoriasDetalleAsync();
            var dbData = await _unitOfWork.ConvocatoriasDetalle.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<ConvocatoriaDetalle>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<ConvocatoriaDetalle>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.ConvocatoriasDetalle.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncFinalidadesAsync()
        {
            var apiData = await _apiClient.GetFinalidadesAsync();
            var dbData = await _unitOfWork.Finalidades.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<Finalidad>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<Finalidad>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.Finalidades.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncGrandesBeneficiariosAsync()
        {
            var apiData = await _apiClient.GetGrandesBeneficiariosAsync();
            var dbData = await _unitOfWork.GrandesBeneficiarios.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<GrandeBeneficiario>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<GrandeBeneficiario>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.GrandesBeneficiarios.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncInstrumentosAsync()
        {
            var apiData = await _apiClient.GetInstrumentosAsync();
            var dbData = await _unitOfWork.Instrumentos.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<Instrumento>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<Instrumento>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.Instrumentos.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncMinimisAsync()
        {
            var apiData = await _apiClient.GetMinimisAsync();
            var dbData = await _unitOfWork.Minimis.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<Minimis>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<Minimis>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.Minimis.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncObjetivosAsync()
        {
            var apiData = await _apiClient.GetObjetivosAsync();
            var dbData = await _unitOfWork.Objetivos.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<Objetivo>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<Objetivo>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.Objetivos.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncOrganosCodigoAdminAsync()
        {
            var apiData = await _apiClient.GetOrganosCodigoAdminAsync();
            var dbData = await _unitOfWork.OrganosCodigoAdmin.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<OrganosCodigoAdmin>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<OrganosCodigoAdmin>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.OrganosCodigoAdmin.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncPartidosPoliticosAsync()
        {
            var apiData = await _apiClient.GetPartidosPoliticosAsync();
            var dbData = await _unitOfWork.PartidosPoliticos.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<PartidoPolitico>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<PartidoPolitico>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.PartidosPoliticos.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncPlanesEstrategicosAsync()
        {
            var apiData = await _apiClient.GetPlanesEstrategicosAsync();
            var dbData = await _unitOfWork.PlanesEstrategicos.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<PlanEstrategico>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<PlanEstrategico>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.PlanesEstrategicos.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncPlanesEstrategicosDetalleAsync()
        {
            var apiData = await _apiClient.GetPlanesEstrategicosDetalleAsync();
            var dbData = await _unitOfWork.PlanesEstrategicosDetalle.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<PlanEstrategicoDetalle>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<PlanEstrategicoDetalle>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.PlanesEstrategicosDetalle.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncPlazosAsync()
        {
            var apiData = await _apiClient.GetPlazosAsync();
            var dbData = await _unitOfWork.Plazos.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<Plazo>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<Plazo>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.Plazos.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncRegionesAsync()
        {
            var apiData = await _apiClient.GetRegionesAsync();
            var dbData = await _unitOfWork.Regiones.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<Region>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<Region>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.Regiones.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncReglamentosAsync()
        {
            var apiData = await _apiClient.GetReglamentosAsync();
            var dbData = await _unitOfWork.Reglamentos.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<Reglamento>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<Reglamento>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.Reglamentos.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncSancionesAsync()
        {
            var apiData = await _apiClient.GetSancionesAsync();
            var dbData = await _unitOfWork.Sanciones.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<Sancion>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<Sancion>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.Sanciones.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncSancionesDetalleAsync()
        {
            var apiData = await _apiClient.GetSancionesDetalleAsync();
            var dbData = await _unitOfWork.SancionesDetalle.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<SancionDetalle>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<SancionDetalle>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.SancionesDetalle.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncSectoresProductosAsync()
        {
            var apiData = await _apiClient.GetSectoresProductosAsync();
            var dbData = await _unitOfWork.SectoresProductos.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<SectorProducto>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<SectorProducto>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.SectoresProductos.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncSuscripcionesAsync()
        {
            var apiData = await _apiClient.GetSuscripcionesAsync();
            var dbData = await _unitOfWork.Suscripciones.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<Suscripcion>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<Suscripcion>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.Suscripciones.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SyncTercerosAsync()
        {
            var apiData = await _apiClient.GetTercerosAsync();
            var dbData = await _unitOfWork.Terceros.GetAllAsync();
            var dbKeys = dbData.Select(e => e.Id).ToHashSet();
            var newEntities = new List<Tercero>();

            foreach (var dto in apiData)
            {
                var entity = _mapper.Map<Tercero>(dto);
                if (!dbKeys.Contains(entity.Id))
                {
                    newEntities.Add(entity);
                }
            }
            if (newEntities.Any())
            {
                await _unitOfWork.Terceros.AddRangeAsync(newEntities);
                await _unitOfWork.CommitAsync();
            }
        }
        
        public async Task<IEnumerable<AccionDto>> GetAccionesAsync()
        {
            var acciones = await _unitOfWork.Acciones.GetAllAsync();
            return _mapper.Map<IEnumerable<AccionDto>>(acciones);
        }
        public async Task<IEnumerable<AgrupacionDto>> GetAgrupacionesAsync()
        {
            var agrupaciones = await _unitOfWork.Agrupaciones.GetAllAsync();
            return _mapper.Map<IEnumerable<AgrupacionDto>>(agrupaciones);
        }
        public async Task<IEnumerable<AreaDto>> GetAreasAsync()
        {
            var areas = await _unitOfWork.Areas.GetAllAsync();
            return _mapper.Map<IEnumerable<AreaDto>>(areas);
        }
        public async Task<IEnumerable<BeneficiarioDto>> GetBeneficiariosAsync()
        {
            var beneficiarios = await _unitOfWork.Beneficiarios.GetAllAsync();
            return _mapper.Map<IEnumerable<BeneficiarioDto>>(beneficiarios);
        }
        public async Task<IEnumerable<ConcesionDto>> GetConcesionesAsync()
        {
            var concesiones = await _unitOfWork.Concesiones.GetAllAsync();
            return _mapper.Map<IEnumerable<ConcesionDto>>(concesiones);
        }
        public async Task<IEnumerable<ConvocatoriaDto>> GetConvocatoriasAsync()
        {
            var convocatorias = await _unitOfWork.Convocatorias.GetAllAsync();
            return _mapper.Map<IEnumerable<ConvocatoriaDto>>(convocatorias);
        }
        public async Task<IEnumerable<DatosEstadisticosDto>> GetDatosEstadisticosAsync()
        {
            var datosEstadisticos = await _apiClient.GetDatosEstadisticosAsync();
            return datosEstadisticos;
        }
        public async Task<IEnumerable<EntidadDto>> GetEntidadesAsync()
        {
            var entidades = await _unitOfWork.Entidades.GetAllAsync();
            return _mapper.Map<IEnumerable<EntidadDto>>(entidades);
        }
        public async Task<IEnumerable<EstadoDto>> GetEstadosAsync()
        {
            var estados = await _unitOfWork.Estados.GetAllAsync();
            return _mapper.Map<IEnumerable<EstadoDto>>(estados);
        }
        public async Task<IEnumerable<FormaPagoDto>> GetFormasPagoAsync()
        {
            var formasPago = await _unitOfWork.FormasPago.GetAllAsync();
            return _mapper.Map<IEnumerable<FormaPagoDto>>(formasPago);
        }
        public async Task<IEnumerable<LineaDto>> GetLineasAsync()
        {
            var lineas = await _unitOfWork.Lineas.GetAllAsync();
            return _mapper.Map<IEnumerable<LineaDto>>(lineas);
        }
        public async Task<IEnumerable<MunicipioDto>> GetMunicipiosAsync()
        {
            var municipios = await _unitOfWork.Municipios.GetAllAsync();
            return _mapper.Map<IEnumerable<MunicipioDto>>(municipios);
        }
        public async Task<IEnumerable<OrganismoDto>> GetOrganismosAsync()
        {
            var organismos = await _unitOfWork.Organismos.GetAllAsync();
            return _mapper.Map<IEnumerable<OrganismoDto>>(organismos);
        }
        public async Task<IEnumerable<ProgramaDto>> GetProgramasAsync()
        {
            var programas = await _unitOfWork.Programas.GetAllAsync();
            return _mapper.Map<IEnumerable<ProgramaDto>>(programas);
        }
        public async Task<IEnumerable<ProvinciaDto>> GetProvinciasAsync()
        {
            var provincias = await _unitOfWork.Provincias.GetAllAsync();
            return _mapper.Map<IEnumerable<ProvinciaDto>>(provincias);
        }
        public async Task<IEnumerable<SectorDto>> GetSectoresAsync()
        {
            var sectores = await _unitOfWork.Sectores.GetAllAsync();
            return _mapper.Map<IEnumerable<SectorDto>>(sectores);
        }
        public async Task<IEnumerable<SituacionEntornoDto>> GetSituacionesEntornoAsync()
        {
            var situaciones = await _unitOfWork.SituacionesEntorno.GetAllAsync();
            return _mapper.Map<IEnumerable<SituacionEntornoDto>>(situaciones);
        }
        public async Task<IEnumerable<SubtipoSubvencionDto>> GetSubtiposSubvencionAsync()
        {
            var subtipos = await _unitOfWork.SubtiposSubvencion.GetAllAsync();
            return _mapper.Map<IEnumerable<SubtipoSubvencionDto>>(subtipos);
        }
        public async Task<IEnumerable<TipoBeneficiarioDto>> GetTiposBeneficiarioAsync()
        {
            var tipos = await _unitOfWork.TiposBeneficiario.GetAllAsync();
            return _mapper.Map<IEnumerable<TipoBeneficiarioDto>>(tipos);
        }
        public async Task<IEnumerable<TipoConvocatoriaDto>> GetTiposConvocatoriaAsync()
        {
            var tipos = await _unitOfWork.TiposConvocatoria.GetAllAsync();
            return _mapper.Map<IEnumerable<TipoConvocatoriaDto>>(tipos);
        }
        public async Task<IEnumerable<TipoOrganismoDto>> GetTiposOrganismoAsync()
        {
            var tipos = await _unitOfWork.TiposOrganismo.GetAllAsync();
            return _mapper.Map<IEnumerable<TipoOrganismoDto>>(tipos);
        }
        public async Task<IEnumerable<TipoSubvencionDto>> GetTiposSubvencionAsync()
        {
            var tipos = await _unitOfWork.TiposSubvencion.GetAllAsync();
            return _mapper.Map<IEnumerable<TipoSubvencionDto>>(tipos);
        }
        public async Task<IEnumerable<TramoDto>> GetTramosAsync()
        {
            var tramos = await _unitOfWork.Tramos.GetAllAsync();
            return _mapper.Map<IEnumerable<TramoDto>>(tramos);
        }
        public async Task<IEnumerable<UnidadAdministrativaDto>> GetUnidadesAdministrativasAsync()
        {
            var unidades = await _unitOfWork.UnidadesAdministrativas.GetAllAsync();
            return _mapper.Map<IEnumerable<UnidadAdministrativaDto>>(unidades);
        }
        public async Task<IEnumerable<AyudaDto>> GetAyudasAsync()
        {
            var ayudas = await _unitOfWork.Ayudas.GetAllAsync();
            return _mapper.Map<IEnumerable<AyudaDto>>(ayudas);
        }
        public async Task<IEnumerable<AyudaEstadoDto>> GetAyudasEstadosAsync()
        {
            var ayudaEstados = await _unitOfWork.AyudasEstados.GetAllAsync();
            return _mapper.Map<IEnumerable<AyudaEstadoDto>>(ayudaEstados);
        }
        public async Task<IEnumerable<ConcesionDetalleDto>> GetConcesionesDetalleAsync()
        {
            var concesionesDetalle = await _unitOfWork.ConcesionesDetalle.GetAllAsync();
            return _mapper.Map<IEnumerable<ConcesionDetalleDto>>(concesionesDetalle);
        }
        public async Task<IEnumerable<ConvocatoriaDetalleDto>> GetConvocatoriasDetalleAsync()
        {
            var convocatoriasDetalle = await _unitOfWork.ConvocatoriasDetalle.GetAllAsync();
            return _mapper.Map<IEnumerable<ConvocatoriaDetalleDto>>(convocatoriasDetalle);
        }
        public async Task<IEnumerable<FinalidadDto>> GetFinalidadesAsync()
        {
            var finalidades = await _unitOfWork.Finalidades.GetAllAsync();
            return _mapper.Map<IEnumerable<FinalidadDto>>(finalidades);
        }
        public async Task<IEnumerable<GrandeBeneficiarioDto>> GetGrandesBeneficiariosAsync()
        {
            var grandes = await _unitOfWork.GrandesBeneficiarios.GetAllAsync();
            return _mapper.Map<IEnumerable<GrandeBeneficiarioDto>>(grandes);
        }
        public async Task<IEnumerable<InstrumentoDto>> GetInstrumentosAsync()
        {
            var instrumentos = await _unitOfWork.Instrumentos.GetAllAsync();
            return _mapper.Map<IEnumerable<InstrumentoDto>>(instrumentos);
        }
        public async Task<IEnumerable<MinimisDto>> GetMinimisAsync()
        {
            var minimis = await _unitOfWork.Minimis.GetAllAsync();
            return _mapper.Map<IEnumerable<MinimisDto>>(minimis);
        }
        public async Task<IEnumerable<ObjetivoDto>> GetObjetivosAsync()
        {
            var objetivos = await _unitOfWork.Objetivos.GetAllAsync();
            return _mapper.Map<IEnumerable<ObjetivoDto>>(objetivos);
        }
        public async Task<IEnumerable<OrganosCodigoAdminDto>> GetOrganosCodigoAdminAsync()
        {
            var organos = await _unitOfWork.OrganosCodigoAdmin.GetAllAsync();
            return _mapper.Map<IEnumerable<OrganosCodigoAdminDto>>(organos);
        }
        public async Task<IEnumerable<PartidoPoliticoDto>> GetPartidosPoliticosAsync()
        {
            var partidos = await _unitOfWork.PartidosPoliticos.GetAllAsync();
            return _mapper.Map<IEnumerable<PartidoPoliticoDto>>(partidos);
        }
        public async Task<IEnumerable<PlanEstrategicoDto>> GetPlanesEstrategicosAsync()
        {
            var planes = await _unitOfWork.PlanesEstrategicos.GetAllAsync();
            return _mapper.Map<IEnumerable<PlanEstrategicoDto>>(planes);
        }
        public async Task<IEnumerable<PlanEstrategicoDetalleDto>> GetPlanesEstrategicosDetalleAsync()
        {
            var planesDetalle = await _unitOfWork.PlanesEstrategicosDetalle.GetAllAsync();
            return _mapper.Map<IEnumerable<PlanEstrategicoDetalleDto>>(planesDetalle);
        }
        public async Task<IEnumerable<PlazoDto>> GetPlazosAsync()
        {
            var plazos = await _unitOfWork.Plazos.GetAllAsync();
            return _mapper.Map<IEnumerable<PlazoDto>>(plazos);
        }
        public async Task<IEnumerable<RegionDto>> GetRegionesAsync()
        {
            var regiones = await _unitOfWork.Regiones.GetAllAsync();
            return _mapper.Map<IEnumerable<RegionDto>>(regiones);
        }
        public async Task<IEnumerable<ReglamentoDto>> GetReglamentosAsync()
        {
            var reglamentos = await _unitOfWork.Reglamentos.GetAllAsync();
            return _mapper.Map<IEnumerable<ReglamentoDto>>(reglamentos);
        }
        public async Task<IEnumerable<SancionDto>> GetSancionesAsync()
        {
            var sanciones = await _unitOfWork.Sanciones.GetAllAsync();
            return _mapper.Map<IEnumerable<SancionDto>>(sanciones);
        }
        public async Task<IEnumerable<SancionDetalleDto>> GetSancionesDetalleAsync()
        {
            var sancionesDetalle = await _unitOfWork.SancionesDetalle.GetAllAsync();
            return _mapper.Map<IEnumerable<SancionDetalleDto>>(sancionesDetalle);
        }
        public async Task<IEnumerable<SectorProductoDto>> GetSectoresProductosAsync()
        {
            var sectores = await _unitOfWork.SectoresProductos.GetAllAsync();
            return _mapper.Map<IEnumerable<SectorProductoDto>>(sectores);
        }
        public async Task<IEnumerable<SuscripcionDto>> GetSuscripcionesAsync()
        {
            var suscripciones = await _unitOfWork.Suscripciones.GetAllAsync();
            return _mapper.Map<IEnumerable<SuscripcionDto>>(suscripciones);
        }
        public async Task<IEnumerable<TerceroDto>> GetTercerosAsync()
        {
            var terceros = await _unitOfWork.Terceros.GetAllAsync();
            return _mapper.Map<IEnumerable<TerceroDto>>(terceros);
        }
    }
}
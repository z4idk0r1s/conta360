// System
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

// ASP.NET Core
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// Entity Framework Core / ORM
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Dapper;

// Validación
using FluentValidation;
using FluentValidation.Results;

// Utilidades / 3rd Party
using AutoMapper;
using MediatR;
using Serilog;

// Swagger
using Swashbuckle.AspNetCore.Annotations;

// Conta360.Application
using Conta360.Application;
using Conta360.Application.DTOs;
using Conta360.Application.Interfaces;
using Conta360.Application.UseCases;

// Conta360.Domain
using Conta360.Domain;
using Conta360.Domain.Entities;
using Conta360.Domain.ValueObjects;
using Conta360.Domain.Rules;
using Conta360.Domain.Rules.EmittedInvoice;
using Conta360.Domain.Mapping;

// Conta360.Infrastructure
using Conta360.Infrastructure;
using Conta360.Infrastructure.Adapters;
using Conta360.Infrastructure.Adapters.ExcelProcessor;
using Conta360.Infrastructure.Adapters.PGCExtractor;
using Conta360.Infrastructure.Adapters.PGCExtractor.Core;
using Conta360.Infrastructure.Adapters.PGCExtractor.Core.Models;
using Conta360.Infrastructure.Adapters.PGCExtractor.Data;
using Conta360.Infrastructure.Adapters.PGCExtractor.Data.Services;
using Conta360.Infrastructure.Adapters.PGCExtractor.Logic;
using Conta360.Infrastructure.Adapters.PGCExtractor.Logic.Services;
using Conta360.Infrastructure.Adapters.PGCExtractor.Tracker;
using Conta360.Infrastructure.Adapters.PGCExtractor.Tracker.Services;
using Conta360.Infrastructure.Repositories;
using Conta360.Infrastructure.Validation;
using Conta360.Infrastructure.Validation.Models;

// Conta360.Persistence
using Conta360.Persistence;
using Conta360.Persistence.postgres;
using Conta360.Persistence.sqlite;

// Conta360.SDK
using Conta360.SDK;

// Conta360.Services.Api
using Conta360.Services.Api;
using Conta360.Services.Api.Controllers;

// Conta360.Shared.Models
using Conta360.Shared.Models;
using Conta360.Shared.Models.DTOs;
using Conta360.Shared.Models.Enums;


namespace Conta360.Infrastructure.Adapters.ExcelProcessor
{
    public class ExcelProcessor : IExcelProcessor
    {
        private readonly IMapper _mapper;

        public ExcelProcessor(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<(IEnumerable<EmittedInvoiceDto>, IEnumerable<ReceivedInvoiceDto>)> ProcessExcelFile(Stream fileStream)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var reader = ExcelReaderFactory.CreateReader(fileStream))
            {
                var emittedInvoices = new List<EmittedInvoiceDto>();
                var receivedInvoices = new List<ReceivedInvoiceDto>();

                do
                {
                    if (reader.Name.Contains("Emitidas", StringComparison.OrdinalIgnoreCase))
                    {
                        emittedInvoices.AddRange(await ProcessEmittedInvoicesSheetAsync(reader));
                    }
                    else if (reader.Name.Contains("Recibidas", StringComparison.OrdinalIgnoreCase))
                    {
                        receivedInvoices.AddRange(await ProcessReceivedInvoicesSheetAsync(reader));
                    }
                } while (reader.NextResult());

                return (emittedInvoices, receivedInvoices);
            }
        }

        private async Task<IEnumerable<EmittedInvoiceDto>> ProcessEmittedInvoicesSheetAsync(IExcelDataReader reader)
        {
            return await Task.Run(() =>
            {
                var invoices = new List<EmittedInvoiceDto>();
                bool isHeaderRow = true;

                while (reader.Read())
                {
                    if (isHeaderRow)
                    {
                        isHeaderRow = false;
                        continue;
                    }

                    var invoice = new EmittedInvoiceDto
                    {
                        InvoiceNumber = reader.GetString(0),
                        IssueDate = reader.GetDateTime(1),
                        Amount = (decimal)reader.GetDouble(2),
                        // Agrega el resto de los mapeos reales aquí según columnas
                    };

                    invoices.Add(invoice);
                }

                return invoices;
            });
        }

        private async Task<IEnumerable<ReceivedInvoiceDto>> ProcessReceivedInvoicesSheetAsync(IExcelDataReader reader)
        {
            return await Task.Run(() =>
            {
                var invoices = new List<ReceivedInvoiceDto>();
                bool isHeaderRow = true;

                while (reader.Read())
                {
                    if (isHeaderRow)
                    {
                        isHeaderRow = false;
                        continue;
                    }

                    var invoice = new ReceivedInvoiceDto
                    {
                        InvoiceNumber = reader.GetString(0),
                        Date = reader.GetDateTime(1).ToString("yyyy-MM-dd"),
                        Amount = (decimal)reader.GetDouble(2),
                        // Agrega el resto de los mapeos reales aquí según columnas
                    };

                    invoices.Add(invoice);
                }

                return invoices;
            });
        }
    }
}

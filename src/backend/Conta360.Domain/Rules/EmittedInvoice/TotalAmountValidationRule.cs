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



namespace Conta360.Domain.Rules.EmittedInvoice
{
    public class TotalAmountValidationRule : IValidationRule<EmittedInvoiceDto>
    {
        public string RuleName => "TotalAmountValidation";
        public string ErrorMessage => "El importe total de la factura no coincide con la suma de las líneas";

        public async Task<ValidationResult> ValidateAsync(EmittedInvoiceDto invoice)
        {
            return await Task.Run(() =>
            {
                var result = new ValidationResult
                {
                    Source = "EmittedInvoice",
                    EntityId = invoice.Id
                };

                var calculatedTotal = 0m;
                foreach (var line in invoice.Lines)
                {
                    calculatedTotal += line.Amount * (1 + line.TaxRate / 100);
                }

                if (calculatedTotal != invoice.TotalAmount)
                {
                    result.AddError(
                        "INV001",
                        $"El importe total ({invoice.TotalAmount}) no coincide con la suma de las líneas ({calculatedTotal})"
                    );
                }

                return result;
            });
        }
    }
}

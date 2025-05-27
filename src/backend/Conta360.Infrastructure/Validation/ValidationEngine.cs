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



namespace Conta360.Infrastructure.Validation
{
    public class ValidationEngine : IValidationEngine
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ValidationEngine> _logger;

        public ValidationEngine(IServiceProvider serviceProvider, ILogger<ValidationEngine> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task<ValidationResult> ValidateEmittedInvoiceAsync(EmittedInvoiceDto invoice)
        {
            var rules = _serviceProvider.GetServices<IValidationRule<EmittedInvoiceDto>>();
            var result = new ValidationResult { Source = "EmittedInvoice", EntityId = invoice.Id };

            foreach (var rule in rules)
            {
                try
                {
                    var ruleResult = await rule.ValidateAsync(invoice);
                    if (!ruleResult.IsValid)
                    {
                        result.Errors.AddRange(ruleResult.Errors);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error validating rule {RuleName} for invoice {InvoiceId}", rule.RuleName, invoice.Id);
                    result.AddError("VAL001", $"Error interno en la validación: {rule.RuleName}");
                }
            }

            return result;
        }

        public async Task<ValidationResult> ValidateReceivedInvoiceAsync(ReceivedInvoiceDto invoice)
        {
            var rules = _serviceProvider.GetServices<IValidationRule<ReceivedInvoiceDto>>();
            var result = new ValidationResult { Source = "ReceivedInvoice", EntityId = invoice.Id };

            foreach (var rule in rules)
            {
                try
                {
                    var ruleResult = await rule.ValidateAsync(invoice);
                    if (!ruleResult.IsValid)
                    {
                        result.Errors.AddRange(ruleResult.Errors);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error validating rule {RuleName} for invoice {InvoiceId}", rule.RuleName, invoice.Id);
                    result.AddError("VAL001", $"Error interno en la validación: {rule.RuleName}");
                }
            }

            return result;
        }

        public async Task<ValidationResult> ValidateAccountingEntryAsync(AccountingEntryDto entry)
        {
            var rules = _serviceProvider.GetServices<IValidationRule<AccountingEntryDto>>();
            var result = new ValidationResult { Source = "AccountingEntry", EntityId = entry.Id };

            foreach (var rule in rules)
            {
                try
                {
                    var ruleResult = await rule.ValidateAsync(entry);
                    if (!ruleResult.IsValid)
                    {
                        result.Errors.AddRange(ruleResult.Errors);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error validating rule {RuleName} for entry {EntryId}", rule.RuleName, entry.Id);
                    result.AddError("VAL001", $"Error interno en la validación: {rule.RuleName}");
                }
            }

            return result;
        }

        public async Task<IEnumerable<ValidationResult>> ValidateAllAsync(ValidationContext context)
        {
            var results = new List<ValidationResult>();

            if (context.EmittedInvoices != null)
            {
                foreach (var invoice in context.EmittedInvoices)
                {
                    results.Add(await ValidateEmittedInvoiceAsync(invoice));
                }
            }

            if (context.ReceivedInvoices != null)
            {
                foreach (var invoice in context.ReceivedInvoices)
                {
                    results.Add(await ValidateReceivedInvoiceAsync(invoice));
                }
            }

            if (context.AccountingEntries != null)
            {
                foreach (var entry in context.AccountingEntries)
                {
                    results.Add(await ValidateAccountingEntryAsync(entry));
                }
            }

            return results;
        }
    }
}

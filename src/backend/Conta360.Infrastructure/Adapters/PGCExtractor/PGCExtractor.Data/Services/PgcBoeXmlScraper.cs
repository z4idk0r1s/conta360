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


namespace Conta360.Infrastructure.Adapters.PGCExtractor.PGCExtractor.Data.Services
{
    public class PgcBoeXmlScraper
    {
        public List<AccountNode> ExtractFromXsd(string xsdPath)
        {
            var xdoc = XDocument.Load(xsdPath);

            var accounts = xdoc.Descendants()
                .Where(e => e.Name.LocalName == "element")
                .Select(ParseAccountElement)
                .Where(n => n != null)
                .ToList()!;

            return accounts;
        }

        private AccountNode? ParseAccountElement(XElement element)
        {
            var nameAttr = element.Attribute("name")?.Value;
            if (string.IsNullOrWhiteSpace(nameAttr) || !nameAttr.Contains('.'))
                return null;

            var parts = nameAttr.Split('.', 2);
            if (parts.Length != 2)
                return null;

            var code = parts[0];
            if (!int.TryParse(code, out _))  // Asegura que sea código numérico
                return null;

            var name = parts[1].Trim();
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return new AccountNode
            {
                Code = code,
                Name = name,
                Type = AccountUtils.GetTypeFromCode(code)
            };
        }

        [Obsolete("Este método está obsoleto. La extracción desde texto plano del BOE ya no es soportada. Utiliza ExtractFromXsd.")]
        public List<AccountNode> ExtractFromXml(string xmlPath)
        {
            throw new NotSupportedException("Este método ya no está soportado. Usa ExtractFromXsd en su lugar.");
        }
    }
}

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


namespace Conta360.Infrastructure.Adapters.PGCExtractor.PGCExtractor.Tracker.Services

public class PgcChangeTracker
{
    private const string HashPath = "Data/last_hash.txt";
    private const string SnapshotPath = "Data/last_accounts.json";
    private const string DiffPath = "Data/diff_log.json";
    private readonly PgcBoeXmlScraper _scraper;
    private readonly AccountClassifier _classifier;

    public PgcChangeTracker(PgcBoeXmlScraper scraper, AccountClassifier classifier)
    {
        _scraper = scraper;
        _classifier = classifier;
    }

    public bool CheckForChanges(string xsdPath)
    {
        if (!File.Exists(xsdPath)) return false;
        var xsdContent = File.ReadAllText(xsdPath);
        var currentHash = ComputeHash(xsdContent);
        var lastHash = File.Exists(HashPath) ? File.ReadAllText(HashPath) : null;
        if (lastHash == currentHash) return false;
        File.WriteAllText(HashPath, currentHash);

        var newAccounts = _scraper.ExtractFromXsd(xsdPath);
        _classifier.ClassifyAccounts(newAccounts);

        List<AccountNode>? oldAccounts = null;
        if (File.Exists(SnapshotPath))
            oldAccounts = JsonSerializer.Deserialize<List<AccountNode>>(File.ReadAllText(SnapshotPath));

        File.WriteAllText(SnapshotPath, JsonSerializer.Serialize(newAccounts, new JsonSerializerOptions { WriteIndented = true }));

        if (oldAccounts != null)
        {
            var diffs = CalculateDiff(oldAccounts, newAccounts);
            File.AppendAllText(DiffPath, JsonSerializer.Serialize(new { Timestamp = DateTime.UtcNow, Diffs = diffs }, new JsonSerializerOptions { WriteIndented = true }) + " ");
        }

        return true;
    }


    private string ComputeHash(string content)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(content);
        return Convert.ToHexString(sha.ComputeHash(bytes));
    }

    private object CalculateDiff(List<AccountNode> oldList, List<AccountNode> newList)
    {
        var added = newList.Where(n => !oldList.Any(o => o.Code == n.Code)).ToList();
        var removed = oldList.Where(o => !newList.Any(n => n.Code == o.Code)).ToList();
        var changed = newList.Where(n => oldList.Any(o => o.Code == n.Code && (o.Name != n.Name || o.Type != n.Type))).ToList();
        return new { Added = added, Removed = removed, Modified = changed };
    }
}

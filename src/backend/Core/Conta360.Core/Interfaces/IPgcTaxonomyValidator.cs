using Conta360.Core.Common;

namespace Conta360.Core.Interfaces;

public interface IPgcTaxonomyValidator
{
    Task<ValidationResult> ValidateXmlAsync(string xmlContent, PgcModelType modelType);
}

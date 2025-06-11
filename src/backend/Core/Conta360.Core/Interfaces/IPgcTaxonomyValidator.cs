namespace Conta360.Core.Interfaces;

public interface IPgcTaxonomyValidator
{
    Task<ValidationResult> ValidateXmlAsync(string xmlContent, PgcModelType modelType);
}

using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace Software.IP.Trace.Views.Find;

public class LinkValidationRule : ValidationRule {
    private readonly LinkValidator _validator = new();

    public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
        var bindingExpression = value as BindingExpression;
        var source = bindingExpression?.ResolvedSource;
        var propertyName = bindingExpression?.ResolvedSourcePropertyName;
        Debug.Assert(propertyName != null, nameof(propertyName) + " != null");
        var propertyValue = source?.GetType().GetProperty(propertyName)?.GetValue(source) as string;
        propertyValue = propertyValue?.Trim() ?? string.Empty;
        var result = _validator.Validate(propertyValue);
        if (result.IsValid)
            return ValidationResult.ValidResult;

        var errorContent = result.Errors.First().ErrorMessage;
        return new ValidationResult(false, errorContent);
    }
}

public class ApiKeyValidationRule : ValidationRule {
    private readonly ApiKeyValidator _validator = new();

    public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
        var bindingExpression = value as BindingExpression;
        var source = bindingExpression?.ResolvedSource;
        var propertyName = bindingExpression?.ResolvedSourcePropertyName;
        Debug.Assert(propertyName != null, nameof(propertyName) + " != null");
        var propertyValue = source?.GetType().GetProperty(propertyName)?.GetValue(source) as string;
        propertyValue = propertyValue?.Trim() ?? string.Empty;
        var result = _validator.Validate(propertyValue);
        if (result.IsValid)
            return ValidationResult.ValidResult;

        var errorContent = result.Errors.First().ErrorMessage;
        return new ValidationResult(false, errorContent);
    }
}

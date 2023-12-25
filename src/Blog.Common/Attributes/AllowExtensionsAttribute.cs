using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Blog.Common.Attributes;

public class AllowExtensionsAttribute : BaseValidationAttribute, IClientModelValidator
{
    private readonly string[] _allowExtensions;
    private readonly string[] _allowContentTypes;
    private readonly string _errorMessage;
    public AllowExtensionsAttribute(string displayName, string[] allowExtensions, string[] allowContentTypes)
    {
        _allowExtensions = allowExtensions;
        _allowContentTypes = allowContentTypes;
        _errorMessage = $"فرمت های مجاز برای {displayName} : ";

        foreach (var allowExtension in allowExtensions)
        {
            _errorMessage += $"{allowExtension}, ";
        }

        _errorMessage = _errorMessage.Trim(' ').Trim(',');
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var file = value as IFormFile;
        if (file is not null && file.Length > 0)
        {
            if (!_allowContentTypes.Contains(file.ContentType))
            {
                return new ValidationResult(_errorMessage);
            }
        }
        return ValidationResult.Success;
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        MergeAttribute(context.Attributes, "data-val", "true");
        MergeAttribute(context.Attributes, "data-val-allowExtensions", _errorMessage);
        MergeAttribute(context.Attributes, "data-val-whitelistextensions", string.Join(',', _allowContentTypes));
    }
}

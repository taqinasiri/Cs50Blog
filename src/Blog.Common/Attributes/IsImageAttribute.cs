using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Blog.Common.Attributes;

public class IsImageAttribute : BaseValidationAttribute, IClientModelValidator
{
    private readonly string[] _allowExtensions =
    {
        "image/png",
        "image/jpeg"
    };
    private readonly string _errorMessage;
    public IsImageAttribute(string displayName)
    {
        _errorMessage = $"{displayName} حتما باید یک عکس با فرمت png یا jpg باشد";
    }

    //Server validation
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var file = value as IFormFile;
        if (file is not null && file.Length > 0)
        {
            if (!_allowExtensions.Contains(file.ContentType))
            {
                return new ValidationResult(_errorMessage);
            }
            try
            {
                Image.FromStream(file.OpenReadStream());
            }
            catch
            {
                return new ValidationResult(_errorMessage);
            }
        }
        return ValidationResult.Success;
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        MergeAttribute(context.Attributes, "data-val", "true");
        MergeAttribute(context.Attributes, "data-val-isImage", _errorMessage);
        MergeAttribute(context.Attributes, "data-val-whitelistextensions", string.Join(',', _allowExtensions));
    }
}

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Blog.Common.Attributes;

public class BoolValidationAttribute : BaseValidationAttribute, IClientModelValidator
{
    private readonly bool _validStatus;
    public BoolValidationAttribute(bool validStatus = true)
    {
        _validStatus = validStatus;
    }
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var stauts = (bool)value;
        if (stauts != _validStatus) return new ValidationResult(ErrorMessage);
        return ValidationResult.Success;
    }
    public void AddValidation(ClientModelValidationContext context)
    {
        MergeAttribute(context.Attributes, "data-val", "true");
        MergeAttribute(context.Attributes, "data-val-validstatus", _validStatus.ToString().ToLower());
        MergeAttribute(context.Attributes, "data-val-boolValidation", ErrorMessage);
    }
}

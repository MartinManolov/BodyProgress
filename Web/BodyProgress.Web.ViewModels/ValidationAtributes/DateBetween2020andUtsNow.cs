namespace BodyProgress.Web.ViewModels.ValidationAtributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class DateBetween2020andUtsNow : ValidationAttribute
    {
       protected override ValidationResult IsValid(object value, ValidationContext validationContext)
       {
           var date = (DateTime)value;
           if (date.Year >= 2020 && date.Date <= DateTime.UtcNow)
           {
                return ValidationResult.Success;
           }

           return new ValidationResult($"Date should be between 01.01.2020 and now");
        }
    }
}

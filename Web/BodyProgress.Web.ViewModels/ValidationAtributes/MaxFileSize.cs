namespace BodyProgress.Web.ViewModels.ValidationAtributes
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    public class MaxFileSize : ValidationAttribute
    {
        private readonly int maxFileSize;

        public MaxFileSize(int maxFileSize)
        {
            this.maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file.Length > this.maxFileSize)
            {
                return new ValidationResult(this.GetErrorMessage());
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"Maximum allowed file size is {this.maxFileSize} bytes.";
        }
    }
}

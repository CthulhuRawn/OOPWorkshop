using System;
using System.ComponentModel.DataAnnotations;

namespace DTO.Validators
{
    public class NotInPastAttribute : ValidationAttribute
    {
        public bool AllowNulls { get; set; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (AllowNulls && value == null)
            {
                return ValidationResult.Success;
            }

            if (!(value is DateTime))
            {
                throw new ValidationException("Value must be DateTime");
            }

            var dt = (DateTime)value;
            if (dt < DateTime.Now)
            {
                return new ValidationResult("Make sure your date is newer than today");
            }
            return ValidationResult.Success;
        }
    }
}
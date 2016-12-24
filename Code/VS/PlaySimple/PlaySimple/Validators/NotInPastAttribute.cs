using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PlaySimple.Validators
{
    public class NotInPastAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(value is DateTime))
            {
                throw new ValidationException("Value must be DateTime");
            }

            DateTime dt = (DateTime)value;
            if (dt < DateTime.Now)
            {
                return new ValidationResult("Make sure your date is newer than today");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
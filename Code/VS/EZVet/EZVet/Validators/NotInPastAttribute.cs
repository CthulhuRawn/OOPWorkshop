﻿using System;
using System.ComponentModel.DataAnnotations;

namespace EZVet.Validators
{
    public class NotInPastAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
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
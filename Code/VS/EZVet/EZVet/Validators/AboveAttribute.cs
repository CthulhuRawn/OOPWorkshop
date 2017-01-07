using System.ComponentModel.DataAnnotations;

namespace EZVet.Validators
{
    public class AboveAttribute : ValidationAttribute
    {
        private int _min;

        public AboveAttribute(int min)
        {
            _min = min;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(value is int))
                throw new ValidationException("value must be of integer type");

            if ((int)value < _min)
            {
                return new ValidationResult("Value below minimal");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
using System.ComponentModel.DataAnnotations;

namespace DTO.Validators
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
            if (!(value is int) && !(value is double))
                throw new ValidationException("value must be of numeric type");

            var valueToTest = 0;
            if (value is double)
            {
                valueToTest = (int) (double) value;
            }
            else
            {
                valueToTest = (int)value;
            }

            if ((double)valueToTest < _min)
            {
                return new ValidationResult("Value below minimal");
            }
            return ValidationResult.Success;
        }
    }
}
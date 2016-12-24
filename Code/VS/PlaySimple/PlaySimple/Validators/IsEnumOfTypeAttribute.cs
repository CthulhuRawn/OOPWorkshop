using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PlaySimple.Validators
{
    public class IsEnumOfTypeAttribute : ValidationAttribute
    {
        private Type _type;

        public IsEnumOfTypeAttribute(Type type)
        {
            _type = type;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (!_type.IsEnum)
                throw new ValidationException("type must be an enum");


            var enumTypesArr = Enum.GetValues(_type).GetEnumerator();

            enumTypesArr.Reset();

            while (enumTypesArr.MoveNext())
            {
                if ((int)enumTypesArr.Current == (int)value)
                   return ValidationResult.Success;
            }

            return new ValidationResult("Invalid enum");
        }
    }
}
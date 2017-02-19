using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using Domain;
using NHibernate;

namespace EZVet.Validators
{
    public class ListExistsInDbAttribute : ValidationAttribute
    {
        private Type _type;

        public ListExistsInDbAttribute(Type type)
        {
            _type = type;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ISession session = null; // NhibernateManager.Instance.OpenSession();

            try
            {
                var valueAsList = (IList)value;
                
                foreach(var currVal in valueAsList)
                {
                    var entity = session.Get(_type, ((Entity)currVal).Id);
                    if (entity == null)
                    {
                        return new ValidationResult("Entity doesn't exist");
                    }
                }

                return ValidationResult.Success;
            }
            finally
            {
            }
        }
    }
}
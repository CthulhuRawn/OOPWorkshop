using NHibernate;
using PlaySimple.Common;
using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PlaySimple.Validators
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
                IList valueAsList = (IList)value;
                
                foreach(object currVal in valueAsList)
                {
                    var entity = session.Get(_type, ((Domain.Entity)currVal).Id);
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
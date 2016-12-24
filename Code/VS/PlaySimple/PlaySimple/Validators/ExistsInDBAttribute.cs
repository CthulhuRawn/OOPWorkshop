using NHibernate;
using PlaySimple.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace PlaySimple.Validators
{
    public class ExistsInDBAttribute : ValidationAttribute
    {
        private Type _type;

        public ExistsInDBAttribute(Type type)
        {
            _type = type;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (!_type.IsSubclassOf(typeof(Domain.Entity)))
                throw new ValidationException("type must inherit from Domain.Entity");

            ISession session = (ISession)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ISession));
            try
            {
                // TODO cast to Entity<TDTO, TDomain>
                var entity = session.Get(_type, ((DTOs.EntityBase)value).Id);
                if (entity == null)
                {
                    return new ValidationResult("Entity doesn't exist");
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            finally
            {
            }
        }
    }
}
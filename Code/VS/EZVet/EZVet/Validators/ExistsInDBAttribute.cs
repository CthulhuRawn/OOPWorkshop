using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Http;
using Domain;
using EZVet.DTOs;
using NHibernate;

namespace EZVet.Validators
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

            if (!_type.IsSubclassOf(typeof(Entity)))
                throw new ValidationException("type must inherit from Domain.Entity");

            var session = (ISession)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ISession));
            try
            {
                // TODO cast to Entity<TDTO, TDomain>
                var entity = session.Get(_type, ((EntityBase)value).Id);
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
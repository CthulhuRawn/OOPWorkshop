using System;
using Domain;
using System.ComponentModel.DataAnnotations;
using PlaySimple.Validators;

namespace PlaySimple.DTOs
{
    public class Field : Entity<DTOs.Field, Domain.Field>
    {
        [MaxLength(20)]
        public virtual string Name { get; set; }

        [IsEnumOfType(typeof(Consts.Decodes.FieldType))]
        public virtual int Type { get; set; }

        [IsEnumOfType(typeof(Consts.Decodes.FieldSize))]
        public virtual int Size { get; set; }

        public override Field Initialize(Domain.Field domain)
        {
            Id = domain.Id;
            Name = domain.Name;
            Size = domain.Size.Id;
            Type = domain.Type.Id;

            return this;
        }
    }
}

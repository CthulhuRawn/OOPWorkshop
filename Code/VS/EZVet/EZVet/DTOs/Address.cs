using System.ComponentModel.DataAnnotations;

namespace EZVet.DTOs
{
    public class Address : Entity<DTOs.Address, Domain.Address>
    {
        [MaxLength(30)]
        public virtual string Country { get; set; }
        [MaxLength(50)]
        public virtual string City { get; set; }
        [MaxLength(50)]
        public virtual string StreetName { get; set; }
        public virtual int StreetNumber { get; set; }

        public override Address Initialize(Domain.Address domain)
        {
            Id = domain.Id;
            Country = domain.Country;
            City = domain.City;
            StreetName = domain.StreetName;
            StreetNumber = domain.StreetNumber;

            return this;
        }
    }
}

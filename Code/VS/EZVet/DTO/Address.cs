using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class Address : Entity<Address, Domain.Address>
    {
        [MaxLength(30)]
        public string Country { get; set; }
        [MaxLength(50)]
        public string City { get; set; }
        [MaxLength(50)]
        public string StreetName { get; set; }
        public int StreetNumber { get; set; }

        public override Address Initialize(Domain.Address domain)
        {
            Id = domain.Id;
            Country = domain.Country;
            City = domain.City;
            StreetName = domain.StreetName;
            StreetNumber = domain.StreetNumber;

            return this;
        }

        public string ForUI()
        {
            return Country + ", " + City + ", " + StreetName + " " + StreetNumber;
        }
    }
}

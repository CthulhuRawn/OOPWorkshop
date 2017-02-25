﻿using Domain;

namespace Maps
{
    public class DoctorMap : EntityMap<Doctor>
    {
        public DoctorMap()
        {
            Map(x => x.FirstName);
            Map(x => x.LastName);
            Map(x => x.Password);
            Map(x => x.BirthDate);
            Map(x => x.Email);
            Map(x => x.DoctorCode);
            Map(x => x.Notes);
            Map(x => x.OpeningHours);
            Map(x => x.Phone);

            References(x => x.Address).Cascade.All();

            HasMany(x => x.Animals);
            HasManyToMany(x => x.AnimalTypes);
            HasMany(x => x.Treatments);
            HasMany(x => x.TreatingAnimals);
        }
    }
}

using System;
using System.Collections.Generic;

namespace Domain
{
    public class Owner : Entity
    {
        public virtual string Password { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual DateTime BirthDate { get; set; }
        public virtual string Email { get; set; }
        public virtual Address Address { get; set; }

        public virtual IList<Animal> Animals { get; set; }

        public virtual string GetName()
        {
            return FirstName + " " + LastName;
        }
    }
}

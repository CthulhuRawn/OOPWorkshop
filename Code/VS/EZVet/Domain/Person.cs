using System;

namespace Domain
{
    public class  Person : Entity
    {
        public virtual string Password { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual DateTime BirthDate { get; set; }
        public virtual string Email { get; set; }
        public virtual Address Address { get; set; }
        public virtual string Phone { get; set; }
        public virtual string GetName()
        {
            return FirstName + " " + LastName;
        }
    }
}

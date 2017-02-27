using System.Collections.Generic;

namespace Domain
{
    public class Owner : Person
    {
        public virtual IList<Animal> Animals { get; set; }
    }
}

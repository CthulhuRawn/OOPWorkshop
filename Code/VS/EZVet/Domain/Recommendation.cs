using System;

namespace Domain
{
    public class Recommendation : Entity
    {
        public virtual Owner Owner { get; set; }
        public virtual string Text{ get; set; }
        public virtual DateTime Date { get; set; }
    }
}

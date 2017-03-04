using System;

namespace DTO
{
    public class Recommendation : Entity<Recommendation, Domain.Recommendation>
    {
        public virtual string Text { get; set; }
        public virtual string OwnerName { get; set; }
        public virtual DateTime Date { get; set; }
        
        public override Recommendation Initialize(Domain.Recommendation domain)
        {
            Id = domain.Id;
            Text = domain.Text;
            OwnerName = domain.Owner.GetName();
            Date = domain.Date;

            return this;
        }
       
    }
}

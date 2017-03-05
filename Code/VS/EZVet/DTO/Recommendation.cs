using System;

namespace DTO
{
    public class Recommendation : Entity<Recommendation, Domain.Recommendation>
    {
        public string Text { get; set; }
        public string OwnerName { get; set; }
        public DateTime Date { get; set; }
        
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

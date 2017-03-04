using System;

namespace DTO
{
    public class VisitsReport
    {
        public virtual string AnimalName { get; set; }
        public virtual string EntityName { get; set; }
        public virtual string EntityPhone { get; set; }
        public virtual DateTime Date { get; set; }
    }
}
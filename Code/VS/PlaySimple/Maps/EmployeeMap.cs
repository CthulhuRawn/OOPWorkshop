using Domain;
using FluentNHibernate.Mapping;

namespace Maps
{
    public class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            Id(x => x.Id);

            Map(x => x.Username);
            Map(x => x.Password);
            Map(x => x.FirstName);
            Map(x => x.LastName);
            Map(x => x.Email);
            Map(x => x.Salary);
        }
    }
}

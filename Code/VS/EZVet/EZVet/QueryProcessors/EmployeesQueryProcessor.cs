using System.Collections.Generic;
using System.Linq;
using EZVet.DTOs;
using LinqKit;
using NHibernate;

namespace EZVet.QueryProcessors
{
    public interface IEmployeesQueryProcessor
    {
        IEnumerable<Employee> Search(string firstName, string lastName, string eMail, int? id);

        Employee GetEmployee(int id);

        Employee Save(Employee employee);

        Employee Update(int id, Employee employee);

        bool Exists(string name);

        void Delete(int id);
    }

    public class EmployeesQueryProcessor : DBAccessBase<Domain.Employee>, IEmployeesQueryProcessor
    {
        public EmployeesQueryProcessor(ISession session) : base(session)
        {
        }
        public IEnumerable<Employee> Search(string firstName, string lastName, string eMail, int? id)
        {
            var filter = PredicateBuilder.New<Domain.Employee>(x => true);

            if (!string.IsNullOrEmpty(firstName))
            {
                filter.And(x => x.FirstName.Contains(firstName));
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                filter.And(x => x.LastName.Contains(lastName));
            }

            if (!string.IsNullOrEmpty(eMail))
            {
                filter.And(x => x.Email.Contains(eMail));
            }

            if (id.HasValue)
            {
                filter.And(x => x.Id == id);
            }

            return Query().Where(filter).ToList().Select(x => new Employee().Initialize(x));
        }

        public Employee GetEmployee(int id)
        {
            return new Employee().Initialize(Get(id));
        }

        public Employee Save(Employee employee)
        {
            var newEmployee = new Domain.Employee
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Password = employee.Username,
                Salary = employee.Salary,
                Email = employee.Email
            };

            var persistedEmployee = Save(newEmployee);

            return new Employee().Initialize(persistedEmployee);
        }

        public Employee Update(int id, Employee employee)
        {
            var existingEmployee = Get(id);

            existingEmployee.FirstName = employee.FirstName ?? existingEmployee.FirstName;
            existingEmployee.LastName = employee.LastName ?? existingEmployee.LastName;
            existingEmployee.Password = employee.Password ?? existingEmployee.Password;
            existingEmployee.Email = employee.Email ?? existingEmployee.Email;

            if (employee.Salary != 0)
                existingEmployee.Salary = employee.Salary;

            Update(id, existingEmployee);

            return new Employee().Initialize(existingEmployee);
        }

        public bool Exists(string name)
        {
            return Query().Where(emp => emp.Email == name).Any();
        }

        public void Delete(int id)
        {
            base.Delete(new Domain.Employee { Id = id });
        }
    }
}
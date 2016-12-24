using Domain;
using LinqKit;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlaySimple.QueryProcessors
{
    public interface IEmployeesQueryProcessor
    {
        IEnumerable<DTOs.Employee> Search(string firstName, string lastName, string eMail, int? id);

        DTOs.Employee GetEmployee(int id);

        DTOs.Employee Save(DTOs.Employee employee);

        DTOs.Employee Update(int id, DTOs.Employee employee);

        bool Exists(string name);

        void Delete(int id);
    }

    public class EmployeesQueryProcessor : DBAccessBase<Employee>, IEmployeesQueryProcessor
    {
        public EmployeesQueryProcessor(ISession session) : base(session)
        {
        }
        public IEnumerable<DTOs.Employee> Search(string firstName, string lastName, string eMail, int? id)
        {
            var filter = PredicateBuilder.New<Employee>(x => true);

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

            return Query().Where(filter).ToList().Select(x => new DTOs.Employee().Initialize(x));
        }

        public DTOs.Employee GetEmployee(int id)
        {
            return new DTOs.Employee().Initialize(Get(id));
        }

        public DTOs.Employee Save(DTOs.Employee employee)
        {
            Employee newEmployee = new Employee
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Username = employee.Username,
                Password = employee.Username,
                Salary = employee.Salary,
                Email = employee.Email
            };

            Employee persistedEmployee = Save(newEmployee);

            return new DTOs.Employee().Initialize(persistedEmployee);
        }

        public DTOs.Employee Update(int id, DTOs.Employee employee)
        {
            Employee existingEmployee = Get(id);

            existingEmployee.FirstName = employee.FirstName ?? existingEmployee.FirstName;
            existingEmployee.LastName = employee.LastName ?? existingEmployee.LastName;
            existingEmployee.Username = employee.Username ?? existingEmployee.Username;
            existingEmployee.Password = employee.Password ?? existingEmployee.Password;
            existingEmployee.Email = employee.Email ?? existingEmployee.Email;

            if (employee.Salary != 0)
                existingEmployee.Salary = employee.Salary;

            Update(id, existingEmployee);

            return new DTOs.Employee().Initialize(existingEmployee);
        }

        public bool Exists(string name)
        {
            return Query().Where(emp => emp.Username == name).Any();
        }

        public void Delete(int id)
        {
            base.Delete(new Employee() { Id = id });
        }
    }
}
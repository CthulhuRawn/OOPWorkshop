using System.Collections.Generic;
using System.Web.Http;
using EZVet.DTOs;
using EZVet.Filters;
using EZVet.QueryProcessors;

namespace EZVet.Controllers
{
    [Authorize(Roles = Consts.Roles.Admin + "," + Consts.Roles.Owner)]
    public class EmployeesController : ApiController
    {
        private readonly IEmployeesQueryProcessor _employessQueryProcessor;

        public EmployeesController(IEmployeesQueryProcessor employessQueryProcessor)
        {
            _employessQueryProcessor = employessQueryProcessor;
        }

        [HttpGet]
        public IEnumerable<Employee> Search(string firstName = null, string lastName = null, string eMail = null, int? id = null)
        {
            return _employessQueryProcessor.Search(firstName, lastName, eMail, id);
        }

        [HttpGet]
        public Employee Get(int id)
        {
            return _employessQueryProcessor.GetEmployee(id);
        }

        [HttpPost]
        [TransactionFilter]
        public EmployeeResponse Save([FromBody]Employee employee)
        {
            if (!_employessQueryProcessor.Exists(employee.Username))
                return new EmployeeResponse
                {
                    AlreadyExists = false,
                    Employee = _employessQueryProcessor.Save(employee)
                };

            return new EmployeeResponse
            {
                AlreadyExists = true
            };
        }

        [HttpPut]
        [TransactionFilter]
        public Employee Update([FromUri]int id, [FromBody]Employee employee)
        {
            return _employessQueryProcessor.Update(id, employee);
        }

        [HttpDelete]
        [TransactionFilter]
        public void Delete([FromUri]int id)
        {
            _employessQueryProcessor.Delete(id);
        }
    }
}

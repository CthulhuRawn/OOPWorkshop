using PlaySimple.Common;
using PlaySimple.Filters;
using PlaySimple.QueryProcessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PlaySimple.Controllers
{
    [Authorize(Roles = Consts.Roles.Admin + "," + Consts.Roles.Employee)]
    public class EmployeesController : ApiController
    {
        private readonly IEmployeesQueryProcessor _employessQueryProcessor;

        public EmployeesController(IEmployeesQueryProcessor employessQueryProcessor)
        {
            _employessQueryProcessor = employessQueryProcessor;
        }

        [HttpGet]
        public IEnumerable<DTOs.Employee> Search(string firstName = null, string lastName = null, string eMail = null, int? id = null)
        {
            return _employessQueryProcessor.Search(firstName, lastName, eMail, id);
        }

        [HttpGet]
        public DTOs.Employee Get(int id)
        {
            return _employessQueryProcessor.GetEmployee(id);
        }

        [HttpPost]
        [TransactionFilter]
        public DTOs.EmployeeResponse Save([FromBody]DTOs.Employee employee)
        {
            if (!_employessQueryProcessor.Exists(employee.Username))
                return new DTOs.EmployeeResponse()
                {
                    AlreadyExists = false,
                    Employee = _employessQueryProcessor.Save(employee)
                };

            return new DTOs.EmployeeResponse()
            {
                AlreadyExists = true
            };
        }

        [HttpPut]
        [TransactionFilter]
        public DTOs.Employee Update([FromUri]int id, [FromBody]DTOs.Employee employee)
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

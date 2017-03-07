using System.Linq;
using System.Web.Http;
using DTO.Enums;

namespace EZVet.Filters
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params Roles[] roles)
        {
            Roles = string.Join(",", roles.Select(x=>x.ToString()));
        }
    }
}
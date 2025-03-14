using LinqToDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPIDatabaseLinq2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly AppDataConnection _db;
        public EmployeesController(AppDataConnection db)
        {
            _db = db;
        }

        [HttpGet]
        public Employee[] GetEmployees()
        {
            var r = _db.GetTable<Employee>().Where(x => x.Id == 1).ToArray();
            var q = from employee in _db.GetTable<Employee>()
                    where employee.Id == 1
                    select employee;
            return r;
        }
    }
}

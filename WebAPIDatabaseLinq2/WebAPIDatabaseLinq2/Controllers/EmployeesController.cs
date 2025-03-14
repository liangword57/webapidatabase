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

        [HttpPost]
        public void AddEmployees(int id,string name,string position,decimal salary)
        {
            Employee employee = new Employee
            {
                Id = id,
                Name = name,
                Position = position,
                Salary = salary
            };
            _db.Insert(employee);
        }
    }
}

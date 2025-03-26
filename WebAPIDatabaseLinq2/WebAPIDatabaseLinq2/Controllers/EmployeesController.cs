using LinqToDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reactive;
using System.Reactive.Linq;

namespace WebAPIDatabaseLinq2.Controllers
{
    [Route("api/Employees")]
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

        [HttpHead]
        public IActionResult HeadEmployees(int id)
        {
            var employee = _db.GetTable<Employee>().FirstOrDefault(x => x.Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpPost]
        public void AddEmployees(int id, string name, string position, decimal salary)
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

        [HttpPut]
        public void UpdateEmployees(int id, Employee updateEmployee)
        {
            var employee = _db.GetTable<Employee>().FirstOrDefault(x => x.Id == id);
            if (employee != null)
            {
                employee.Name = updateEmployee.Name;
                employee.Position = updateEmployee.Position;
                employee.Salary = updateEmployee.Salary;
                _db.Update(employee);
            }
        }

        [HttpPatch]
        public void UpdateEmployeesSalary(int id, decimal salary)
        {
            var employee = _db.GetTable<Employee>().FirstOrDefault(x => x.Id == id);
            if (employee != null)
            {
                employee.Salary = salary;
                _db.Update(employee);
            }
        }

        [HttpDelete]
        public void DeleteEmployees(int id)
        {
            var employee = _db.GetTable<Employee>().FirstOrDefault(x => x.Id == id);
            if (employee != null)
            {
                _db.Delete(employee);
            }
        }
    }
}

using CRUD_Operations_Assignment.Entities;
using CRUD_Operations_Assignment.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_Operations_Assignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository employeeRepository;

        public EmployeeController(IEmployeeRepository _employeeRepository)
        {
            employeeRepository = _employeeRepository;
        }

        // CRUD Operations 

        // Get All Employees End Point
        [HttpGet]
        public IActionResult GetEmployees()
        {
            return Ok(employeeRepository.GetAll());
        }

        // Get Employee By His Id End Point
        [HttpGet("{id:int}",Name = "EmployeeDetailsRoute")]
        public IActionResult GetById(int id)
        {
            // Worst Case
            if (employeeRepository.GetById(id) == null)
                return BadRequest("Employee Not Found");
      
            // Happy Case
            return Ok(employeeRepository.GetById(id));
        }

        // Get Employee By His Name End Point
        [HttpGet("{name:alpha}")]
        public IActionResult GetByName(string name)
        {
            // Worst Case
            if (employeeRepository.GetByName(name) == null)
                return BadRequest("Employee Not Found");
            
            // Happy Case
            return Ok(employeeRepository.GetByName(name));
        }

        // Add New Employee End Point
        [HttpPost]
        public IActionResult PostEmployee(Employee newEmployee)
        {
            // Worst Case
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // HappyCase
            employeeRepository.Post(newEmployee);
            string url = Url.Link("EmployeeDetailsRoute", new { id = newEmployee.Id });
            return Created(url, newEmployee);
        }

        // Update Employee End Point
        [HttpPut("{id}")]
        public IActionResult PutEmployee(int id , Employee employee)
        {
            // Worst Case
            if (employeeRepository.GetById(id) == null)
                return BadRequest("Employee Not found");

            // Worst Case
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // HappyCase
            employeeRepository.Update(id, employee);
            return StatusCode(StatusCodes.Status204NoContent, "Saved Succeeded");
        }

        // Delete Employee End Point 
        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee (int id)
        {
            // Worst Case
            if (employeeRepository.GetById(id) == null)
                return BadRequest("Employee Not Found");

            // Happy case
            employeeRepository.Delete(id);
            return Ok("Removed Succeeded");
        }
    }
}

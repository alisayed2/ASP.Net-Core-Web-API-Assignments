using Assignment_2.Contexts;
using Assignment_2.DTOs;
using Assignment_2.Entities;
using Microsoft.EntityFrameworkCore;

namespace Assignment_2.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private ITIDbContext context;

        public EmployeeRepository(ITIDbContext _context)
        {
            context = _context;
        }

        public List<Employee> GetAll()
        {
            return context.Employees.AsNoTracking().ToList();
        }

        public Employee GetById(int id)
        {
            return context.Employees.FirstOrDefault(e => e.Id == id);
        }

        public Employee GetByName(string name)
        {
            return context.Employees.FirstOrDefault(e => e.Name == name);
        }

        public void Post(Employee employee)
        {
            context.Employees.Add(employee);
            context.SaveChanges();
        }

        public void Update(int id, Employee employee)
        {
            employee.Id = id;
            // Must take the id because in Core 7 if Id not Exist => Add new row in database | Id Exist => Update it
            context.Employees.Update(employee);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            context.Employees.Remove(GetById(id));
            context.SaveChanges();
        }

        public Employee GetEmployeeDetails(int id)
        {
            return context.Employees
                          .Include(e => e.Department)
                          .FirstOrDefault(e => e.Id == id);
        }
    }
}

using Assignment_2.DTOs;
using Assignment_2.Entities;

namespace Assignment_2.Repositories
{
    public interface IEmployeeRepository
    {
        List<Employee> GetAll();
        Employee GetById(int id);
        Employee GetByName(string name);
        void Post(Employee employee);
        void Delete(int id);
        void Update(int id, Employee employee);
        Employee GetEmployeeDetails(int id);
    }
}

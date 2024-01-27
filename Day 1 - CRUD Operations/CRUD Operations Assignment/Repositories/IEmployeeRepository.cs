using CRUD_Operations_Assignment.Entities;

namespace CRUD_Operations_Assignment.Repositories
{
    public interface IEmployeeRepository
    {
        List<Employee> GetAll();
        Employee GetById(int id);
        Employee GetByName(string name);
        void Post (Employee employee);
        void Delete(int id);
        void Update(int id , Employee employee);
    }
}

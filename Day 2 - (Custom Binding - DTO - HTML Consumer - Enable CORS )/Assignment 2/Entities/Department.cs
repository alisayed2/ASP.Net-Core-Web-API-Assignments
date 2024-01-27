using System.Text.Json.Serialization;

namespace Assignment_2.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ManagerName { get; set; }
        //[JsonIgnore]
        public virtual ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment_2.Entities
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(8)]
        [MinLength(3)]
        public string Name { get; set; }

        [Range(4000, 10000)]
        public int Salary { get; set; }

        public int? Age { get; set; }
        public string Address { get; set; }

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }
        public virtual Department? Department { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace CRUD_Operations_Assignment.Entities
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(8)]
        [MinLength(3)]
        public string Name{ get; set; }

        [Range(4000,10000)]
        public int Salary { get; set; }

        public int? Age { get; set; }
        public string Address { get; set; }

    }
}

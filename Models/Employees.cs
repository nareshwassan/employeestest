using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace employeestest.Models
{
    [Table("employees")]
    public class Employees
    {
        [Key]
        public int employeeId { get; set; }
        public string employeeName { get; set; }
    }
}

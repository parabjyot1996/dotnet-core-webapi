using System.ComponentModel.DataAnnotations;

namespace NetCoreAPI.Dtos
{
    public class EmployeeDto
    {
        public int EmpId { get; set; }

        [Required]
        public string EmpName { get; set; }

        [Phone]
        [Required]
        public string EmpContact { get; set; }

        [EmailAddress]
        [Required]
        public string EmpEmail { get; set; }

        [MaxLength(length: 250)]
        [Required]
        public string EmpAddress { get; set; }
    }
}
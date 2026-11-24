using System.ComponentModel.DataAnnotations;

namespace AirlineApi.Models
{
    public class CrewMember
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; } = string.Empty;
        [Required]
        public string Role { get; set; } = string.Empty; // 1-й пилот, 2-й пилот, Штурман, Радист, Стюардесса
        [Required]
        public string EmployeeNumber { get; set; } = string.Empty;
        public bool MedicalClearance { get; set; } = true;
    }
}

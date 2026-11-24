using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AirlineApi.Models
{
    public class Flight
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FlightNumber { get; set; } = string.Empty;
        [Required]
        public string Aircraft { get; set; } = string.Empty;
        [Required]
        public string Departure { get; set; } = string.Empty;
        [Required]
        public string Destination { get; set; } = string.Empty;
        public string DepartureDate { get; set; } = string.Empty;
        public string DepartureTime { get; set; } = string.Empty;
        public string Status { get; set; } = "Формируется";

        public List<FlightCrewAssignment> CrewAssignments { get; set; } = new();
    }
}

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AirlineApi.Models
{
    public class FlightCrewAssignment
    {
        [Key]
        public int Id { get; set; }
        public int FlightId { get; set; }
        [JsonIgnore]
        public Flight? Flight { get; set; }

        public int CrewMemberId { get; set; }
        public CrewMember? CrewMember { get; set; }

        public string AssignedRole { get; set; } = string.Empty;
    }
}

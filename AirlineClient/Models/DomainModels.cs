using System.Collections.Generic;

namespace AirlineClient.Models
{
    public class CrewMember
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = "1-й пилот";
        public string EmployeeNumber { get; set; } = string.Empty;
        public bool MedicalClearance { get; set; } = true;

        public string DisplayInfo => $"{FullName} ({Role}, №{EmployeeNumber})" + (MedicalClearance ? "" : " [Нет допуска]");
    }

    public class FlightCrewAssignment
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public int CrewMemberId { get; set; }
        public CrewMember? CrewMember { get; set; }
        public string AssignedRole { get; set; } = string.Empty;
    }

    public class Flight
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; } = string.Empty;
        public string Aircraft { get; set; } = string.Empty;
        public string Departure { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public string DepartureDate { get; set; } = string.Empty;
        public string DepartureTime { get; set; } = string.Empty;
        public string Status { get; set; } = "Формируется";
        public List<FlightCrewAssignment> CrewAssignments { get; set; } = new();
    }
}

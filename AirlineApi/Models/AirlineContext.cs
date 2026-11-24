using Microsoft.EntityFrameworkCore;

namespace AirlineApi.Models
{
    public class AirlineContext : DbContext
    {
        public AirlineContext(DbContextOptions<AirlineContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Flight> Flights { get; set; } = null!;
        public DbSet<CrewMember> CrewMembers { get; set; } = null!;
        public DbSet<FlightCrewAssignment> FlightCrewAssignments { get; set; } = null!;
    }
}

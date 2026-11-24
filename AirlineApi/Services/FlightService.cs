using AirlineApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AirlineApi.Services
{
    public class FlightService : IService<Flight>
    {
        private readonly AirlineContext _db;

        public FlightService(AirlineContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Flight>> GetAll()
        {
            return await _db.Flights
                .Include(f => f.CrewAssignments)
                .ThenInclude(a => a.CrewMember)
                .OrderBy(f => f.Id)
                .ToListAsync();
        }

        public async Task<Flight?> GetById(int id)
        {
            return await _db.Flights
                .Include(f => f.CrewAssignments)
                .ThenInclude(a => a.CrewMember)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task Create(Flight entity)
        {
            _db.Flights.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Update(Flight entity)
        {
            var existing = await _db.Flights.FindAsync(entity.Id);
            if (existing != null)
            {
                existing.FlightNumber = entity.FlightNumber;
                existing.Aircraft = entity.Aircraft;
                existing.Departure = entity.Departure;
                existing.Destination = entity.Destination;
                existing.DepartureDate = entity.DepartureDate;
                existing.DepartureTime = entity.DepartureTime;
                existing.Status = entity.Status;
                await _db.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var existing = await _db.Flights
                .Include(f => f.CrewAssignments)
                .FirstOrDefaultAsync(f => f.Id == id);
            if (existing != null)
            {
                _db.FlightCrewAssignments.RemoveRange(existing.CrewAssignments);
                _db.Flights.Remove(existing);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<Flight?> AssignCrewMember(int flightId, int crewMemberId, string role)
        {
            var flight = await GetById(flightId);
            var member = await _db.CrewMembers.FindAsync(crewMemberId);
            if (flight == null || member == null) return null;

            // For unique roles (not Стюардесса), replace existing assignment for that role
            if (role != "Стюардесса")
            {
                var prev = flight.CrewAssignments.FirstOrDefault(a => a.AssignedRole == role);
                if (prev != null) _db.FlightCrewAssignments.Remove(prev);
            }
            else
            {
                // Avoid duplicate same stewardess on same flight
                if (flight.CrewAssignments.Any(a => a.CrewMemberId == crewMemberId))
                    return flight;
            }

            _db.FlightCrewAssignments.Add(new FlightCrewAssignment
            {
                FlightId = flightId,
                CrewMemberId = crewMemberId,
                AssignedRole = role
            });
            await _db.SaveChangesAsync();

            await RecalculateFlightStatus(flightId);
            return await GetById(flightId);
        }

        public async Task<Flight?> RemoveCrewAssignment(int flightId, int assignmentId)
        {
            var assignment = await _db.FlightCrewAssignments
                .FirstOrDefaultAsync(a => a.Id == assignmentId && a.FlightId == flightId);
            if (assignment != null)
            {
                _db.FlightCrewAssignments.Remove(assignment);
                await _db.SaveChangesAsync();
                await RecalculateFlightStatus(flightId);
            }
            return await GetById(flightId);
        }

        public async Task RecalculateFlightStatus(int flightId)
        {
            var flight = await GetById(flightId);
            if (flight == null || flight.Status == "Отменен") return;

            var roles = flight.CrewAssignments.ToList();
            bool hasCaptain = roles.Any(a => a.AssignedRole == "1-й пилот" && a.CrewMember?.MedicalClearance == true);
            bool hasFirstOfficer = roles.Any(a => a.AssignedRole == "2-й пилот" && a.CrewMember?.MedicalClearance == true);
            bool hasNavigator = roles.Any(a => a.AssignedRole == "Штурман" && a.CrewMember?.MedicalClearance == true);
            bool hasRadio = roles.Any(a => a.AssignedRole == "Радист" && a.CrewMember?.MedicalClearance == true);
            int stewardessCount = roles.Count(a => a.AssignedRole == "Стюардесса" && a.CrewMember?.MedicalClearance == true);

            flight.Status = (hasCaptain && hasFirstOfficer && hasNavigator && hasRadio && stewardessCount >= 2)
                ? "Готов к вылету"
                : "Формируется";

            await _db.SaveChangesAsync();
        }
    }
}

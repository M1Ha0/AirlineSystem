using AirlineApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AirlineApi.Services
{
    public class CrewService : IService<CrewMember>
    {
        private readonly AirlineContext _db;

        public CrewService(AirlineContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<CrewMember>> GetAll()
        {
            return await _db.CrewMembers.OrderBy(c => c.Role).ThenBy(c => c.FullName).ToListAsync();
        }

        public async Task<CrewMember?> GetById(int id)
        {
            return await _db.CrewMembers.FindAsync(id);
        }

        public async Task Create(CrewMember entity)
        {
            _db.CrewMembers.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Update(CrewMember entity)
        {
            var existing = await _db.CrewMembers.FindAsync(entity.Id);
            if (existing != null)
            {
                existing.FullName = entity.FullName;
                existing.Role = entity.Role;
                existing.EmployeeNumber = entity.EmployeeNumber;
                existing.MedicalClearance = entity.MedicalClearance;
                await _db.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var existing = await _db.CrewMembers.FindAsync(id);
            if (existing != null)
            {
                var related = _db.FlightCrewAssignments.Where(a => a.CrewMemberId == id);
                _db.FlightCrewAssignments.RemoveRange(related);
                _db.CrewMembers.Remove(existing);
                await _db.SaveChangesAsync();
            }
        }
    }
}

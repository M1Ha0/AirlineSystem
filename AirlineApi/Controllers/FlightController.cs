using AirlineApi.Models;
using AirlineApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirlineApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class FlightController : ControllerBase
    {
        private readonly FlightService _flightService;

        public FlightController(FlightService service)
        {
            _flightService = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Flight>>> GetAllFlights()
        {
            return Ok(await _flightService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Flight>> GetFlightById(int id)
        {
            var flight = await _flightService.GetById(id);
            if (flight == null) return NotFound();
            return Ok(flight);
        }

        [HttpPost]
        public async Task<ActionResult<Flight>> CreateFlight([FromBody] Flight flight)
        {
            await _flightService.Create(flight);
            return CreatedAtAction(nameof(GetFlightById), new { id = flight.Id }, flight);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Flight>> UpdateFlight(int id, [FromBody] Flight flight)
        {
            if (flight.Id != id) return BadRequest();
            await _flightService.Update(flight);
            return Ok(await _flightService.GetById(id));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _flightService.Delete(id);
            return NoContent();
        }

        [HttpPost("{flightId}/assign")]
        public async Task<ActionResult<Flight>> AssignCrew(int flightId, [FromQuery] int crewMemberId, [FromQuery] string role)
        {
            var updated = await _flightService.AssignCrewMember(flightId, crewMemberId, role);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{flightId}/assign/{assignmentId}")]
        public async Task<ActionResult<Flight>> RemoveAssignment(int flightId, int assignmentId)
        {
            var updated = await _flightService.RemoveCrewAssignment(flightId, assignmentId);
            if (updated == null) return NotFound();
            return Ok(updated);
        }
    }
}

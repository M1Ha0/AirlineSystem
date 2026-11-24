using AirlineApi.Models;
using AirlineApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirlineApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CrewController : ControllerBase
    {
        private readonly CrewService _crewService;

        public CrewController(CrewService service)
        {
            _crewService = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CrewMember>>> GetAllCrew()
        {
            return Ok(await _crewService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CrewMember>> GetCrewById(int id)
        {
            var member = await _crewService.GetById(id);
            if (member == null) return NotFound();
            return Ok(member);
        }

        [HttpPost]
        public async Task<ActionResult<CrewMember>> CreateCrew([FromBody] CrewMember member)
        {
            await _crewService.Create(member);
            return CreatedAtAction(nameof(GetCrewById), new { id = member.Id }, member);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CrewMember>> UpdateCrew(int id, [FromBody] CrewMember member)
        {
            if (member.Id != id) return BadRequest();
            await _crewService.Update(member);
            return Ok(member);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCrew(int id)
        {
            await _crewService.Delete(id);
            return NoContent();
        }
    }
}

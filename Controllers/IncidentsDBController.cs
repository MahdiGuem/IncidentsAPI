using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IncidentAPI_Mahdi.Models;

namespace IncidentAPI_Mahdi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class IncidentsDbController : ControllerBase
	{
		private readonly IncidentsDbContext _context;
		private static readonly string[] AllowedSeverities = { "LOW", "MEDIUM", "HIGH", "CRITICAL" };
		private static readonly string[] AllowedStatuses = { "OPEN", "IN PROGRESS", "RESOLVED" };
		public IncidentsDbController(IncidentsDbContext context)
		{
			_context = context;
		}

		// GET: api/IncidentsDb
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Incident>>> GetIncidents()

			=> await _context.Incidents.ToListAsync();

		// POST: api/IncidentsDb
		[HttpPost]
		public async Task<ActionResult<Incident>> PostIncident(Incident incident)
		{

			incident.Status = "OPEN";
			incident.CreatedAt = DateTime.Now;
			_context.Incidents.Add(incident);
			await _context.SaveChangesAsync();
			return CreatedAtAction(nameof(GetIncidents), new { id = incident.Id }, incident);
		}


        //« Action ajoutée par mon collaborateur ».
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> PutIncidentStatus(int id, string status)
        {
            if (!AllowedStatuses.Contains(status.ToUpper()))
            {
                return BadRequest($"Status must be one of the following: {string.Join(", ",
                AllowedStatuses)}");
            }
            var incident = await _context.Incidents.FindAsync(id);
            if (incident == null)
                return NotFound();
            incident.Status = status;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Synchronous Filter by Status
        [HttpGet("FilterByStatus")]
		public IActionResult FilterByStatus([FromQuery] string status)
		{
			var result = _context.Incidents
				.Where(i => i.Status.Contains(status))
				.ToList();
			return Ok(result);
		}
		// GET: api/IncidentsDb/FilterByStatusAsync?status=OPEN
		[HttpGet("FilterByStatusAsync")]
		public async Task<IActionResult> FilterByStatusAsync([FromQuery] string status)
		{
			var result = await _context.Incidents
				.Where(i => i.Status.Contains(status))
				.ToListAsync();
			return Ok(result);
		}

		// GET: api/IncidentsDb/FilterBySeverityAsync?severity=HIGH
		[HttpGet("FilterBySeverityAsync")]
		public async Task<IActionResult> FilterBySeverityAsync([FromQuery] string severity)
		{
			var result = await _context.Incidents
				.Where(i => i.Severity.Contains(severity))
				.ToListAsync();
			return Ok(result);
		}
	}
}
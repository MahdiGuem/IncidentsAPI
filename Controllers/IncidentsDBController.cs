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
		private static readonly string[] AllowedStatuses = { "OPEN", "IN PROGRESS", "RESOLVED" };

		public IncidentsDbController(IncidentsDbContext context)
		{
			_context = context;
		}

		// GET: api/IncidentsDb (Get All) [cite: 64]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Incident>>> GetIncidents()
		{
			return await _context.Incidents.ToListAsync();
		}

		// GET: api/IncidentsDb/5 [cite: 64]
		[HttpGet("{id}")]
		public async Task<ActionResult<Incident>> GetIncident(int id)
		{
			var incident = await _context.Incidents.FindAsync(id);
			return incident == null ? NotFound() : incident;
		}

		// POST: api/IncidentsDb [cite: 64, 147]
		[HttpPost]
		public async Task<ActionResult<Incident>> PostIncident(Incident incident)
		{
			incident.Status = "OPEN"; // Requirements from Part 5 [cite: 147, 162]
			incident.CreatedAt = DateTime.Now;
            _context.Incidents.Add(incident);
			await _context.SaveChangesAsync();
			return CreatedAtAction(nameof(GetIncident), new { id = incident.Id }, incident);
		}

		// PUT: api/IncidentsDb/5 [cite: 64]
		[HttpPut("{id}")]
		public async Task<IActionResult> PutIncident(int id, Incident incident)
		{
			if (id != incident.Id) return BadRequest();
			_context.Entry(incident).State = EntityState.Modified;
			await _context.SaveChangesAsync();
			return NoContent();
		}

		// DELETE: api/IncidentsDb/5 [cite: 64]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteIncident(int id)
		{
			var incident = await _context.Incidents.FindAsync(id);
			if (incident == null) return NotFound();
			_context.Incidents.Remove(incident);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		// --- FILTERING (Synchronous - TP2 Part 2) --- [cite: 95, 97]

		[HttpGet("FilterByStatus")]
		public IActionResult FilterByStatus([FromQuery] string status)
		{
			var result = _context.Incidents
				.Where(i => i.Status.Contains(status))
				.ToList(); // LINQ Query [cite: 98]
			return Ok(result);
		}

		[HttpGet("FilterBySeverity")]
		public IActionResult FilterBySeverity([FromQuery] string severity)
		{
			var result = _context.Incidents
				.Where(i => i.Severity.Contains(severity))
				.ToList();
            return Ok(result);
		}
	}
}
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

		// Synchronous Filter by Status
		[HttpGet("FilterByStatus")]
		public IActionResult FilterByStatus([FromQuery] string status)
		{
			var result = _context.Incidents
				.Where(i => i.Status.Contains(status))
				.ToList();
			return Ok(result);
		}
	}
}
using System.ComponentModel.DataAnnotations;
using IncidentAPI_Mahdi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IncidentAPI_Mahdi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class IncidentsDbController : ControllerBase
	{
		private readonly IncidentsDbContext _context;

		[RegularExpression("LOW|MEDIUM|HIGH|CRITICAL", ErrorMessage = "Invalid severity")]
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
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

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

		[HttpGet("{id}")]
		public async Task<ActionResult<Incident>> GetIncident(int id)
		{
			var incident = await _context.Incidents.FindAsync(id);
			if (incident == null)
			{
				return NotFound();
			}
			return incident;
		}
		[HttpPut("{id}")]
		public async Task<IActionResult> PutIncident(int id, Incident updatedIncident)
		{
			if (id != updatedIncident.Id)
			{
				return BadRequest();
			}
			var existingIncident = await _context.Incidents.FindAsync(id);
			if (existingIncident == null)
			{
				return NotFound();
			}
			existingIncident.Title = updatedIncident.Title;
			existingIncident.Description = updatedIncident.Description;
			existingIncident.Severity = updatedIncident.Severity;
			existingIncident.Status = updatedIncident.Status;
			await _context.SaveChangesAsync();
			return NoContent();
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteIncident(int id)
		{
			var incident = await _context.Incidents.FindAsync(id);
			if (incident == null)
			{
				return NotFound();
			}
			_context.Incidents.Remove(incident);
			await _context.SaveChangesAsync();
			return NoContent();
		}
	}
}
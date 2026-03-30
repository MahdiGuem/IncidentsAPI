using Microsoft.EntityFrameworkCore;
using IncidentAPI_Mahdi.Models;

namespace IncidentAPI_Mahdi.Models
{
	public class IncidentsDbContext : DbContext
	{
		public IncidentsDbContext(DbContextOptions<IncidentsDbContext> options)
			: base(options)
		{
		}

		public virtual DbSet<Incident> Incidents { get; set; }
	}
}
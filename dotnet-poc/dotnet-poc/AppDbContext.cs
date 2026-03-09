using Microsoft.EntityFrameworkCore;

namespace dotnet_poc
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}
		// Add DbSets here later when you have models
	}
}
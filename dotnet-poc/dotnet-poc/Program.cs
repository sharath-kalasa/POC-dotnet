using Microsoft.EntityFrameworkCore;
using dotnet_poc;

var builder = WebApplication.CreateBuilder(args);

// 1) Register CORS
builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(policy =>
	{
		policy
			.AllowAnyOrigin()
			.AllowAnyHeader()
			.AllowAnyMethod();
	});
});

// 2) Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3) Other services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Health check
app.MapGet("/health", () => Results.Ok("API Healthy"));

// ✅ DB Connection Test Endpoint
app.MapGet("/db-test", async (AppDbContext db) =>
{
	try
	{
		await db.Database.OpenConnectionAsync();
		await db.Database.CloseConnectionAsync();
		return Results.Ok("✅ Database connection successful!");
	}
	catch (Exception ex)
	{
		return Results.Problem($"❌ Database connection failed: {ex.Message}");
	}
});

// 4) Correct order: Routing -> CORS -> Authorization -> Endpoints
app.UseRouting();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();
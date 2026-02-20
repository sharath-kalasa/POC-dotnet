var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS for React dev server (localhost:3000)
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowReact", policy =>
	{
		policy.WithOrigins("http://192.168.40.53:8080")
			  .AllowAnyHeader()
			  .AllowAnyMethod();
	});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Key: Add CORS BEFORE UseAuthorization
app.UseCors("AllowReact");

app.UseAuthorization();

app.MapControllers();

app.Run();

var builder = WebApplication.CreateBuilder(args);

// 1) Register CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()    // for local dev; tighten later
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// 2) Other services
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

// 3) Correct order: Routing -> CORS -> Authorization -> Endpoints
app.UseRouting();
app.UseCors();          // uses the default policy above
app.UseAuthorization();
app.MapControllers();

app.Run();

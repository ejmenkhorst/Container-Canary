var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


var canaryNames = new[]
{
    "Charlie", "Sunny", "Goldie", "Peep", "Amber", "Tweet", "Nugget"
};

app.MapGet("/canary", () =>
{
    var name = canaryNames[Random.Shared.Next(canaryNames.Length)];
    return Results.Ok(new { name });
})
.WithName("GetCanaryName");

app.Run();
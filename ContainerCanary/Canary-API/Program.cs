var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Resolve mock service URL from config or environment (fallback to localhost)
var mockUrl = builder.Configuration["MOCK_SERVICE_URL"]
             ?? Environment.GetEnvironmentVariable("MOCK_SERVICE_URL")
             ?? "http://localhost:5002";

// Register an HttpClient for the mock service
builder.Services.AddHttpClient("MockService", c => c.BaseAddress = new Uri(mockUrl));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/canary", async (IHttpClientFactory httpFactory) =>
{
    var client = httpFactory.CreateClient("MockService");
    string[]? names;
    try
    {
        names = await client.GetFromJsonAsync<string[]>("/generateName");
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: $"Error calling mock service: {ex.Message}", statusCode: 502);
    }

    if (names == null || names.Length == 0)
    {
        return Results.Problem("Mock service returned no names", statusCode: 502);
    }

    var name = names[Random.Shared.Next(names.Length)];
    return Results.Ok(new { name });
})
.WithName("GetCanaryName");

app.Run();
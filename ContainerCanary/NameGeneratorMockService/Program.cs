var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

var canaryNames = new[]
{
    "Erik", "Piet", "Marigold", "Daffodil", "Honey", "Blaze", "Spark", "Ginger", "Pumpkin", "Saffron", "Citrine", "Topaz"
};

app.MapGet("/generateName", () =>
{
    var name = canaryNames[Random.Shared.Next(canaryNames.Length)];
    return Results.Ok(new[] { name }); // return a single-element array
})
.WithName("GetNames");

app.Run();
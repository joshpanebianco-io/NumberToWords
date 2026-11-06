using NumberToWords.Api;

var app = WebApplication.CreateBuilder(args).Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/convert", (string? number) =>
{
    if (NumberConverter.TryConvert(number, out string words))
    {
        return Results.Ok(new { words });
    }

    return Results.BadRequest(new
    {
        error = "Enter an amount like 123.45, using digits only, with up to 15 digits before the decimal point and 2 after."
    });
});

app.Run();

// Lets the API tests start the app with WebApplicationFactory<Program>
public partial class Program { }

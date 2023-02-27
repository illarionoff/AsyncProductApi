using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=RequestDB.db"));
var app = builder.Build();
app.UseHttpsRedirection();

// Start Endpoint
app.MapPost("api/v1/products", async (AppDbContext context, ListingRequest request) =>
{
    if(request == null)
    {
        return Results.BadRequest();
    }

    request.RequestStatus = "ACCEPT";
    request.EstimatedCompletionTime = "2023-02-27:20:45:00";

    await context.ListingRequests.AddAsync(request);

    await context.SaveChangesAsync();

    return Results.Accepted($"api/v/productstatus/{request.RequestId}", request);
});

app.Run();
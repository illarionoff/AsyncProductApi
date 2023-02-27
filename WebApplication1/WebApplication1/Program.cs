using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dtos;
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

// Status Endpoint
app.MapGet("api/v1/productstatus/{requestId}", (AppDbContext context, string requestId) =>
{
    var listingRequest = context.ListingRequests.FirstOrDefault(x => x.RequestId == requestId);
    if(listingRequest == null)
    {
        return Results.NotFound();
    }

    ListingStatus listingStatus = new ListingStatus();
    listingStatus.RequestStatus = listingRequest.RequestStatus;
    listingStatus.ResourseUrl = string.Empty;

    if(listingRequest.RequestStatus!.ToUpper() == "COMPLETE")
    {
        listingStatus.ResourseUrl = $"api/v1/products/{Guid.NewGuid().ToString()}";
        return Results.Ok(listingStatus);
    }

    listingStatus.EstimatedCompletionTime = "2023-02-27:21:00:00";

    return Results.Ok(listingStatus);
});

app.Run();
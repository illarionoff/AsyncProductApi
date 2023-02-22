using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=RequestDB.db"));
var app = builder.Build();
app.UseHttpsRedirection();
app.Run();
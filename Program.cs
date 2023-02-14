using Okoul.Models;
using Microsoft.EntityFrameworkCore;
using Okoul.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var provider = builder.Services.BuildServiceProvider();
var configuration = provider.GetRequiredService<IConfiguration>();
builder.Services.AddDbContext<OkoulContext>(item =>
item.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IQuoteService, QuoteService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

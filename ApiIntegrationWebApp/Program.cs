using ApiIntegration.Data;
using ApiIntegration.Interfaces;
using ApiIntegration.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddScoped<IImporter, Importer>();
builder.Services.AddSingleton<IApiDownloader, ApiDownloader>();
builder.Services.AddSingleton<IApiDownloaderHttpHandler, ApiDownloaderHttpHandler>();
builder.Services.AddSingleton<ITourRepository, TourRepository>();
builder.Services.AddSingleton<IProviderRepository, ProviderRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

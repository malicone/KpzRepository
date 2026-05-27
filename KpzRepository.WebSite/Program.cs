using KpzRepository.WebSite.Components;
using KpzRepository.WebSite.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ReadmeRenderer>();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

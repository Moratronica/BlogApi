using Blazored.LocalStorage;
using BlogApp;
using BlogApp.Servicios;
using BlogApp.Servicios.IServicio;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Agregar servicios aqu�
builder.Services.AddScoped<IPostServicio, PostServicio>();
builder.Services.AddScoped<IAuthServicio, AuthServicio>();

// Para usar Local Storage del navegador
builder.Services.AddBlazoredLocalStorage();

// Agregar autorizaci�n y autenticaci�n
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<AuthStateProvider>());

await builder.Build().RunAsync();

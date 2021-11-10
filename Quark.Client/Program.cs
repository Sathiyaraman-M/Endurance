using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Quark.Client.Extensions;

var host = WebAssemblyHostBuilder.CreateDefault(args).AddRootComponent().AddClientServices().Build();
host.Services.GetService<Navigation>();
await host.RunAsync();
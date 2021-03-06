global using Blazored.FluentValidation;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Components;
global using Microsoft.JSInterop;
global using MudBlazor;
global using Quark.Client.Extensions;
global using Quark.Core.Requests;
global using Quark.Core.Requests.Identity;
global using Quark.Core.Responses;
global using Quark.Shared.Constants;
global using Quark.Shared.Constants.Permission;
global using Quark.Shared.Wrapper;
global using System.Net.Http.Json;
global using System.Security.Claims;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var host = WebAssemblyHostBuilder.CreateDefault(args).AddRootComponent().AddClientServices().Build();
host.Services.GetService<Navigation>();
await host.RunAsync();
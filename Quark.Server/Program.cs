global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Quark.Server.Controllers.Utility;
global using Quark.Shared.Constants;
global using Quark.Shared.Constants.Permission;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders().AddConsole();
builder.Host.ConfigureWebHost(webBuilder =>
{
    webBuilder.UseStaticWebAssets();
    webBuilder.UseStartup<Startup>();
});
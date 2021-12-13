global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Quark.Server.Controllers.Utility;
global using Quark.Shared.Constants;
global using Quark.Shared.Constants.Permission;
using FluentValidation.AspNetCore;
using Hangfire;
using Microsoft.Extensions.FileProviders;
using Quark.Core.Configurations;
using Quark.Core.Extensions;
using Quark.Infrastructure.Extensions;
using Quark.Server.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders().AddConsole();
builder.Services.AddCors();
builder.Services.AddCurrentUserService();
builder.Services.AddSerialization();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddMailConfiguration(builder.Configuration);
builder.Services.ConfigureWritable<LibrarySettings>(builder.Configuration.GetSection("LibrarySettings"));
builder.Services.AddServerStorage();
builder.Services.AddIdentity();
builder.Services.AddJwtAuthentication(builder.Services.GetApplicationSettings(builder.Configuration));
builder.Services.AddApplicationLayer();
builder.Services.AddApplicationServices();
builder.Services.AddRepositories();
builder.Services.RegisterSwagger();
builder.Services.AddInfrastructureMappings();
builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();
builder.Services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AppConfiguration>());
builder.Services.AddRazorPages();
builder.Services.AddLazyCache();

using var app = builder.Build();

app.UseCors();
app.UseExceptionHandling(builder.Environment);
app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Files")),
    RequestPath = new PathString("/Files")
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseHangfireDashboard("/jobs", new DashboardOptions
{
    DashboardTitle = "Library Management System Jobs",
    Authorization = new[] { new HangfireAuthorizationFilter() }
});

app.UseEndpoints();
app.ConfigureSwagger();
app.InitializeSeeding();

await app.RunAsync();
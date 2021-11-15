using FluentValidation.AspNetCore;
using Hangfire;
using Microsoft.Extensions.FileProviders;
using Quark.Core.Configurations;
using Quark.Core.Extensions;
using Quark.Infrastructure.Extensions;
using Quark.Server.Extensions;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors();
        services.AddCurrentUserService();
        services.AddSerialization();
        services.AddDatabase(Configuration);
        services.AddServerStorage();
        services.AddIdentity();
        services.AddJwtAuthentication(services.GetApplicationSettings(Configuration));
        services.AddApplicationLayer();
        services.AddApplicationServices();
        services.AddRepositories();
        services.RegisterSwagger();
        services.AddInfrastructureMappings();
        services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")));
        services.AddHangfireServer();
        services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AppConfiguration>());
        services.AddRazorPages();
        services.AddLazyCache();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseCors();
        app.UseExceptionHandling(env);
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
    }
}
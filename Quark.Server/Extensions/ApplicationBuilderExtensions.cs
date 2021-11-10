using Quark.Core.Interfaces.Services;

namespace Quark.Server.Extensions;

internal static class ApplicationBuilderExtensions
{
    internal static void UseExceptionHandling(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseWebAssemblyDebugging();
        }
    }

    internal static void UseEndpoints(this IApplicationBuilder app)
    {
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapRazorPages();
            endpoints.MapFallbackToFile("index.html");
        });
    }

    internal static void ConfigureSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Quark Library Management System v1");
            options.DisplayRequestDuration();
            options.RoutePrefix = "swagger";
        });
    }

    internal static void InitializeSeeding(this IApplicationBuilder app)
    {
        foreach (IDatabaseSeeder initializer in app.ApplicationServices.CreateScope().ServiceProvider.GetServices<IDatabaseSeeder>())
        {
            initializer.Initialize();
        }
    }
}
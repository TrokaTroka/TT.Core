using TT.Core.Infra.IoC.DependencyInjection;

namespace TT.Core.Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApiConfiguration(Configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())        
            app.UseDeveloperExceptionPage();        

        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Troka Troka API"));

        app.UseApiConfiguration();
    }

}

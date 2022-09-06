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
        services.AddDependencies(Configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
    {
        app.UseDependencies(environment, Configuration);
    }

}

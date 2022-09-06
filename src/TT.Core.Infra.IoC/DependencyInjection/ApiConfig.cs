using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TT.Core.Application.Dtos.Inputs.Validators;
using TT.Core.Domain.Constants;
using TT.Core.Infra.Data.Context;

namespace TT.Core.Infra.IoC.DependencyInjection;

public static class ApiConfig
{
    public static IServiceCollection AddApiConfiguration(this IServiceCollection services, 
        IConfiguration configuration)
    {
        AppSettings.Initialize(configuration.GetSection("AppSettings").Get<AppSettings>());

        services.AddDbContext<TrokaTrokaDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("SqlServer")));

        services.AddInjection();

        services.AddControllers()
            .AddFluentValidation(fv =>
                fv.RegisterValidatorsFromAssemblyContaining<LoginDtoValidator>());

        services.AddSwagger();

        services.AddCors(options =>
        {
            options.AddPolicy("Total",
                builder =>
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
        });

        return services;
    }

    public static void UseApiConfiguration(this IApplicationBuilder app)
    {
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseCors("Total");

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}

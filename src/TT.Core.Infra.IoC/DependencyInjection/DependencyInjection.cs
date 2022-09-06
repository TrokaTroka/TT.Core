using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using TT.Core.Application.Helpers;
using TT.Core.Application.Interfaces;
using TT.Core.Application.Interfaces.Helpers;
using TT.Core.Application.Notifications;
using TT.Core.Application.Services;
using TT.Core.Domain.Interfaces.Repositories;
using TT.Core.Infra.Data.Context;
using TT.Core.Infra.Data.Repositories;

namespace TT.Core.Infra.IoC.DependencyInjection;

public static class DependencyInjection
{
    public static void AddInjection(this IServiceCollection services)
    {
        services.AddRepositories();
        services.AddServices();      
        
        services.AddJwt();
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<BaseService>();
        services.AddScoped<IAuthenticatedUser, AuthenticatedUser>();
        services.AddScoped<IAuthenticateService, AuthenticateService>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IFavoriteService, FavoriteService>();
        services.AddScoped<INotifier, Notifier>();
        services.AddScoped<IRatingService, RatingService>();
        services.AddScoped<ITradeService, TradeService>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<IEmailHelper, MailHelper>();
        services.AddScoped<ITokenHelper, TokenHelper>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<TrokaTrokaDbContext>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IBookCategoryRepository, BookCategoryRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IFavoriteRepository, FavoriteRepository>();
        services.AddScoped<ILogErrorRepository, LogErrorRepository>();
        services.AddScoped<IPhotoRepository, PhotoRepository>();
        services.AddScoped<IRatingRepository, RatingRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IResetPasswordRepository, ResetPasswordRepository>();
        services.AddScoped<ITradeRepository, TradeRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
    }        
}

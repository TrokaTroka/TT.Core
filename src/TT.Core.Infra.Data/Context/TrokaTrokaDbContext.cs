using Microsoft.EntityFrameworkCore;
using TT.Core.Domain.Entities;

namespace TT.Core.Infra.Data.Context;

public class TrokaTrokaDbContext : DbContext
{
    public TrokaTrokaDbContext(DbContextOptions<TrokaTrokaDbContext> options)
        : base(options)
    { }
    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<BookCategory> BookCategories { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Trade> Trades { get; set; }
    public DbSet<PhotosBook> PhotosBooks { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Favorite> Favorites { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<LogError> LogErrors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TrokaTrokaDbContext).Assembly);
    }
}

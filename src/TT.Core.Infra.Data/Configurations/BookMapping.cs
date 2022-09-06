using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TT.Core.Domain.Entities;

namespace TT.Core.Infra.Data.Configurations;

public class BookMapping : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(b => b.Id);

        builder
            .Property(b => b.Title)
            .IsRequired();
        builder
            .Property(b => b.Author)
            .IsRequired();
        builder
            .Property(b => b.Isbn)
            .IsRequired();
        builder
            .Property(b => b.Publisher)
            .IsRequired();
        builder
            .Property(b => b.Model)
            .IsRequired();
        builder
            .Property(b => b.Language)
            .IsRequired();
        builder
            .Property(b => b.Reason)
            .IsRequired();
        builder
            .Property(b => b.BuyPrice)
            .IsRequired();
        builder
            .Property(b => b.BuyDate)
            .IsRequired();
        builder
            .Property(b => b.IdUser)
            .IsRequired();

        builder
             .HasOne(b => b.Owner)
             .WithMany(u => u.Books)
             .HasForeignKey(b => b.IdUser);

        builder
            .HasMany(b => b.BookCategories)
            .WithOne(bc => bc.Book)
            .HasForeignKey(b => b.IdBook);

        builder
            .HasMany(b => b.Favorites)
            .WithOne(bc => bc.Book)
            .HasForeignKey(b => b.IdBook);
    }
}

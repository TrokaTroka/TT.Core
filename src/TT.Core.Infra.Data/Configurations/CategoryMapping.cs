using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TT.Core.Domain.Entities;

namespace TT.Core.Infra.Data.Configurations;

public class CategoryMapping : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(b => b.Id);
        
        builder
            .HasMany(b => b.BookCategories)
            .WithOne(bc => bc.Category)
            .HasForeignKey(b => b.IdCategory);

    }
}

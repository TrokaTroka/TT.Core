using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TT.Core.Domain.Entities;

namespace TT.Core.Infra.Data.Configurations;

public class PhotoMapping : IEntityTypeConfiguration<PhotosBook>
{
    public void Configure(EntityTypeBuilder<PhotosBook> builder)
    {
        builder
            .HasKey(p => p.Id);

        builder
            .Property(p => p.Path)
            .IsRequired();
        builder
            .Property(p => p.Name)
            .IsRequired();
        builder
            .Property(p => p.IdBook)
            .IsRequired();

        builder
             .HasOne(p => p.Book)
             .WithMany(b => b.PhotosBooks)
             .HasForeignKey(p => p.IdBook)
             .OnDelete(DeleteBehavior.Cascade);
    }
}

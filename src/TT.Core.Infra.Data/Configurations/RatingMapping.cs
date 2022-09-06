using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TT.Core.Domain.Entities;

namespace TT.Core.Infra.Data.Configurations;

public class RatingMapping : IEntityTypeConfiguration<Rating>
{
    public void Configure(EntityTypeBuilder<Rating> builder)
    {
        builder
            .HasKey(r => r.Id);

        builder
            .Property(r => r.Comment)
            .IsRequired();
        builder
            .Property(r => r.Grade)
            .IsRequired();
        builder
            .Property(r => r.IdRated)
            .IsRequired();
        builder
            .Property(r => r.IdRater)
            .IsRequired();

        builder
             .HasOne(r => r.RatedUser)
             .WithMany(u => u.Ratings)
             .HasForeignKey(r => r.IdRated)
             .OnDelete(DeleteBehavior.Restrict);
    }
}

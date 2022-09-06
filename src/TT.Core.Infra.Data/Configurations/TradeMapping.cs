using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TT.Core.Domain.Entities;

namespace TT.Core.Infra.Data.Configurations;

public class TradeMapping : IEntityTypeConfiguration<Trade>
{
    public void Configure(EntityTypeBuilder<Trade> builder)
    {
        builder
            .HasKey(t => t.Id);
    }
}

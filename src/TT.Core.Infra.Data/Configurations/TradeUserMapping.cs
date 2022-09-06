using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TT.Core.Domain.Entities;

namespace TT.Core.Infra.Data.Configurations;

public class TradeUserMapping : IEntityTypeConfiguration<TradeUser>
{
    public void Configure(EntityTypeBuilder<TradeUser> builder)
    {
        builder
            .HasKey(tu => new { tu.IdUser, tu.IdTrade });
    }
}

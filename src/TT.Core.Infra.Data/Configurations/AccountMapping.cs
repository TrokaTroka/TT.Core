using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TT.Core.Domain.Entities;

namespace TT.Core.Infra.Data.Configurations;

public class AccountMapping : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder
            .HasKey(r => r.Id);

        builder
             .HasOne(r => r.User)
             .WithOne(u => u.Account)
             .HasForeignKey<Account>(r => r.IdUser);
    }
}

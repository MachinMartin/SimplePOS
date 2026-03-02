using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimplePOS.Domain.Entities;

namespace SimplePOS.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.HasKey(u => u.Id);

        entity.Property(u => u.Username)
              .IsRequired()
              .HasMaxLength(80);

        entity.Property(u => u.Password)
              .IsRequired()
              .HasMaxLength(255);

        entity.HasOne(u => u.Tenant)
              .WithMany(t => t.Users)
              .HasForeignKey(u => u.TenantId)
              .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(u => u.TenantId);

        entity.HasIndex(u => new { u.TenantId, u.Username })
              .IsUnique();
    }
}

using EA.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EA.Repository.Configurations;

public class AdministratorConfiguration : IEntityTypeConfiguration<Administrator>
{
    public void Configure(EntityTypeBuilder<Administrator> builder)
    {
        builder.Property(administrator => administrator.Name).HasMaxLength(150).IsRequired();
        builder.Property(administrator => administrator.LastName).HasMaxLength(150).IsRequired();
        builder.Property(administrator => administrator.RegistrationTime);
        builder.Property(administrator => administrator.Login).HasMaxLength(50).IsRequired();
        builder.Property(administrator => administrator.Password).HasMaxLength(255).IsRequired();
        builder.Property(administrator => administrator.OldPassword).HasMaxLength(255).IsRequired();
    }
}
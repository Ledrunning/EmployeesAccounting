using EA.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EA.Repository.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.Property(employee => employee.Name).IsRequired();
        builder.Property(employee => employee.LastName).HasMaxLength(150).IsRequired();
        builder.Property(employee => employee.Department).HasMaxLength(50).IsRequired();
        builder.Property(employee => employee.DateTime);
        builder.Property(employee => employee.PhotoName);
        builder.Property(employee => employee.PhotoPath);
    }
}
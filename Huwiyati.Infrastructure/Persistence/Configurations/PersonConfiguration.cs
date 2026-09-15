namespace Huwiyati.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Enums;
using System;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("Persons");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.NationalNumber)
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(p => p.NationalNumber)
            .IsUnique();

        builder.Property(p => p.FirstName).HasMaxLength(50).IsRequired();
        builder.Property(p => p.FatherName).HasMaxLength(50).IsRequired();
        builder.Property(p => p.GrandfatherName).HasMaxLength(50).IsRequired();
        builder.Property(p => p.FamilyName).HasMaxLength(50).IsRequired();
        builder.Property(p => p.PlaceOfBirth).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Nationality).HasMaxLength(50).IsRequired();
        builder.Property(p => p.Governorate).HasMaxLength(50).IsRequired();
        builder.Property(p => p.District).HasMaxLength(50).IsRequired();
        builder.Property(p => p.AddressDetails).HasMaxLength(200).IsRequired();
        builder.Property(p => p.PhotoUrl).HasMaxLength(500).IsRequired(false);

        // Convert BloodGroup enum to standard symbol ("O+", "O-", etc.) in DB
        builder.Property(p => p.BloodGroup)
            .HasConversion(
                v => v.HasValue ? v.Value.ToString().Replace("Positive", "+").Replace("Negative", "-") : null,
                v => !string.IsNullOrEmpty(v) ? Enum.Parse<BloodGroup>(v.Replace("+", "Positive").Replace("-", "Negative")) : (BloodGroup?)null)
            .HasMaxLength(10)
            .IsRequired(false);

        // Convert Enums to string in DB
        builder.Property(p => p.Gender).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(p => p.MaritalStatus).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(p => p.PersonStatus).HasConversion<string>().HasMaxLength(20).IsRequired();
    }
}

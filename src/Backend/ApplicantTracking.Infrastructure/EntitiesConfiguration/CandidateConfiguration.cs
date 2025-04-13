using System;

using ApplicantTracking.Domain.Entities;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicantTracking.Infrastructure.EntitiesConfiguration;

public class CandidateConfiguration : BaseEntityConfiguration<Candidate>
{
    public override void Configure(EntityTypeBuilder<Candidate> builder)
    {
        base.Configure(builder);

        builder.Property(r => r.Name).IsRequired().HasMaxLength(80);
        builder.Property(r => r.Surname).IsRequired().HasMaxLength(150);
        builder.Property(r => r.Birthdate).IsRequired();
        builder.Property(r => r.Email).IsRequired().HasMaxLength(250);
        builder.Property(r => r.LastUpdateAt).IsRequired(false);

        builder.HasData(new Candidate()
        {
            Id = 1,
            Name = "John",
            Surname = "Doe",
            Birthdate = new DateTime(1990, 1, 2),
            Email = "john@email.com",
        });

        builder.HasData(new Candidate()
        {
            Id = 2,
            Name = "Paul",
            Surname = "Doe",
            Birthdate = new DateTime(1990, 1, 3),
            Email = "paul@email.com",
        });

        builder.HasData(new Candidate()
        {
            Id = 3,
            Name = "Erick",
            Surname = "Doe",
            Birthdate = new DateTime(1990, 1, 4),
            Email = "Erick@email.com",
        });
    }
}

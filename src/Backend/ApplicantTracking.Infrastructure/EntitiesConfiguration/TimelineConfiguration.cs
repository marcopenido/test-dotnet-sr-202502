using ApplicantTracking.Domain.Entities;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicantTracking.Infrastructure.EntitiesConfiguration;

public class TimelineConfiguration : BaseEntityConfiguration<Timeline>
{
    public override void Configure(EntityTypeBuilder<Timeline> builder)
    {
        base.Configure(builder);

        builder.Property(r => r.IdTimelineType).IsRequired();
        builder.Property(r => r.IdAggregateRoot).IsRequired();
        builder.Property(r => r.OldData).IsRequired(false);
        builder.Property(r => r.NewData).IsRequired(false);
    }
}

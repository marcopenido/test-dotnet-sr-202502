using System;

namespace ApplicantTracking.Domain.Entities.Base;

public class EntityBase
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

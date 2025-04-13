using System;

using ApplicantTracking.Domain.Entities.Base;

namespace ApplicantTracking.Domain.Entities;

public class Candidate : EntityBase
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime Birthdate { get; set; }
    public string Email { get; set; }
    public DateTime? LastUpdateAt { get; set; }
}

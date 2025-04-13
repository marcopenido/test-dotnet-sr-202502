using ApplicantTracking.Communication.Requests;

using Bogus;

namespace CommonTestUtilities.Requests;

public class RequestCandidateJsonBuilder
{
    public static RequestCandidateJson Build()
    {
        return new Faker<RequestCandidateJson>()
            .RuleFor(x => x.Name, f => f.Person.FirstName)
            .RuleFor(u => u.Surname, f => f.Person.LastName)
            .RuleFor(u => u.Birthdate, _ => DateTime.UtcNow.AddYears(-29))
            .RuleFor(x => x.Email, (f, x) => f.Internet.Email(x.Name));
    }
}

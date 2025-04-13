using ApplicantTracking.Domain.Entities;

using Bogus;

namespace CommonTestUtilities.Entities;

public class CandidateBuilder
{
    public static IList<Candidate> Collection(uint count = 2)
    {
        var list = new List<Candidate>();

        if (count == 0)
            count = 1;

        var candidateId = 1;

        for (int i = 0; i < count; i++)
        {
            var fakeCandidate = Build();
            fakeCandidate.Id = candidateId++;

            list.Add(fakeCandidate);
        }

        return list;
    }

    public static Candidate Build()
    {
        return new Faker<Candidate>()
            .RuleFor(x => x.Id, _ => 1)
            .RuleFor(x => x.Name, f => f.Person.FirstName)
            .RuleFor(u => u.Surname, f => f.Person.LastName)
            .RuleFor(u => u.Birthdate, _ => DateTime.UtcNow.AddYears(-29))
            .RuleFor(x => x.Email, (f, x) => f.Internet.Email(x.Name));
    }
}

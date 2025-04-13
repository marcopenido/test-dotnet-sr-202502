using System.Net;
using System.Text.Json;

using ApplicantTracking.Exceptions;

using FluentAssertions;

using Xunit;

namespace WebApi.Test.Candidate.Delete;

public class DeleteCandidateTest : ApplicantTrackingClassFixture
{
    public DeleteCandidateTest(CustomWebApplicationFactory factory) : base(factory) { }

    private readonly string METHOD = "candidates";

    [Fact]
    public async Task Success()
    {
        var response = await DoDelete(method: $"{METHOD}/{_cadidateId}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        response = await DoGet(method: $"{METHOD}/{_cadidateId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Error_Candidate_Not_Found()
    {
        var response = await DoDelete(method: $"{METHOD}/{1000}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        errors.Should().ContainSingle().And.Contain(errors => errors.GetString()!.Equals(ResourceMessagesException.CANDIDATE_NOT_FOUND));
    }
}

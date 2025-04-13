using System.Net;
using System.Text.Json;

using ApplicantTracking.Exceptions;

using FluentAssertions;

using Xunit;

namespace WebApi.Test.Candidate.GetById;

public class GetCandidateByIdTest : ApplicantTrackingClassFixture
{
    public GetCandidateByIdTest(CustomWebApplicationFactory factory) : base(factory) { }

    private readonly string METHOD = "candidates";

    [Fact]
    public async Task Success()
    {
        var response = await DoGet(method: $"{METHOD}/{_cadidateId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("id").GetInt32().Should().BeGreaterThan(0);
        responseData.RootElement.GetProperty("name").GetString().Should().Be(_cadidateName);
    }

    [Fact]
    public async Task Error_Candidate_Not_Found()
    {
        var response = await DoGet(method: $"{METHOD}/1000");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        errors.Should().ContainSingle().And.Contain(errors => errors.GetString()!.Equals(ResourceMessagesException.CANDIDATE_NOT_FOUND));
    }
}

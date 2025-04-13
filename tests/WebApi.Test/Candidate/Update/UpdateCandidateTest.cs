using System.Net;
using System.Text.Json;

using ApplicantTracking.Exceptions;

using CommonTestUtilities.Requests;

using FluentAssertions;

using Xunit;

namespace WebApi.Test.Candidate.Update;

public class UpdateCandidateTest : ApplicantTrackingClassFixture
{
    public UpdateCandidateTest(CustomWebApplicationFactory factory) : base(factory) { }

    private readonly string METHOD = "candidates";

    [Fact]
    public async Task Success()
    {
        var request = RequestCandidateJsonBuilder.Build();

        var response = await DoPut(method: $"{METHOD}/{_cadidateId}", request: request);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        response = await DoGet(method: $"{METHOD}/{_cadidateId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().Should().Be(request.Name);
        responseData.RootElement.GetProperty("id").GetInt32().Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Error_Candidate_Not_Found()
    {
        var request = RequestCandidateJsonBuilder.Build();

        var response = await DoPut(method: $"{METHOD}/{1000}", request: request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        errors.Should().ContainSingle().And.Contain(errors => errors.GetString()!.Equals(ResourceMessagesException.CANDIDATE_NOT_FOUND));
    }
}

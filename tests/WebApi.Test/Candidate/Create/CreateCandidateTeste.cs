using System.Net;
using System.Text.Json;

using ApplicantTracking.Exceptions;

using CommonTestUtilities.Requests;

using FluentAssertions;

using Xunit;

namespace WebApi.Test.Candidate.Create;

public class CreateCandidateTeste : ApplicantTrackingClassFixture
{
    public CreateCandidateTeste(CustomWebApplicationFactory factory) : base(factory) { }

    private readonly string METHOD = "candidates";

    [Fact]
    public async Task Success()
    {
        var request = RequestCandidateJsonBuilder.Build();

        var response = await DoPost(method: METHOD, request: request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().Should().Be(request.Name);
        responseData.RootElement.GetProperty("id").GetInt32().Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestCandidateJsonBuilder.Build();
        request.Name = string.Empty;

        var response = await DoPost(method: METHOD, request: request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        errors.Should().ContainSingle().And.Contain(errors => errors.GetString()!.Equals(ResourceMessagesException.NAME_EMPTY));
    }
}

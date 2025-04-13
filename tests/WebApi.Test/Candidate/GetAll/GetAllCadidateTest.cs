using System.Net;
using System.Text.Json;

using FluentAssertions;

using Xunit;

namespace WebApi.Test.Candidate.GetAll;

public class GetAllCadidateTest : ApplicantTrackingClassFixture
{
    public GetAllCadidateTest(CustomWebApplicationFactory factory) : base(factory) { }

    private readonly string METHOD = "candidates";

    [Fact]
    public async Task Success()
    {
        var response = await DoGet(method: METHOD);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("candidates").GetArrayLength().Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Success_No_Content()
    {
        await DoDelete(method: $"{METHOD}/{_cadidateId}");

        var response = await DoGet(method: METHOD);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}

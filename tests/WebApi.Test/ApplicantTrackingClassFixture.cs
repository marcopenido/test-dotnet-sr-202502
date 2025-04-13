using System.Net.Http.Headers;
using System.Net.Http.Json;

using ApplicantTracking.Domain.Extensions;

using Xunit;

namespace WebApi.Test;

public class ApplicantTrackingClassFixture : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    protected readonly int _cadidateId;
    protected readonly string _cadidateName;

    public ApplicantTrackingClassFixture(CustomWebApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();
        _cadidateId = factory.GetCandidadeId();
        _cadidateName = factory.GetCandidadeName();
    }

    protected async Task<HttpResponseMessage> DoPost(string method, object request, string token = "", string culture = "en")
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(token);

        return await _httpClient.PostAsJsonAsync(method, request);
    }

    protected async Task<HttpResponseMessage> DoGet(string method, string token = "", string culture = "en")
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(token);

        return await _httpClient.GetAsync(method);
    }

    protected async Task<HttpResponseMessage> DoPut(string method, object request, string token = "", string culture = "en")
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(token);

        return await _httpClient.PutAsJsonAsync(method, request);
    }

    protected async Task<HttpResponseMessage> DoDelete(string method, string token = "", string culture = "en")
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(token);

        return await _httpClient.DeleteAsync(method);
    }

    private void AuthorizeRequest(string token)
    {
        if (token.Empty())
            return;

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    private void ChangeRequestCulture(string culture)
    {
        if (_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
            _httpClient.DefaultRequestHeaders.Remove("Accept-Language");

        _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);
    }
}

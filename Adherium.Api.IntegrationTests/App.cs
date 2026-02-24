using System.Net.Http.Headers;
using Adherium.Api.Features.Auth.AuthenticateUser;
using Adherium.Infra;
using FastEndpoints;
using FastEndpoints.Testing;

namespace Adherium.Api.IntegrationTests;

public class App : AppFixture<Program>
{
    public async Task<HttpClient> CreateClientWithToken()
    {
        var request = new Request(SeedData.Patient.Email, SeedData.Patient.Password);
        var (_, result) = await Client.POSTAsync<Endpoint, Request, Response>(request);

        var client = CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result!.Token);

        return client;
    }
}
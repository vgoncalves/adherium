using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using Adherium.Api.Features.Auth.AuthenticateUser;
using Adherium.Infra;
using FastEndpoints;
using Shouldly;

namespace Adherium.Api.IntegrationTests.Features;

public class AuthenticateTests(App app) : IClassFixture<App>
{
    [Fact]
    public async Task Authenticate_ValidCredentials_ReturnsValidToken()
    {
        // Arrange 
        var request = new Request(SeedData.Patient.Email, SeedData.Patient.Password);

        // Act
        var (response, result) = await app.Client.POSTAsync<Endpoint, Request, Response>(request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        result.ShouldNotBeNull();
        result!.Token.ShouldNotBeNullOrEmpty();

        // Decode and validate JWT claims
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(result.Token);

        jwt.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value
            .ShouldBe(SeedData.Patient.Id.ToString());
        
        jwt.Claims.First(c => c.Type == ClaimTypes.Name).Value
            .ShouldBe(SeedData.Patient.Name);
        
        jwt.Claims.First(c => c.Type == ClaimTypes.Email).Value
            .ShouldBe(SeedData.Patient.Email);
    }

    [Fact]
    public async Task Authenticate_InvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange 
        var request = new Request(SeedData.Patient.Email, "WrongPassword");

        // Act
        var (response, _) = await app.Client.POSTAsync<Endpoint, Request, Response>(request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
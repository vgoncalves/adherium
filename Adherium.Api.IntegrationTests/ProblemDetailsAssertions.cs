using System.Text.Json;
using Shouldly;

namespace Adherium.Api.IntegrationTests;

public record ProblemDetailsResponse(
    int StatusCode,
    string Message,
    Dictionary<string, string[]> Errors);

public class ProblemDetailsAssertion(ProblemDetailsResponse problemDetails)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ProblemDetailsResponse Value => problemDetails;

    public static async Task<ProblemDetailsAssertion> FromContentAsync(
        HttpContent content,
        CancellationToken cancellationToken = default)
    {
        var json = await content.ReadAsStringAsync(cancellationToken);
        var details = JsonSerializer.Deserialize<ProblemDetailsResponse>(json, JsonOptions)
            ?? throw new InvalidOperationException("Failed to deserialize ProblemDetails response");
        return new ProblemDetailsAssertion(details);
    }

    public ProblemDetailsAssertion ShouldContainError(string errorMessage, string? property = null)
    {
        if (property is not null)
        {
            var matchingProperty = problemDetails.Errors.Keys
                .FirstOrDefault(k => k.Equals(property, StringComparison.OrdinalIgnoreCase));

            matchingProperty.ShouldNotBeNull($"Expected property '{property}' in errors, but found: [{string.Join(", ", problemDetails.Errors.Keys)}]");

            problemDetails.Errors[matchingProperty]
                .ShouldContain(e => e.Contains(errorMessage, StringComparison.OrdinalIgnoreCase),
                    $"Expected error message containing '{errorMessage}' for property '{property}', but found: [{string.Join(", ", problemDetails.Errors[matchingProperty])}]");
        }
        else
        {
            var allErrors = problemDetails.Errors.Values.SelectMany(e => e).ToList();
            allErrors.ShouldContain(e => e.Contains(errorMessage, StringComparison.OrdinalIgnoreCase),
                $"Expected error message containing '{errorMessage}', but found: [{string.Join(", ", allErrors)}]");
        }

        return this;
    }

    public ProblemDetailsAssertion ShouldHaveStatusCode(int statusCode)
    {
        problemDetails.StatusCode.ShouldBe(statusCode);
        return this;
    }

    public ProblemDetailsAssertion ShouldHaveErrorForProperty(string property)
    {
        var matchingProperty = problemDetails.Errors.Keys
            .FirstOrDefault(k => k.Equals(property, StringComparison.OrdinalIgnoreCase)
                              || k.Contains(property, StringComparison.OrdinalIgnoreCase));

        matchingProperty.ShouldNotBeNull($"Expected errors for property '{property}', but found: [{string.Join(", ", problemDetails.Errors.Keys)}]");
        return this;
    }
}

public static class ProblemDetailsExtensions
{
    public static Task<ProblemDetailsAssertion> AsProblemDetailsAsync(
        this HttpContent content,
        CancellationToken cancellationToken = default)
        => ProblemDetailsAssertion.FromContentAsync(content, cancellationToken);
}

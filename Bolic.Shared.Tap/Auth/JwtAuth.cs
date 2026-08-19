using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Bolic.Shared.Tap.Auth;

public static class JwtAuth
{
    public static Option<string> ExtractUserId(
        HttpRequestData request,
        TokenValidationParameters validationParameters,
        string userIdClaim = "sub")
    {
        if (!request.Headers.TryGetValues("Authorization", out var values))
            return Option<string>.None;

        var header = values.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(header) || !header.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            return Option<string>.None;

        var token = header["Bearer ".Length..].Trim();
        if (string.IsNullOrWhiteSpace(token))
            return Option<string>.None;

        var handler = new JsonWebTokenHandler();
        if (!handler.CanReadToken(token))
            return Option<string>.None;

        var result = handler.ValidateTokenAsync(token, validationParameters).GetAwaiter().GetResult();
        if (!result.IsValid)
            return Option<string>.None;

        return Optional(result.ClaimsIdentity?.FindFirst(userIdClaim)?.Value);
    }
}

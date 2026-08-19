using Bolic.Shared.Core;
using Bolic.Shared.Tap.Models;
using Microsoft.Azure.Functions.Worker.Http;
using Bolic.Shared.Core.Utils;

namespace Bolic.Shared.Tap;

public static class Tap
{
    public static Eff<Runtime, TapResult<T>> Process<T>(
        HttpRequestData request,
        Func<HttpRequestData, Option<string>>? userIdExtractor = null,
        JsonSerializerOptions? serializerSettings = null,
        Func<Stream, T>? action = null)
    {
        return liftEff<Runtime, TapResult<T>>(_ =>
        {
            var userId = userIdExtractor?.Invoke(request) ?? Option<string>.None;
            return userId.Match(
                Some: id => Right<Error, TapResult<T>>(ProcessAsync(request, serializerSettings, action, id)),
                None: () => Left<Error, TapResult<T>>(Error.New(new TapAuthException(
                    new TapError(401, "Unauthorized", "Missing or invalid authentication token")))));
        });
    }

    private static TapResult<T> ProcessAsync<T>(
        HttpRequestData request,
        JsonSerializerOptions? serializerSettings,
        Func<Stream, T>? action,
        Option<string> userId)
    {
        var body = action is null
            ? Utils.To<T>(request.Body, serializerSettings)
            : liftEff(() => action(request.Body));

        return new TapResult<T>(
            Method: request.Method,
            RequestUri: request.Url,
            Body: body,
            UserId: userId
        );
    }
}

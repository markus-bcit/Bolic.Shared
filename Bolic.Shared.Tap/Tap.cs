using Bolic.Shared.Core;
using Bolic.Shared.Tap.Models;
using Microsoft.Azure.Functions.Worker.Http;
using Bolic.Shared.Core.Utils;

namespace Bolic.Shared.Tap;

public static class Tap
{
    public static Eff<Runtime, TapResult<T>> Process<T>(HttpRequestData request,
        JsonSerializerOptions? serializerSettings = null,
        Func<Stream, T>? action = null)
    {
        return Eff<Runtime, TapResult<T>>.Lift(_ =>
            ProcessAsync(request, serializerSettings, action)
        );
    }

    private static TapResult<T> ProcessAsync<T>(
        HttpRequestData request,
        JsonSerializerOptions? serializerSettings = null,
        Func<Stream, T>? action = null)
    {
        var body = action is null
            ? Utils.To<T>(request.Body, serializerSettings)
            : liftEff(() => action(request.Body));

        return new TapResult<T>(
            Method: request.Method,
            RequestUri: request.Url,
            Body: body
        );
    }
}
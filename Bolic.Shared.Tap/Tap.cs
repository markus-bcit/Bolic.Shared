using Bolic.Shared.Core;
using Bolic.Shared.Tap.Models;
using Microsoft.Azure.Functions.Worker.Http;
using Bolic.Shared.Core.Utils;
using Newtonsoft.Json;

namespace Bolic.Shared.Tap;

public static class Tap
{
    public static Eff<Runtime, TapResult<T>> Process<T>(HttpRequestData request,
        JsonSerializerSettings? serializerSettings = null,
        Func<Stream, T>? action = null)
    {
        return LanguageExt.Eff<Runtime, TapResult<T>>.Lift(_ =>
            ProcessAsync(request, serializerSettings, action)
        );
    }

    private static TapResult<T> ProcessAsync<T>(
        HttpRequestData request,
        JsonSerializerSettings? serializerSettings = null,
        Func<Stream, T>? action = null)
    {
        Option<T> body;

        if (action is null)
        {
            body = (T)Utils.To<T>(request.Body, serializerSettings);
        }
        else
        {
            body = action(request.Body);
        }

        return new TapResult<T>(
            Method: request.Method,
            RequestUri: request.Url,
            Body: body
        );
    }
}
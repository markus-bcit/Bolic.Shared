using Bolic.Shared.Core;
using Bolic.Shared.Tap.Models;
using Microsoft.Azure.Functions.Worker.Http;
using Bolic.Shared.Core.Utils;

namespace Bolic.Shared.Tap;

public static class Tap
{
    public static Eff<Runtime, Task<TapResult<T>>> Process<T>(HttpRequestData request)
    {
        return LanguageExt.Eff<Runtime, Task<TapResult<T>>>.Lift(_ => 
            ProcessAsync<T>(request)
        );
    }

    private static async Task<TapResult<T>> ProcessAsync<T>(HttpRequestData request)
    {
        var body = await Utils.To<T>(request.Body);
        return new TapResult<T>(
            Method: request.Method,
            RequestUri: request.Url,
            Body: body
        );
    }
}
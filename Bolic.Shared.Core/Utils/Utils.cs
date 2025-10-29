using LanguageExt.Async;
using Newtonsoft.Json;


namespace Bolic.Shared.Core.Utils;

public static class Utils
{
    public static async Task<Option<string>> ToString(Stream stream)
    {
        using var reader = new StreamReader(stream);
        var body = await reader.ReadToEndAsync();
        return Optional(body).Filter(b => !string.IsNullOrWhiteSpace(b));
    }

    public static Option<T> To<T>(Stream stream, JsonSerializerSettings? serializerSettings = null)
    {
        using var reader = new StreamReader(stream);
        var body = Async.await(reader.ReadToEndAsync());
        var obj = JsonConvert.DeserializeObject<T>(body, serializerSettings);
        return obj;
    }
}
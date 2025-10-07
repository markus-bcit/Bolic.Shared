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

    public static async Task<Option<T>> To<T>(Stream stream)
    {
        using var reader = new StreamReader(stream);
        var body = await reader.ReadToEndAsync();

        return Optional(body)
            .Filter(b => !string.IsNullOrWhiteSpace(b))
            .Bind(b =>
            {
                try
                {
                    var json = JsonConvert.DeserializeObject<T>(b);
                    return Optional(json);
                }
                catch
                {
                    return Option<T>.None;
                }
            });
    }
}
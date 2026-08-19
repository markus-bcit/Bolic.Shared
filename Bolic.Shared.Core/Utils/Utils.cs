namespace Bolic.Shared.Core.Utils;

public static class Utils
{
    public static async Task<Option<string>> ToString(Stream stream)
    {
        using var reader = new StreamReader(stream);
        var body = await reader.ReadToEndAsync();
        return Optional(body).Filter(b => !string.IsNullOrWhiteSpace(b));
    }

    public static Eff<T> To<T>(Stream stream, JsonSerializerOptions? options = null) =>
        liftEff(async () =>
        {
            var obj = await JsonSerializer.DeserializeAsync<T>(stream, options);
            return obj ?? throw new JsonException($"Failed to deserialize {typeof(T).Name}");
        });
}
namespace Bolic.Shared.Tap.Models;

public record TapError(
    int StatusCode,
    string Title,
    string Detail,
    Exception? Exception = null,
    Map<string, object> Context = default)
{
}
namespace Bolic.Shared.Tap.Models;

public record TapResult<T>(
    string Method,
    Uri RequestUri,
    Option<T> Body//, ToDo Add back when completed
    // Map<string, string> Headers,
    // Map<string, string> QueryParameters,
    // Option<string> ContentType
);
using LanguageExt;

namespace Bolic.Shared.Database.Api;

public record CreateRequest<T>(
    string UserId,
    string Id,
    T Document,
    string Container,
    string Database
) where T : class;

public record ReadRequest(
    string UserId,
    string Id,
    string Container,
    string Database
);

public record UpdateRequest<T>(
    string UserId,
    string Id,
    T Document,
    string Container,
    string Database
) where T : class;

public record DeleteRequest(
    string UserId,
    string Id,
    string Container,
    string Database
);

public record QueryRequest(
    string UserId,
    string Id,
    string Query,
    string Container,
    string Database
);

public record CreateResponse<T>(
    T Document,
    string UserId,
    string Id
) where T : class;

public record ReadResponse<T>(
    T Document,
    string UserId
) where T : class;

public record UpdateResponse<T>(
    T Document,
    string UserId
) where T : class;

public record DeleteResponse(
    bool Success,
    string UserId
);

public record QueryResponse<T>(
    Seq<T> Documents,
    string UserId,
    string ContinuationToken = default
) where T : class;

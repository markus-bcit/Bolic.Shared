using LanguageExt;

namespace Bolic.Shared.Database.Api;

public record CreateRequest<T>(
    string UserId,
    string Id,
    T Document
) where T : class;

public record ReadRequest(
    string UserId,
    string Id
);

public record UpdateRequest<T>(
    string UserId,
    string Id,
    T Document
) where T : class;

public record DeleteRequest(
    string UserId,
    string Id
);

public record QueryRequest(
    string UserId,
    string Id,
    string Query
);

public record CreateResponse<T>(
    T Document,
    string Id,
    double RequestCharge
) where T : class;

public record ReadResponse<T>(
    T Document,
    double RequestCharge
) where T : class;

public record UpdateResponse<T>(
    T Document,
    double RequestCharge
) where T : class;

public record DeleteResponse(
    bool Success,
    double RequestCharge
);

public record QueryResponse<T>(
    Seq<T> Documents,
    double RequestCharge,
    Option<string> ContinuationToken = default
) where T : class;

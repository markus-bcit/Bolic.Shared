using LanguageExt;

namespace Bolic.Shared.Database.Api;

public record CreateRequest<T>(
    Option<string> UserId,
    Option<string> Id,
    T Document,
    Option<string> Container,
    Option<string> Database
) where T : class;

public record ReadRequest(
    Option<string> UserId,
    Option<string> Id,
    Option<string> Container,
    Option<string> Database
);

public record UpdateRequest<T>(
    Option<string> UserId,
    Option<string> Id,
    T Document,
    Option<string> Container,
    Option<string> Database
) where T : class;

public record DeleteRequest(
    Option<string> UserId,
    Option<string> Id,
    Option<string> Container,
    Option<string> Database
);

public record QueryRequest(
    Option<string> UserId,
    Option<string> Id,
    Option<string> Query,
    Option<string> Container,
    Option<string> Database
);

public record CreateResponse<T>(
    T Document,
    Option<string> UserId,
    Option<string> Id
) where T : class;

public record ReadResponse<T>(
    T Document,
    Option<string> UserId
) where T : class;

public record UpdateResponse<T>(
    T Document,
    Option<string> UserId
) where T : class;

public record DeleteResponse(
    bool Success,
    Option<string> UserId
);

public record QueryResponse<T>(
    Seq<T> Documents,
    Option<string> UserId,
    Option<string> ContinuationToken = default
) where T : class;

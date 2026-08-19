using Bolic.Shared.Core;
using Bolic.Shared.Database.Api;

namespace Bolic.Shared.Database;

public interface IDatabase
{
    static abstract Eff<Runtime, CreateResponse<T>> CreateItem<T>(CreateRequest<T> request)
        where T : class;

    static abstract Eff<Runtime, ReadResponse<T>> ReadItem<T>(ReadRequest request)
        where T : class;

    static abstract Eff<Runtime, UpdateResponse<T>> UpdateItem<T>(UpdateRequest<T> request)
        where T : class;

    static abstract Eff<Runtime, IAsyncEnumerable<T>> QueryItem<T>(QueryRequest request)
        where T : class;

    static abstract Eff<Runtime, QueryResponse<T>> QueryAll<T>(QueryRequest request)
        where T : class;

    static abstract Eff<Runtime, DeleteResponse<T>> DeleteItem<T>(CreateRequest<T> request)
        where T : class;

    static abstract Eff<Runtime, PatchResponse<T>> PatchItem<T>(PatchRequest<T> request)
        where T : class;

    static abstract Eff<Runtime, Unit> UpsertBatch<T>(UpsertBatchRequest<T> request)
        where T : class;
}
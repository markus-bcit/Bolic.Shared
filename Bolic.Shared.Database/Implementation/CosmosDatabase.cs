using Bolic.Shared.Core;
using Bolic.Shared.Database.Api;

namespace Bolic.Shared.Database.Implementation;

public class CosmosDatabase
{
    public static Eff<Runtime, CreateResponse<T>> CreateItem<T>(CreateRequest<T> request)
        where T : class =>
        liftEff<Runtime, CreateResponse<T>>(async runtime =>
        {
            var container = runtime.Cosmos.GetContainer(request.Database, request.Container);
            var response = await container.CreateItemAsync(request.Document, new PartitionKey(request.UserId));
            return new CreateResponse<T>(response.Resource, request.UserId, request.Id);
        });

    public static Eff<Runtime, ReadResponse<T>> ReadItem<T>(ReadRequest request)
        where T : class =>
        liftEff<Runtime, ReadResponse<T>>(async runtime =>
        {
            var container = runtime.Cosmos.GetContainer(request.Database, request.Container);
            var response = await container.ReadItemAsync<T>(request.Id, new PartitionKey(request.UserId));
            return new ReadResponse<T>(response.Resource, request.Id);
        });

    public static Eff<Runtime, UpdateResponse<T>> UpdateItem<T>(UpdateRequest<T> request)
        where T : class =>
        liftEff<Runtime, UpdateResponse<T>>(async runtime =>
        {
            var container = runtime.Cosmos.GetContainer(request.Database, request.Container);
            var response = await container.UpsertItemAsync(request.Document, new PartitionKey(request.UserId));
            return new UpdateResponse<T>(response.Resource, request.UserId);
        });

    public static Eff<Runtime, IAsyncEnumerable<T>> QueryItem<T>(QueryRequest request)
        where T : class =>
        lift<Runtime, IAsyncEnumerable<T>>(runtime =>
        {
            var container = runtime.Cosmos.GetContainer(request.Database, request.Container);
            var iterator = container.GetItemQueryIterator<T>(request.Query);
            return Enumerate(iterator);
        });

    public static Eff<Runtime, QueryResponse<T>> QueryAll<T>(QueryRequest request)
        where T : class =>
        liftEff<Runtime, QueryResponse<T>>(async runtime =>
        {
            var container = runtime.Cosmos.GetContainer(request.Database, request.Container);
            var iterator = container.GetItemQueryIterator<T>(request.Query);
            var items = new List<T>();
            while (iterator.HasMoreResults)
                foreach (var item in await iterator.ReadNextAsync())
                    items.Add(item);
            return new QueryResponse<T>(toSeq(items), request.UserId);
        });

    public static Eff<Runtime, DeleteResponse<T>> DeleteItem<T>(CreateRequest<T> request)
        where T : class =>
        liftEff<Runtime, DeleteResponse<T>>(async runtime =>
        {
            var container = runtime.Cosmos.GetContainer(request.Database, request.Container);
            var response = await container.DeleteItemAsync<T>(request.Id, new PartitionKey(request.UserId));
            return new DeleteResponse<T>(response.Resource, request.UserId, request.Id);
        });

    public static Eff<Runtime, PatchResponse<T>> PatchItem<T>(PatchRequest<T> request)
        where T : class =>
        liftEff<Runtime, PatchResponse<T>>(async runtime =>
        {
            var container = runtime.Cosmos.GetContainer(request.Database, request.Container);
            var response = await container.PatchItemAsync<T>(
                id: request.Id,
                partitionKey: new PartitionKey(request.UserId),
                patchOperations: request.Operations
            );
            return new PatchResponse<T>(response.Resource, request.UserId, request.Id);
        });

    private static async IAsyncEnumerable<T> Enumerate<T>(FeedIterator<T> iterator)
    {
        while (iterator.HasMoreResults)
            foreach (var item in await iterator.ReadNextAsync())
                yield return item;
    }
}
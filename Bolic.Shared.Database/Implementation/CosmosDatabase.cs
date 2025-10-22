using Bolic.Shared.Core;
using Bolic.Shared.Database.Api;
using LanguageExt.Async;

namespace Bolic.Shared.Database.Implementation;

public class CosmosDatabase
{
    public static Eff<Runtime, Either<Exception, CreateResponse<T>>> CreateItem<T>(CreateRequest<T> request)
        where T : class =>
        lift<Runtime, Either<Exception, CreateResponse<T>>>(runtime =>
            {
                try
                {
                    var container = runtime.Cosmos
                        .GetContainer(request.Database, request.Container);

                    var response = Async.await(container.CreateItemAsync(
                        request.Document,
                        new PartitionKey(request.UserId)
                    ));

                    return Right(new CreateResponse<T>(response.Resource, request.UserId,
                        request.Id));
                }
                catch (Exception ex)
                {
                    return Left(ex);
                }
            }
        );

    public static Eff<Runtime, Either<Exception, ReadResponse<T>>> ReadItem<T>(ReadRequest request)
        where T : class =>
        lift<Runtime, Either<Exception, ReadResponse<T>>>(runtime =>
            {
                try
                {
                    var container = runtime.Cosmos
                        .GetContainer(request.Database, request.Container);

                    var response = Async.await(container.ReadItemAsync<T>(
                        request.Id,
                        new PartitionKey(request.UserId)
                    ));
                    return Right(new ReadResponse<T>(response.Resource, request.Id));
                }
                catch (Exception ex)
                {
                    return Left(ex);
                }
            }
        );

    public static Eff<Runtime, Either<Exception, UpdateResponse<T>>> UpdateItem<T>(UpdateRequest<T> request)
        where T : class =>
        lift<Runtime, Either<Exception, UpdateResponse<T>>>(runtime =>
            {
                try
                {
                    var container = runtime.Cosmos
                        .GetContainer(request.Database, request.Container);

                    var response = Async.await(container.UpsertItemAsync((
                        request.Id,
                        new PartitionKey(request.UserId)
                    )));
                    return Right(new UpdateResponse<T>(request.Document, request.UserId));
                }
                catch (Exception ex)
                {
                    return Left(ex);
                }
            }
        );

    public static Eff<Runtime, Either<Exception, QueryResponse<T>>> QueryItem<T>(QueryRequest request)
        where T : class =>
        lift<Runtime, Either<Exception, QueryResponse<T>>>(runtime =>
            {
                try
                {
                    var container = runtime.Cosmos
                        .GetContainer(request.Database, request.Container);

                    var response = container.GetItemQueryIterator<T>(
                        request.Query
                    );

                    IEnumerable<T> EnumerateAsync() // ToDo not sure if this actually works as intended
                    {
                        while (response.HasMoreResults)
                        {
                            foreach (var item in Async.await(response.ReadNextAsync()))
                                yield return item;
                        }
                    }

                    return Right(new QueryResponse<T>(new Seq<T>(EnumerateAsync()), request.UserId));
                }
                catch (Exception ex)
                {
                    return Left(ex);
                }
            }
        );

    public static Eff<Runtime, Either<Exception, DeleteResponse<T>>> DeleteItem<T>(CreateRequest<T> request)
        where T : class =>
        lift<Runtime, Either<Exception, DeleteResponse<T>>>(runtime =>
            {
                try
                {
                    var container = runtime.Cosmos
                        .GetContainer(request.Database, request.Container);

                    var response = Async.await(container.DeleteItemAsync<T>(request.Id,
                        new PartitionKey(request.UserId)
                    ));

                    return Right(new DeleteResponse<T>(response.Resource, request.UserId,
                        request.Id));
                }
                catch (Exception ex)
                {
                    return Left(ex);
                }
            }
        );
}
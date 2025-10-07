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
                        .GetContainer(request.Database.ToString(), request.Container.ToString());

                    var response = Async.await(container.CreateItemAsync(
                        request.Document,
                        new PartitionKey(request.UserId.ToString())
                    ));

                    return Right(new CreateResponse<T>(response.Resource, request.UserId.ToString(),
                        request.Id.ToString()));
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
                        .GetContainer(request.Database.ToString(), request.Container.ToString());

                    var response = Async.await(container.ReadItemAsync<T>(
                        request.Id.ToString(),
                        new PartitionKey(request.UserId.ToString())
                    ));
                    return Right(new ReadResponse<T>(response.Resource, request.Id.ToString()));
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
                        .GetContainer(request.Database.ToString(), request.Container.ToString());

                    var response = Async.await(container.UpsertItemAsync((
                        request.Id.ToString(),
                        new PartitionKey(request.UserId.ToString())
                    )));
                    return Right(new UpdateResponse<T>(request.Document, request.UserId.ToString()));
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
                        .GetContainer(request.Database.ToString(), request.Container.ToString());

                    var response = container.GetItemQueryIterator<T>(
                        request.Query.ToString()
                    );

                    IEnumerable<T> EnumerateAsync() // ToDo not sure if this actually works as intended
                    {
                        while (response.HasMoreResults)
                        {
                            foreach (var item in Async.await(response.ReadNextAsync()))
                                yield return item;
                        }
                    }

                    return Right(new QueryResponse<T>(new Seq<T>(EnumerateAsync()), request.UserId.ToString()));
                }
                catch (Exception ex)
                {
                    return Left(ex);
                }
            }
        );
}
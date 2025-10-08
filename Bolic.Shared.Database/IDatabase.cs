using Bolic.Shared.Core;
using Bolic.Shared.Database.Api;

namespace Bolic.Shared.Database;

public interface IDatabase
{
    Eff<Runtime, Either<Exception, CreateResponse<T>>> CreateItem<T>(CreateRequest<T> request) 
        where T : class;
    
    Eff<Runtime, Either<Exception, ReadResponse<T>>> ReadItem<T>(ReadRequest request) 
        where T : class;
    
    Eff<Runtime, Either<Exception, UpdateResponse<T>>> UpdateItem<T>(UpdateRequest<T> request) 
        where T : class;
    
    Eff<Runtime, Either<Exception, DeleteResponse>> DeleteItem(DeleteRequest request);
    
    Eff<Runtime, Either<Exception, QueryResponse<T>>> QueryItem<T>(QueryRequest request) 
        where T : class;
}
using Bolic.Shared.Core;
using Bolic.Shared.Database.Api;

namespace Bolic.Shared.Database;

public interface IDatabase
{
    Eff<Runtime, Either<DatabaseError, CreateResponse<T>>> Create<T>(CreateRequest<T> request) 
        where T : class;
    
    // Eff<Runtime, Either<DatabaseError, ReadResponse<T>>> Read<T>(ReadRequest request) 
    //     where T : class;
    //
    // Eff<Runtime, Either<DatabaseError, UpdateResponse<T>>> Update<T>(UpdateRequest<T> request) 
    //     where T : class;
    //
    // Eff<Runtime, Either<DatabaseError, DeleteResponse>> Delete(DeleteRequest request);
    //
    // Eff<Runtime, Either<DatabaseError, QueryResponse<T>>> Query<T>(QueryRequest request) 
    //     where T : class;
}
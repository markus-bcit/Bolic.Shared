using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Bolic.Shared.Core;

public interface IRuntime
{
    CosmosClient Cosmos { get; }
    ILogger<Runtime> Logger { get; }
}
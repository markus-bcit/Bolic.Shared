using Azure.Identity;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Bolic.Shared.Core;

public class Runtime :
    IRuntime,
    Has<Eff<Runtime>, CosmosClient>,
    Has<Eff<Runtime>, ILogger<Runtime>>
{
    public CosmosClient Cosmos { get; }
    public ILogger<Runtime> Logger { get; }


    public Runtime(ILogger<Runtime>? logger = null)
    {
        var cosmosUrl = Environment.GetEnvironmentVariable("CosmosConnection")
                        ?? throw new InvalidOperationException("Missing CosmosConnection");

        Cosmos = new CosmosClient(cosmosUrl, new DefaultAzureCredential());
        Logger = logger ?? new NullLogger<Runtime>();
    }

    static K<Eff<Runtime>, CosmosClient> Has<Eff<Runtime>, CosmosClient>.Ask =>
        Readable.asks<Eff<Runtime>, Runtime, CosmosClient>(rt => rt.Cosmos);

    static K<Eff<Runtime>, ILogger<Runtime>> Has<Eff<Runtime>, ILogger<Runtime>>.Ask =>
        Readable.asks<Eff<Runtime>, Runtime, ILogger<Runtime>>(rt => rt.Logger);
}
using Microsoft.Azure.Functions.Worker.Http;

namespace Bolic.Shared.Tap;

public class Tap : ITap,
    Has<Eff<Tap>, HttpRequestData>
{

}
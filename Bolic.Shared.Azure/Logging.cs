// using Bolic.Shared.Core;
// using Microsoft.Extensions.Logging;
//
// namespace Bolic.Shared.Azure;
//
// public static class Log<RT>
//     where RT : struct, HasLogging<RT>
// {
//     public static Eff<RT, Unit> Info(string message) =>
//         default(RT).LogEff.Map(logger =>
//         {
//             logger.LogInformation(message);
//             return unit;
//         });
//
//     public static Eff<RT, Unit> Error(string message) =>
//         default(RT).LogEff.Map(logger =>
//         {
//             logger.LogError(message);
//             return unit;
//         });
// }
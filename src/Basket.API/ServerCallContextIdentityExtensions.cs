using System.Security.Claims;
using Grpc.Core;

namespace Basket.API;

public static class ServerCallContextIdentityExtensions
{
    public static string? GetUserIdentity(this ServerCallContext context)
    {
        var user = context.GetHttpContext().User;
        foreach (var claim in user.Claims)
        {
            Console.WriteLine($"{claim.Type} = {claim.Value}");
        }
        return context.GetHttpContext().User.FindFirst("sub")?.Value;
    }

    public static string? GetUserName(this ServerCallContext context) =>
        context.GetHttpContext()
            .User
            .FindFirst(x => x.Type == ClaimTypes.Name)?.Value;
}
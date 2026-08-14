namespace Ordering.API.Infrastructure.Services;

public interface IIdentityService
{
    string GetUserIdentity();

    string GetUserName();
}

public class IdentityService(IHttpContextAccessor context) : IIdentityService
{
    public string GetUserIdentity()
    {
        return context.HttpContext?.User.FindFirst("sub")?.Value;
    }

    public string GetUserName()
    {
        return context.HttpContext?.User?.Identity?.Name;
    }
}
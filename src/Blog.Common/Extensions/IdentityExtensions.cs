using Blog.Common.Constants;
using System.Security.Claims;
using System.Security.Principal;

namespace Blog.Common.Extensions;
public static class IdentityExtensions
{
    private static string FindFirstValue(this ClaimsIdentity identity, string claimType)
        => identity?.FindFirst(claimType).Value;

    public static int GetUserId(this IIdentity identity) => int.Parse((identity as ClaimsIdentity).FindFirstValue(ClaimTypes.NameIdentifier));
    public static string GetUserName(this IIdentity identity) => (identity as ClaimsIdentity).FindFirstValue(ClaimTypes.Name);
    public static string GetDisplayName(this IIdentity identity) => (identity as ClaimsIdentity).FindFirstValue(IdentityClaimNames.DisplayName);
    public static string GetAvatar(this IIdentity identity) => (identity as ClaimsIdentity).FindFirstValue(IdentityClaimNames.Avatar);
    public static List<string> GetRoles(this IIdentity identity) => (identity as ClaimsIdentity).Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
}

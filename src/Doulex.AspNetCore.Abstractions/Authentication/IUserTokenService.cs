using System.Security.Claims;

namespace Doulex.AspNetCore.Authentication;

/// <summary>
/// Try to get the user id from the claims
/// </summary>
public interface IUserTokenService
{
    /// <summary>
    /// Attempt to read the token of the logged-in user
    /// </summary>
    /// <param name="identity"></param>
    /// <returns></returns>
    UserToken GetToken(ClaimsIdentity identity);
}

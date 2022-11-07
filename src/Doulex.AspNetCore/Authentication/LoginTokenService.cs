using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Doulex.AspNetCore.Authentication;

/// <summary>
/// User Authentication Service
/// </summary>
public class LoginTokenService : ILoginTokenService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserTokenService    _userTokenService;

    public LoginTokenService(
        IHttpContextAccessor httpContextAccessor,
        IUserTokenService    userTokenService)
    {
        _httpContextAccessor = httpContextAccessor;
        _userTokenService    = userTokenService;
    }

    /// <summary>
    /// Attempt to read the token of the logged-in user
    /// </summary>
    /// <param name="cancel"></param>
    /// <returns></returns>
    public async Task<UserToken?> GetTokenAsync(CancellationToken cancel = default)
    {
        if (_httpContextAccessor.HttpContext == null)
            return null;

        var auth = await _httpContextAccessor.HttpContext.AuthenticateAsync(); //获取登录用户的AuthenticateResult
        if (!auth.Succeeded)
            return null;

        // Return authentication object
        var identity = auth.Principal.Identity as ClaimsIdentity ?? throw new AuthenticateException($"不支持的认证 Identity 类型 {auth.Principal.Identity?.GetType().Name}");

        return _userTokenService.GetToken(identity);
    }

    /// <summary>
    /// Whether the user has logged in to the system
    /// </summary>
    /// <returns></returns>
    public async Task<bool> IsLoggedInAsync(CancellationToken cancel = default)
    {
        if (_httpContextAccessor.HttpContext == null)
            return false;

        var auth = await _httpContextAccessor.HttpContext.AuthenticateAsync();
        return auth.Succeeded;
    }
}

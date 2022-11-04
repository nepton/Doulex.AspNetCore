namespace Doulex.AspNetCore.Authentication;

/// <summary>
/// Save the login user object interface
/// </summary>
public interface ILoginTokenPersistence
{
    /// <summary>
    /// In pipeline of asp.net, after user authenticated, we will save the logged in user to cache and database
    /// </summary>
    /// <param name="loginToken"></param>
    /// <param name="cancel"></param>
    /// <returns></returns>
    Task SaveLoginUserAsync(LoginToken loginToken, CancellationToken cancel = default);
}

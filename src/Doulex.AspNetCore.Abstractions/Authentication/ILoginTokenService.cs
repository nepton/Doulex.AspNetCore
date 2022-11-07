namespace Doulex.AspNetCore.Authentication;

/// <summary>
/// 登录的账户主体服务, 查询当前登录账户的信息, 通常, 基于 AccessToken 认证的登录账户主体为 Claims 的集合
/// </summary>
public interface ILoginTokenService
{
    /// <summary>
    /// 尝试读取登录用户的 token
    /// </summary>
    /// <param name="cancel"></param>
    /// <returns></returns>
    Task<UserToken?> GetTokenAsync(CancellationToken cancel = default);

    /// <summary>
    /// 当前是否已登录
    /// </summary>
    /// <returns></returns>
    Task<bool> IsLoggedInAsync(CancellationToken cancel = default);

    /// <summary>
    /// 获取登录者的手机号
    /// </summary>
    /// <param name="cancel"></param>
    /// <returns></returns>
    public async Task<string?> GetPhoneNumberAsync(CancellationToken cancel = default)
    {
        var loginToken = await GetTokenAsync(cancel);
        return loginToken?.PhoneNumber;
    }

    /// <summary>
    /// 获取当前账户主体的Id
    /// </summary>
    /// <returns></returns>
    public async Task<string?> GetIdAsync(CancellationToken cancel = default)
    {
        var loginToken = await GetTokenAsync(cancel);
        return loginToken?.Id;
    }

    /// <summary>
    /// 确保已登录
    /// </summary>
    async Task EnsureLoggedInAsync(CancellationToken cancel = default)
    {
        if (await IsLoggedInAsync(cancel) == false)
            throw new AuthenticateException();
    }
}

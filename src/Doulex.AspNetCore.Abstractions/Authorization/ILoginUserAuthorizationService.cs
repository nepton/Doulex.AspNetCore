namespace Doulex.AspNetCore.Authorization;

/// <summary>
/// 判断登录用户的权限的接口
/// </summary>
public interface ILoginUserAuthorizationService
{
    /// <summary>
    /// 确保当前登录的用户有权利执行指定权限的操作
    /// </summary>
    /// <param name="scope">权限要求的作用域</param>
    /// <param name="apiPermissionDemand">如果权限要求为null, 则仅判断用户是否可以登录即可</param>
    /// <param name="cancel"></param>
    /// <returns></returns>
    Task EnsureUserHasAuthorizedAsync(string scope, string? apiPermissionDemand, CancellationToken cancel = default);
}

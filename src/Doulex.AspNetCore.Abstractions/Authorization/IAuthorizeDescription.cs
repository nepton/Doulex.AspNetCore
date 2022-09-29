namespace Doulex.AspNetCore.Authorization;

/// <summary>
/// 认证授权的描述信息
/// </summary>
public interface IAuthorizeDescription
{
    /// <summary>
    /// 授权的描述信息 (For Dev)
    /// </summary>
    public string Description { get; }
}
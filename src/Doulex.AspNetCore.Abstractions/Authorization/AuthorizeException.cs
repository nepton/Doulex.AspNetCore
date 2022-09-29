namespace Doulex.AspNetCore.Authorization;

/// <summary>
/// 登录用户缺少指定的权限，将出现此异常
/// </summary>
public class AuthorizeException : Exception
{
    public AuthorizeException()
    {
    }

    public AuthorizeException(string message) : base(message)
    {
    }
}
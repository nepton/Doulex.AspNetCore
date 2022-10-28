namespace Doulex.AspNetCore.Authorization;

/// <summary>
/// 没有找到登录用户，将出现此异常
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
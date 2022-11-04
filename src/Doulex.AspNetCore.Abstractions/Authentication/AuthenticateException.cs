namespace Doulex.AspNetCore.Authentication;

/// <summary>
/// User authentication exception
/// Indicates that the user is not authenticated 401
/// </summary>
public class AuthenticateException : Exception
{
    public AuthenticateException()
    {
    }

    public AuthenticateException(string message) : base(message)
    {
    }
}
namespace Doulex.AspNetCore.ExceptionResponse;

/// <summary>
/// 内容返回接口
/// </summary>
public interface IResponseContent
{
    /// <summary>
    /// 返回的状态码
    /// </summary>
    int StatusCode { get; }

    /// <summary>
    /// 返回的内容类型
    /// </summary>
    string ContentType { get; }

    /// <summary>
    /// 返回的内容(暂时定义成字符串)
    /// </summary>
    string Content { get; }
}
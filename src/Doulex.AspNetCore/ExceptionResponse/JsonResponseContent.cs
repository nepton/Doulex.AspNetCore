using Newtonsoft.Json;

namespace Doulex.AspNetCore.ExceptionResponse;

/// <summary>
/// 返回JSON对象类型
/// </summary>
public class JsonResponseContent : IResponseContent
{
    /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
    public JsonResponseContent(int statusCode, object? content)
    {
        StatusCode = statusCode;
        ContentType = "text/json";
        Content = "{}";

        if (content != null)
        {
            ContentType = "text/json";
            Content = JsonConvert.SerializeObject(content);
        }
    }

    /// <summary>
    /// 返回的状态码
    /// </summary>
    public int StatusCode { get; }

    /// <summary>
    /// 返回的内容类型
    /// </summary>
    public string ContentType { get; }

    /// <summary>
    /// 返回的内容(暂时定义成字符串)
    /// </summary>
    public string Content { get; }
}

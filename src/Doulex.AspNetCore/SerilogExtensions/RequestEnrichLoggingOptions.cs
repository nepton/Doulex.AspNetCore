namespace Doulex.AspNetCore.SerilogExtensions
{
    /// <summary>
    /// 请求内容记录选项
    /// </summary>
    public class RequestEnrichLoggingOptions
    {
        public int RequestBodyMaxLength { get; set; } = 2048;

        public int ResponseBodyMaxLength { get; set; } = 2048;

        public HttpFields LoggingFields { get; set; } = HttpFields.All;

        /// <summary>
        /// 包含基本的请求头信息
        /// </summary>
        public bool IncludeCommonHeaders { get; set; } = true;

        /// <summary>
        /// 包含指定的请求头
        /// </summary>
        public HashSet<string>? RequestHeaders { get; set; } = null;

        private static readonly HashSet<string> _commonRequestHeaders = new()
        {
            "Content-Type",
            "Content-Length",
            "Authorization",
            "User-Agent",
            "Accept",
            "Accept-Language",
            "Accept-Encoding",
            "Referer",
        };

        internal bool IncludeRequestHeaders(string name)
        {
            if (IncludeCommonHeaders && _commonRequestHeaders.Contains(name))
                return true;

            if (RequestHeaders?.Contains(name) == true)
                return true;

            return false;
        }

        /// <summary>
        /// 包含指定的回应头
        /// </summary>
        public HashSet<string>? ResponseHeaders { get; set; } = null;

        private static readonly HashSet<string> _commonResponseHeaders = new()
        {
            "Content-Type",
            "Content-Length",
        };

        internal bool IncludeResponseHeaders(string name)
        {
            if (IncludeCommonHeaders && _commonResponseHeaders.Contains(name))
                return true;

            if (ResponseHeaders?.Contains(name) == true)
                return true;

            return false;
        }
    }
}

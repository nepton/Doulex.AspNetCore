using Serilog.Events;

namespace Doulex.AspNetCore.Logger.RequestEnrichLogging
{
    /// <summary>
    /// 请求内容记录选项
    /// </summary>
    public class RequestEnrichLoggingOptions
    {
        /// <summary>
        /// 有记录日志的需要
        /// </summary>
        internal bool LoggingRequired => LogWhenException || LogWhenSuccess || LogWhenClientError || LogWhenServerError;

        /// <summary>
        /// 日志的等级
        /// </summary>
        public LogEventLevel Level { get; set; } = LogEventLevel.Debug;

        /// <summary>
        /// 出现异常的时候记录
        /// </summary>
        public bool LogWhenException { get; set; } = true;

        /// <summary>
        /// 执行成功的时候记录 StatusCode less that 400
        /// </summary>
        public bool LogWhenSuccess { get; set; } = true;

        /// <summary>
        /// 客户端失败或者错误时候记录 4xx
        /// </summary>
        public bool LogWhenClientError { get; set; } = true;

        /// <summary>
        /// 服务器失败或者错误时候记录 5xx
        /// </summary>
        public bool LogWhenServerError { get; set; } = true;
    }
}

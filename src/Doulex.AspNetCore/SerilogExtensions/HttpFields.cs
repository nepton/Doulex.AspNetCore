﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpLogging;

namespace Doulex.AspNetCore.SerilogExtensions
{
    /// <summary>
    /// Flags used to control which parts of the
    /// request and response are logged.
    /// </summary>
    [Flags]
    public enum HttpFields : long
    {
        /// <summary>
        /// No logging.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// Flag for logging the HTTP Request Path, which includes both the <see cref="HttpRequest.Path"/>
        /// and <see cref="HttpRequest.PathBase"/>.
        /// <p>
        /// For example:
        /// Path: /index
        /// PathBase: /app
        /// </p>
        /// </summary>
        RequestPath = 0x1,

        /// <summary>
        /// Flag for logging the HTTP Request <see cref="HttpRequest.QueryString"/>.
        /// <p>
        /// For example:
        /// Query: ?index=1
        /// </p>
        /// RequestQuery contents can contain private information
        /// which may have regulatory concerns under GDPR
        /// and other laws. RequestQuery should not be logged
        /// unless logs are secure and access controlled
        /// and the privacy impact assessed.
        /// </summary>
        RequestQuery = 0x2,

        /// <summary>
        /// Flag for logging the HTTP Request <see cref="HttpRequest.Protocol"/>.
        /// <p>
        /// For example:
        /// Protocol: HTTP/1.1
        /// </p>
        /// </summary>
        RequestProtocol = 0x4,

        /// <summary>
        /// Flag for logging the HTTP Request <see cref="HttpRequest.Method"/>.
        /// <p>
        /// For example:
        /// Method: GET
        /// </p>
        /// </summary>
        RequestMethod = 0x8,

        /// <summary>
        /// Flag for logging the HTTP Request <see cref="HttpRequest.Scheme"/>.
        /// <p>
        /// For example:
        /// Scheme: https
        /// </p>
        /// </summary>
        RequestScheme = 0x10,

        /// <summary>
        /// Flag for logging the HTTP Request <see cref="HttpRequest.Headers"/>.
        /// Request Headers are logged as soon as the middleware is invoked.
        /// Headers are redacted by default with the character '[Redacted]' unless specified in
        /// the <see cref="HttpLoggingOptions.RequestHeaders"/>.
        /// <p>
        /// For example:
        /// Connection: keep-alive
        /// My-Custom-Request-Header: [Redacted]
        /// </p>
        /// </summary>
        RequestHeaders = 0x20,

        /// <summary>
        /// Flag for logging the HTTP Request <see cref="HttpRequest.Body"/>.
        /// Logging the request body has performance implications, as it requires buffering
        /// the entire request body up to <see cref="HttpLoggingOptions.RequestBodyLogLimit"/>.
        /// </summary>
        RequestBody = 0x40,

        /// <summary>
        /// The request user <see cref="HttpContext.User"/>.
        /// </summary>
        RequestUser = 0x80,

        /// <summary>
        /// Flag for logging the HTTP Response <see cref="HttpResponse.Headers"/>.
        /// Response Headers are logged when the <see cref="HttpResponse.Body"/> is written to
        /// or when <see cref="IHttpResponseBodyFeature.StartAsync(System.Threading.CancellationToken)"/>
        /// is called.
        /// Headers are redacted by default with the character '[Redacted]' unless specified in
        /// the <see cref="HttpLoggingOptions.ResponseHeaders"/>.
        /// <p>
        /// For example:
        /// Content-Length: 16
        /// My-Custom-Response-Header: [Redacted]
        /// </p>
        /// </summary>
        ResponseHeaders = 0x200,

        /// <summary>
        /// Flag for logging the HTTP Response <see cref="HttpResponse.Body"/>.
        /// Logging the response body has performance implications, as it requires buffering
        /// the entire response body up to <see cref="HttpLoggingOptions.ResponseBodyLogLimit"/>.
        /// </summary>
        ResponseBody = 0x400,

        /// <summary>
        /// Flag for logging the entire HTTP Request.
        /// Logging the request body has performance implications, as it requires buffering
        /// the entire request body up to <see cref="HttpLoggingOptions.RequestBodyLogLimit"/>.
        /// </summary>
        Request = RequestPath | RequestProtocol | RequestMethod | RequestScheme | RequestHeaders | RequestBody | RequestQuery | RequestUser,

        /// <summary>
        /// Flag for logging the entire HTTP Response.
        /// Logging the response body has performance implications, as it requires buffering
        /// the entire response body up to <see cref="HttpLoggingOptions.ResponseBodyLogLimit"/>.
        /// </summary>
        Response = ResponseHeaders | ResponseBody,

        /// <summary>
        /// Flag for logging both the HTTP Request and Response.
        /// Includes <see cref="Request"/> and <see cref="Response"/>.
        /// Logging the request and response body has performance implications, as it requires buffering
        /// the entire request and response body up to the <see cref="HttpLoggingOptions.RequestBodyLogLimit"/>
        /// and <see cref="HttpLoggingOptions.ResponseBodyLogLimit"/>.
        /// </summary>
        All = Request | Response,
    }
}

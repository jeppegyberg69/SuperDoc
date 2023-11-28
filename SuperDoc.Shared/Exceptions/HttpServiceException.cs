using System.Net;

namespace SuperDoc.Shared.Exceptions;

/// <summary>
/// Represents an exception specific to HTTP service operations.
/// </summary>
public class HttpServiceException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpServiceException"/> class.
    /// </summary>
    /// <param name="content">The HTTP content associated with the exception.</param>
    /// <param name="statusCode">The HTTP status code associated with the exception.</param>
    public HttpServiceException(HttpContent? content, HttpStatusCode? statusCode)
    {
        Content = content;
        StatusCode = statusCode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpServiceException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that describes the exception.</param>
    /// <param name="content">The HTTP content associated with the exception.</param>
    /// <param name="statusCode">The HTTP status code associated with the exception.</param>
    public HttpServiceException(string message, HttpContent? content, HttpStatusCode? statusCode) : base(message)
    {
        Content = content;
        StatusCode = statusCode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpServiceException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that describes the exception.</param>
    /// <param name="content">The HTTP content associated with the exception.</param>
    /// <param name="statusCode">The HTTP status code associated with the exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception.</param>
    public HttpServiceException(string message, HttpContent? content, HttpStatusCode? statusCode, Exception inner) : base(message, inner)
    {
        Content = content;
        StatusCode = statusCode;
    }

    /// <summary>
    /// Gets the HTTP content associated with the exception.
    /// </summary>
    public HttpContent? Content { get; private set; }

    /// <summary>
    /// Gets the HTTP status code associated with the exception.
    /// </summary>
    public HttpStatusCode? StatusCode { get; private set; }
}

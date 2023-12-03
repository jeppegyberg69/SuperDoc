using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace SuperDoc.Shared.Exceptions;

/// <summary>
/// Represents an exception specific to HTTP service operations.
/// </summary>
public class HttpServiceException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpServiceException"/> class with a specified error message and a optional reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that describes the exception.</param>
    /// <param name="content">The HTTP content associated with the exception.</param>
    /// <param name="statusCode">The HTTP status code associated with the exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception.</param>
    public HttpServiceException(string message, HttpContent? content, HttpStatusCode? statusCode, Exception? inner = null) : base(message, inner)
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

    /// <summary>
    /// Throws an exception for an unexpected error during an HTTP request.
    /// </summary>
    /// <param name="content">The content of the HTTP response.</param>
    /// <param name="statusCode">The status code of the HTTP response.</param>
    /// <param name="inner">An optional inner exception.</param>
    [DoesNotReturn] // Tell the compiler that this method won't return after the method has executed.
    public static void ThrowUnexpectedError(HttpContent? content, HttpStatusCode? statusCode, Exception? inner = default)
    {
        throw new HttpServiceException("An unexpected error occurred during HTTP request.", content, statusCode, inner);
    }

    /// <summary>
    /// Throws an exception for an invalid status code in the HTTP response.
    /// </summary>
    /// <param name="content">The content of the HTTP response.</param>
    /// <param name="statusCode">The status code of the HTTP response.</param>
    /// <param name="inner">An optional inner exception.</param>
    [DoesNotReturn] // Tell the compiler that this method won't return after the method has executed.
    public static void ThrowInvalidStatusCode(HttpContent? content, HttpStatusCode? statusCode, Exception? inner = default)
    {
        throw new HttpServiceException("The attempted HTTP request was not successful.", content, statusCode, inner);
    }
}

using System;
using System.Collections.Generic;

namespace FrenCircle.Contracts.Shared
{
    /// <summary>
    /// Standard API response wrapper for consistent client-server communication.
    /// </summary>
    /// <typeparam name="T">The type of the response data.</typeparam>
    public class ApiResponse<T>(int status, string message, T? data, IEnumerable<string>? hints = null, long? responseTimeMs = null)
    {
        /// <summary>
        /// Status code representing the result of the request.
        /// </summary>
        public int Status { get; set; } = status;

        /// <summary>
        /// Descriptive message about the response.
        /// </summary>
        public string Message { get; set; } = message ?? string.Empty;

        /// <summary>
        /// Actual payload/data returned by the API.
        /// </summary>
        public T? Data { get; set; } = data;

        /// <summary>
        /// Optional hints, suggestions, or extra info for the client.
        /// </summary>
        public IReadOnlyList<string> Hints { get; set; } = hints is null ? Array.Empty<string>() : new List<string>(hints);

        /// <summary>
        /// Optional server response time in milliseconds.
        /// </summary>
        public long? ResponseTimeMs { get; set; } = responseTimeMs;
    }
}

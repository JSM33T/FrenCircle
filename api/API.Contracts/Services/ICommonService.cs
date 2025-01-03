namespace API.Contracts.Services
{
    /// <summary>
    /// Provides common functionality related to user context.
    /// </summary>
    public interface ICommonService
    {
        /// <summary>
        /// Retrieves the username of the currently authenticated user from the HTTP context accessor.
        /// </summary>
        /// <returns>The username of the current user.</returns>
        public string? GetUsername();

        /// <summary>
        /// Retrieves the unique identifier (ID) of the currently authenticated user from the HTTP context accessor.
        /// </summary>
        /// <returns>The user ID of the current user.</returns>
        public int? GetUserId();
    }
}

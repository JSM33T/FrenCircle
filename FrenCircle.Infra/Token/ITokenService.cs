namespace FrenCircle.Infra.Token
{
    /// <summary>
    /// Provides functionality to generate access and refresh tokens for authentication.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Generates a new access and refresh token pair for the given user ID.
        /// </summary>
        /// <param name="userId">The user ID for which tokens are generated.</param>
        /// <returns>
        /// A tuple containing the access token, refresh token, token expiry time, and issuance time.
        /// </returns>
        (string AccessToken, string RefreshToken, DateTime ExpiresAt, DateTime IssuedAt) GenerateTokens(int userId);
    }
}
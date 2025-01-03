namespace API.Contracts.Services
{
    public interface ICryptoService
    {
        /// <summary>
        /// Hashes a plain-text password asynchronously using a cryptographic algorithm.
        /// </summary>
        /// <param name="plainPassword">The plain-text password to be hashed.</param>
        /// <returns>A task representing the asynchronous operation, with a string result containing the hashed password.</returns>
        public Task<string> HashPassword(string plainPassword);

        /// <summary>
        /// Verifies if a given plain-text password matches a hashed password.
        /// </summary>
        /// <param name="plainPassword">The plain-text password to verify.</param>
        /// <param name="hashedPassword">The hashed password to compare against.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating whether the passwords match.</returns>
        public Task<bool> VerifyPassword(string plainPassword, string hashedPassword);

    }
}

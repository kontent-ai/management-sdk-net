namespace Kentico.Kontent.Management.Models.Shared
{
    /// <summary>
    /// Represents identifier of users.
    /// </summary>
    public sealed class UserIdentifier
    {
        private UserIdentifier() { }

        /// <summary>
        /// Gets the id of the identifier.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the email of the identifier.
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// Creates the reference by id.
        /// </summary>
        /// <param name="id">The id of the identifier.</param>
        public static UserIdentifier ById(string id) => new() { Id = id };

        /// <summary>
        /// Creates the user identifier by email.
        /// </summary>
        /// <param name="email">The email of the identifier.</param>
        public static UserIdentifier ByEmail(string email) => new() { Email = email };
    }
}

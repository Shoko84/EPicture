namespace epicture
{
    /// <summary>
    /// Defines public informations of an user
    /// </summary>
    public class UserInfos
    {
        /// <summary>
        /// The full name of the user
        /// </summary>
        public string FullName;
        /// <summary>
        /// The username of the user
        /// </summary>
        public string Username;
        /// <summary>
        /// The user identifier of the user
        /// </summary>
        public string UserId;

        /// <summary>
        /// Constructor of the class <see cref="UserInfos"/>
        /// </summary>
        /// <param name="fullName">The full name of the user</param>
        /// <param name="username">The username of the user</param>
        /// <param name="userId">The user identifier of the user</param>
        public UserInfos(string fullName, string username, string userId)
        {
            FullName = fullName;
            Username = username;
            UserId = userId;
        }
    }
}

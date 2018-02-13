using System;

namespace epicture
{
    /// <summary>
    /// Custom exception about misc user authentification errors
    /// </summary>
    public class UserAuthenticationException : Exception
    {
        /// <summary>
        /// Constructor of the class <see cref="UserAuthenticationException"/>
        /// </summary>
        /// <param name="message">The description of the exception</param>
        public UserAuthenticationException(string message) : base(message)
        {

        }
    }
}

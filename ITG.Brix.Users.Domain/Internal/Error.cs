using System;

namespace ITG.Brix.Users.Domain.Internal
{
    /// <summary>
    ///     Error class for creating and unwrapping <see cref="Exception" /> instances.
    /// </summary>
    internal static class Error
    {
        /// <summary>
        ///     Creates an <see cref="ArgumentNullException" /> with the provided properties.
        /// </summary>
        /// <param name="messageFormat">A composite format string explaining the reason for the exception.</param>
        /// <param name="messageArgs">An object array that contains zero or more objects to format.</param>
        /// <returns>The logged <see cref="Exception" />.</returns>
        internal static ArgumentNullException ArgumentNull(string messageFormat, params object[] messageArgs)
        {
            return new ArgumentNullException(string.Format(messageFormat, messageArgs));
        }

        /// <summary>
        ///     Creates an <see cref="ArgumentException" /> with the provided properties.
        /// </summary>
        /// <param name="messageFormat">A composite format string explaining the reason for the exception.</param>
        /// <param name="messageArgs">An object array that contains zero or more objects to format.</param>
        /// <returns>The logged <see cref="Exception" />.</returns>
        internal static ArgumentException Argument(string messageFormat, params object[] messageArgs)
        {
            return new ArgumentException(string.Format(messageFormat, messageArgs));
        }
    }
}

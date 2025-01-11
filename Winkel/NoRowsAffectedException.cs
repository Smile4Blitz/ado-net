using System;

namespace Winkel
{
    [Serializable]
    internal class NoRowsAffectedException : Exception
    {
        public NoRowsAffectedException()
        {
        }

        public NoRowsAffectedException(string? message) : base($"{message}: no rows affected.")
        {
        }

        public NoRowsAffectedException(string? message, Exception? innerException) : base($"{message}: no rows affected.", innerException)
        {
        }
    }
}
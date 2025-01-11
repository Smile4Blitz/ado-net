namespace Winkel
{
    [Serializable]
    internal class NoQueryException : Exception
    {
        public NoQueryException()
        {
        }

        public NoQueryException(string? message) : base($"{message}: no rows affected.")
        {
        }

        public NoQueryException(string? message, Exception? innerException) : base($"{message}: no rows affected.", innerException)
        {
        }
    }
}
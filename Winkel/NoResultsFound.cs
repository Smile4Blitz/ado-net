namespace Winkel
{
    [Serializable]
    internal class NoResultsFound : Exception
    {
        public NoResultsFound()
        {
        }

        public NoResultsFound(string? message) : base($"{message}: no results found.")
        {
        }

        public NoResultsFound(string? message, Exception? innerException) : base($"{message}: no results found.", innerException)
        {
        }
    }
}
namespace FiveSQD.WebVerse.Handlers.Javascript.APIs.Utilities
{
    /// <summary>
    /// Class for logging.
    /// </summary>
    public class Logging
    {
        /// <summary>
        /// Type for a log message.
        /// </summary>
        public enum Type { Default, Debug, Warning, Error };

        /// <summary>
        /// Log a message.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="type">Type of the message.</param>
        public static void Log(string message, Type type = Type.Default)
        {
            WebVerse.Utilities.Logging.Type tp = WebVerse.Utilities.Logging.Type.Default;
            switch (type)
            {
                case Type.Default:
                    break;

                case Type.Debug:
                    break;

                case Type.Warning:
                    break;

                case Type.Error:
                    break;
            }

            WebVerse.Utilities.Logging.Log(message, tp);
        }

        /// <summary>
        /// Log a debug message.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public static void LogDebug(string message)
        {
            WebVerse.Utilities.Logging.LogDebug(message);
        }

        /// <summary>
        /// Log a warning message.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public static void LogWarning(string message)
        {
            WebVerse.Utilities.Logging.LogWarning(message);
        }

        /// <summary>
        /// Log an error message.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public static void LogError(string message)
        {
            WebVerse.Utilities.Logging.LogError(message);
        }
    }
}
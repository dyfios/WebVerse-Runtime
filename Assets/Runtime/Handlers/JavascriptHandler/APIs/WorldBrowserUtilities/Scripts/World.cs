// Copyright (c) 2019-2024 Five Squared Interactive. All rights reserved.

using FiveSQD.WebVerse.Runtime;

namespace FiveSQD.WebVerse.Handlers.Javascript.APIs.Utilities
{
    /// <summary>
    /// Class for World utilities.
    /// </summary>
    public class World
    {
        /// <summary>
        /// Get a URL Query Parameter.
        /// </summary>
        /// <param name="key">Key of the Query Parameter.</param>
        /// <returns>The value of the Query Parameter, or null.</returns>
        public static string GetQueryParam(string key)
        {
            return WebVerseRuntime.Instance.worldEngine.GetParam(key);
        }

        /// <summary>
        /// Get the current World Load State.
        /// </summary>
        /// <returns>One of: unloaded, loadingworld, loadedworld, webpage, error.</returns>
        public static string GetWorldLoadState()
        {
            switch (WebVerseRuntime.Instance.state)
            {
                case WebVerseRuntime.RuntimeState.Unloaded:
                    return "unloaded";

                case WebVerseRuntime.RuntimeState.LoadingWorld:
                    return "loadingworld";

                case WebVerseRuntime.RuntimeState.LoadedWorld:
                    return "loadedworld";

                case WebVerseRuntime.RuntimeState.WebPage:
                    return "webpage";

                case WebVerseRuntime.RuntimeState.Error:
                default:
                    return "error";
            }
        }
    }
}
// Copyright (c) 2019-2023 Five Squared Interactive. All rights reserved.

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
    }
}
// Copyright (c) 2019-2024 Five Squared Interactive. All rights reserved.

using FiveSQD.WebVerse.Runtime;
using System;
using System.Collections.Generic;

namespace FiveSQD.WebVerse.Utilities
{
    /// <summary>
    /// Class for the Time Handler.
    /// </summary>
    public class TimeHandler : BaseHandler
    {
        /// <summary>
        /// Class for a function that is being called at a given interval.
        /// </summary>
        private class IntervalFunction
        {
            /// <summary>
            /// Function name.
            /// </summary>
            public string name { get; private set; }

            /// <summary>
            /// Current elapsed time since last invocation.
            /// </summary>
            public float currentElapsed = 0f;

            /// <summary>
            /// Interval to invoke at.
            /// </summary>
            public float interval { get; private set; }

            /// <summary>
            /// Constructor for an interval function.
            /// </summary>
            /// <param name="functionName">Function name.</param>
            /// <param name="interval">Interval to invoke at.</param>
            public IntervalFunction(string functionName, float interval)
            {
                name = functionName;
                this.interval = interval;
            }
        }

        /// <summary>
        /// Interval functions.
        /// </summary>
        private Dictionary<Guid, IntervalFunction> intervalFunctions;

        /// <summary>
        /// Initialize the time handler.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            intervalFunctions = new Dictionary<Guid, IntervalFunction>();
        }

        /// <summary>
        /// Start invoking a function.
        /// </summary>
        /// <param name="function">Function.</param>
        /// <param name="interval">Interval to invoke at.</param>
        /// <returns>ID for the function.</returns>
        public Guid StartInvoking(string function, float interval)
        {
            Guid newGuid = Guid.NewGuid();
            intervalFunctions.Add(newGuid, new IntervalFunction(function, interval));
            return newGuid;
        }

        /// <summary>
        /// Stop invoking a function.
        /// </summary>
        /// <param name="id">ID of the function.</param>
        public void StopInvoking(Guid id)
        {
            if (!intervalFunctions.ContainsKey(id))
            {
                Logging.LogWarning("[TimeHandler->StopInvoking] Unknown ID.");
                return;
            }

            intervalFunctions.Remove(id);
        }

        private void Update()
        {
            float elapsedTime = UnityEngine.Time.deltaTime;
            foreach (IntervalFunction intervalFunction in intervalFunctions.Values)
            {
                intervalFunction.currentElapsed += elapsedTime;
                if (intervalFunction.currentElapsed > intervalFunction.interval)
                {
                    WebVerseRuntime.Instance.javascriptHandler.RunScript(intervalFunction.name);
                    intervalFunction.currentElapsed = 0;
                }
            }
        }
    }
}
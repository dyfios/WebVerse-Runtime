using System;
using FiveSQD.WebVerse.WorldEngine.Utilities;
using FiveSQD.WebVerse.Handlers.Javascript.APIs.WorldTypes;
using FiveSQD.WebVerse.Runtime;

namespace FiveSQD.WebVerse.Handlers.Javascript.APIs.Utilities
{
    public class Time
    {
        public static UUID SetInterval(string function, float interval)
        {
            Guid id = WebVerseRuntime.Instance.timeHandler.StartInvoking(function, interval);
            if (id == null)
            {
                LogSystem.LogError("[Time->SetInterval] Unable to assign ID.");
                return null;
            }
            return new UUID(id.ToString());
        }

        public static bool StopInterval(string id)
        {
            Guid uuid = Guid.Parse(id);
            if (uuid == null)
            {
                LogSystem.LogWarning("[Time->StopInterval] Invalid ID.");
                return false;
            }
            WebVerseRuntime.Instance.timeHandler.StopInvoking(uuid);
            return true;
        }
    }
}
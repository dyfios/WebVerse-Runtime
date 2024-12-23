using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FiveSQD.WebVerse.Runtime;
using Jint.Native;

namespace FiveSQD.WebVerse.Handlers.Javascript.APIs.Data
{
    public class AsyncJSON
    {
        public static void Parse(string rawText, string onComplete)
        {Debug.Log("here");
            object result = JsonUtility.FromJson(rawText, typeof(JsObject));Debug.Log(result);
            WebVerseRuntime.Instance.javascriptHandler.CallWithParams(onComplete, new object[] { result });
        }

        public static void Stringify()
        {

        }
    }
}
using FiveSQD.WebVerse.Runtime;
using UnityEngine;

namespace FiveSQD.WebVerse.Handlers.Javascript.APIs.Input
{
    public class Input
    {
        public static Vector2 GetMoveValue()
        {
            return WebVerseRuntime.Instance.inputManager.moveValue;
        }

        public static Vector2 GetLookValue()
        {
            return WebVerseRuntime.Instance.inputManager.lookValue;
        }

        public static bool GetKeyValue(string key)
        {
            return WebVerseRuntime.Instance.inputManager.pressedKeys.Contains(key);
        }

        public static bool GetKeyCodeValue(string key)
        {
            return WebVerseRuntime.Instance.inputManager.pressedKeyCodes.Contains(key);
        }

        public static bool GetLeft()
        {
            return WebVerseRuntime.Instance.inputManager.leftValue;
        }

        public static bool GetMiddle()
        {
            return WebVerseRuntime.Instance.inputManager.middleValue;
        }

        public static bool GetRight()
        {
            return WebVerseRuntime.Instance.inputManager.rightValue;
        }
    }
}
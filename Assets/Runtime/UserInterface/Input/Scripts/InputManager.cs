using FiveSQD.WebVerse.Runtime;
using FiveSQD.WebVerse.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace FiveSQD.WebVerse.Input
{
    public class InputManager : BaseManager
    {
        public Vector2 moveValue;
        public Vector2 lookValue;
        public List<string> pressedKeys;
        public List<string> pressedKeyCodes;
        public bool leftValue;
        public bool middleValue;
        public bool rightValue;

        private List<string> moveFunctions;
        private List<string> endMoveFunctions;
        private List<string> lookFunctions;
        private List<string> endLookFunctions;
        private List<string> keyFunctions;
        private List<string> keyCodeFunctions;
        private List<string> endKeyFunctions;
        private List<string> endKeyCodeFunctions;
        private List<string> leftFunctions;
        private List<string> endLeftFunctions;
        private List<string> middleFunctions;
        private List<string> endMiddleFunctions;
        private List<string> rightFunctions;
        private List<string> endRightFunctions;

        public override void Initialize()
        {
            moveFunctions = new List<string>();
            endMoveFunctions = new List<string>();
            lookFunctions = new List<string>();
            endLookFunctions = new List<string>();
            keyFunctions = new List<string>();
            keyCodeFunctions = new List<string>();
            endKeyFunctions = new List<string>();
            endKeyCodeFunctions = new List<string>();
            pressedKeys = new List<string>();
            pressedKeyCodes = new List<string>();
            leftFunctions = new List<string>();
            endLeftFunctions = new List<string>();
            middleFunctions = new List<string>();
            endMiddleFunctions = new List<string>();
            rightFunctions = new List<string>();
            endRightFunctions = new List<string>();

            base.Initialize();
        }

        public override void Terminate()
        {
            base.Terminate();
        }

        public void RegisterInputEvent(string eventName, string call)
        {
            switch (eventName.ToLower())
            {
                case "move":
                    moveFunctions.Add(call);
                    break;

                case "endmove":
                    endMoveFunctions.Add(call);
                    break;

                case "look":
                    lookFunctions.Add(call);
                    break;

                case "endlook":
                    endLookFunctions.Add(call);
                    break;

                case "key":
                    keyFunctions.Add(call);
                    break;

                case "endkey":
                    endKeyFunctions.Add(call);
                    break;

                case "keycode":
                    keyCodeFunctions.Add(call);
                    break;

                case "endkeycode":
                    endKeyCodeFunctions.Add(call);
                    break;

                case "left":
                    leftFunctions.Add(call);
                    break;

                case "endleft":
                    endLeftFunctions.Add(call);
                    break;

                case "middle":
                    middleFunctions.Add(call);
                    break;

                case "endmiddle":
                    endMiddleFunctions.Add(call);
                    break;

                case "right":
                    rightFunctions.Add(call);
                    break;

                case "endright":
                    endRightFunctions.Add(call);
                    break;
            }
        }

        public void DeregisterInputEvent(string eventName, string call)
        {
            switch (eventName.ToLower())
            {
                case "move":
                    if (!moveFunctions.Contains(call))
                    {
                        Logging.LogWarning("[InputManager->DeregisterInputEvent] Event " + call + " does not exist.");
                        break;
                    }
                    moveFunctions.Remove(call);
                    break;

                case "endmove":
                    if (!endMoveFunctions.Contains(call))
                    {
                        Logging.LogWarning("[InputManager->DeregisterInputEvent] Event " + call + " does not exist.");
                        break;
                    }
                    endMoveFunctions.Remove(call);
                    break;

                case "look":
                    if (!lookFunctions.Contains(call))
                    {
                        Logging.LogWarning("[InputManager->DeregisterInputEvent] Event " + call + " does not exist.");
                        break;
                    }
                    lookFunctions.Remove(call);
                    break;

                case "endlook":
                    if (!endLookFunctions.Contains(call))
                    {
                        Logging.LogWarning("[InputManager->DeregisterInputEvent] Event " + call + " does not exist.");
                        break;
                    }
                    endLookFunctions.Remove(call);
                    break;

                case "key":
                    if (!keyFunctions.Contains(call))
                    {
                        Logging.LogWarning("[InputManager->DeregisterInputEvent] Event " + call + " does not exist.");
                        break;
                    }
                    keyFunctions.Remove(call);
                    break;

                case "endkey":
                    if (!endKeyFunctions.Contains(call))
                    {
                        Logging.LogWarning("[InputManager->DeregisterInputEvent] Event " + call + " does not exist.");
                        break;
                    }
                    endKeyFunctions.Remove(call);
                    break;

                case "keycode":
                    if (!keyCodeFunctions.Contains(call))
                    {
                        Logging.LogWarning("[InputManager->DeregisterInputEvent] Event " + call + " does not exist.");
                        break;
                    }
                    keyCodeFunctions.Remove(call);
                    break;

                case "endkeycode":
                    if (!endKeyCodeFunctions.Contains(call))
                    {
                        Logging.LogWarning("[InputManager->DeregisterInputEvent] Event " + call + " does not exist.");
                        break;
                    }
                    endKeyCodeFunctions.Remove(call);
                    break;

                case "left":
                    if (!leftFunctions.Contains(call))
                    {
                        Logging.LogWarning("[InputManager->DeregisterInputEvent] Event " + call + " does not exist.");
                        break;
                    }
                    leftFunctions.Remove(call);
                    break;

                case "endleft":
                    if (!endLeftFunctions.Contains(call))
                    {
                        Logging.LogWarning("[InputManager->DeregisterInputEvent] Event " + call + " does not exist.");
                        break;
                    }
                    endLeftFunctions.Remove(call);
                    break;

                case "middle":
                    if (!middleFunctions.Contains(call))
                    {
                        Logging.LogWarning("[InputManager->DeregisterInputEvent] Event " + call + " does not exist.");
                        break;
                    }
                    middleFunctions.Remove(call);
                    break;

                case "endmiddle":
                    if (!endMiddleFunctions.Contains(call))
                    {
                        Logging.LogWarning("[InputManager->DeregisterInputEvent] Event " + call + " does not exist.");
                        break;
                    }
                    endMiddleFunctions.Remove(call);
                    break;

                case "right":
                    if (!rightFunctions.Contains(call))
                    {
                        Logging.LogWarning("[InputManager->DeregisterInputEvent] Event " + call + " does not exist.");
                        break;
                    }
                    rightFunctions.Remove(call);
                    break;

                case "endright":
                    if (!endRightFunctions.Contains(call))
                    {
                        Logging.LogWarning("[InputManager->DeregisterInputEvent] Event " + call + " does not exist.");
                        break;
                    }
                    endRightFunctions.Remove(call);
                    break;
            }
        }

        public void Move(Vector2 amount)
        {
            foreach (string function in moveFunctions)
            {
                WebVerseRuntime.Instance.javascriptHandler.Run(function.Replace("?", amount.x + ", " + amount.y));
            }
        }

        public void EndMove()
        {
            foreach (string function in endMoveFunctions)
            {
                WebVerseRuntime.Instance.javascriptHandler.Run(function);
            }
        }

        public void Look(Vector2 amount)
        {
            foreach (string function in lookFunctions)
            {
                WebVerseRuntime.Instance.javascriptHandler.Run(function.Replace("?", amount.x + ", " + amount.y));
            }
        }

        public void EndLook()
        {
            foreach (string function in endLookFunctions)
            {
                WebVerseRuntime.Instance.javascriptHandler.Run(function);
            }
        }

        public void Key(string key, string keyCode)
        {
            foreach (string function in keyFunctions)
            {
                WebVerseRuntime.Instance.javascriptHandler.Run(function.Replace("?", key));
            }

            foreach (string function in keyCodeFunctions)
            {
                WebVerseRuntime.Instance.javascriptHandler.Run(function.Replace("?", keyCode));
            }
        }

        public void EndKey(string key, string keyCode)
        {
            foreach (string function in endKeyFunctions)
            {
                WebVerseRuntime.Instance.javascriptHandler.Run(function.Replace("?", key));
            }

            foreach (string function in endKeyCodeFunctions)
            {
                WebVerseRuntime.Instance.javascriptHandler.Run(function.Replace("?", keyCode));
            }
        }

        public void Left()
        {
            foreach (string function in leftFunctions)
            {
                WebVerseRuntime.Instance.javascriptHandler.Run(function);
            }
        }

        public void EndLeft()
        {
            foreach (string function in endLeftFunctions)
            {
                WebVerseRuntime.Instance.javascriptHandler.Run(function);
            }
        }

        public void Middle()
        {
            foreach (string function in middleFunctions)
            {
                WebVerseRuntime.Instance.javascriptHandler.Run(function);
            }
        }

        public void EndMiddle()
        {
            foreach (string function in endMiddleFunctions)
            {
                WebVerseRuntime.Instance.javascriptHandler.Run(function);
            }
        }

        public void Right()
        {
            foreach (string function in rightFunctions)
            {
                WebVerseRuntime.Instance.javascriptHandler.Run(function);
            }
        }

        public void EndRight()
        {
            foreach (string function in endRightFunctions)
            {
                WebVerseRuntime.Instance.javascriptHandler.Run(function);
            }
        }
    }
}
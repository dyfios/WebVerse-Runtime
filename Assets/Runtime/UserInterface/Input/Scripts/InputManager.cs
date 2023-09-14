// Copyright (c) 2019-2023 Five Squared Interactive. All rights reserved.

using FiveSQD.WebVerse.Runtime;
using FiveSQD.WebVerse.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace FiveSQD.WebVerse.Input
{
    /// <summary>
    /// Class for the Input Manager.
    /// </summary>
    public class InputManager : BaseManager
    {
        /// <summary>
        /// The move value.
        /// </summary>
        public Vector2 moveValue;

        /// <summary>
        /// The look value.
        /// </summary>
        public Vector2 lookValue;

        /// <summary>
        /// Pressed keys.
        /// </summary>
        public List<string> pressedKeys;

        /// <summary>
        /// Keycodes of pressed keys.
        /// </summary>
        public List<string> pressedKeyCodes;

        /// <summary>
        /// The left value.
        /// </summary>
        public bool leftValue;

        /// <summary>
        /// The middle value.
        /// </summary>
        public bool middleValue;

        /// <summary>
        /// The right value.
        /// </summary>
        public bool rightValue;

        /// <summary>
        /// Move functions.
        /// </summary>
        private List<string> moveFunctions;

        /// <summary>
        /// End Move functions.
        /// </summary>
        private List<string> endMoveFunctions;

        /// <summary>
        /// Look functions.
        /// </summary>
        private List<string> lookFunctions;

        /// <summary>
        /// End Look functions.
        /// </summary>
        private List<string> endLookFunctions;

        /// <summary>
        /// Key functions.
        /// </summary>
        private List<string> keyFunctions;

        /// <summary>
        /// Key Code functions.
        /// </summary>
        private List<string> keyCodeFunctions;

        /// <summary>
        /// End Key functions.
        /// </summary>
        private List<string> endKeyFunctions;

        /// <summary>
        /// End Key Code functions.
        /// </summary>
        private List<string> endKeyCodeFunctions;

        /// <summary>
        /// Left functions.
        /// </summary>
        private List<string> leftFunctions;

        /// <summary>
        /// End Left functions.
        /// </summary>
        private List<string> endLeftFunctions;

        /// <summary>
        /// Middle functions.
        /// </summary>
        private List<string> middleFunctions;

        /// <summary>
        /// End Middle functions.
        /// </summary>
        private List<string> endMiddleFunctions;

        /// <summary>
        /// Right functions.
        /// </summary>
        private List<string> rightFunctions;

        /// <summary>
        /// End Right functions.
        /// </summary>
        private List<string> endRightFunctions;

        /// <summary>
        /// Initialize the Input Manager.
        /// </summary>
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

        /// <summary>
        /// Terminate the Input Manager.
        /// </summary>
        public override void Terminate()
        {
            base.Terminate();
        }

        /// <summary>
        /// Register an Input Event.
        /// </summary>
        /// <param name="eventName">Name of the Input Event.</param>
        /// <param name="call">Logic to call on invocation of Input Event.</param>
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

        /// <summary>
        /// Deregister an Input Event.
        /// </summary>
        /// <param name="eventName">Name of the Input Event.</param>
        /// <param name="call">Logic being called on invocation of Input Event.</param>
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

        /// <summary>
        /// Perform a move.
        /// </summary>
        /// <param name="amount">Amount on X and Y axes to move.</param>
        public void Move(Vector2 amount)
        {
            foreach (string function in moveFunctions)
            {
                WebVerseRuntime.Instance.javascriptHandler.Run(function.Replace("?", amount.x + ", " + amount.y));
            }
        }

        /// <summary>
        /// End a move.
        /// </summary>
        public void EndMove()
        {
            foreach (string function in endMoveFunctions)
            {
                WebVerseRuntime.Instance.javascriptHandler.Run(function);
            }
        }

        /// <summary>
        /// Perform a look.
        /// </summary>
        /// <param name="amount">Amount on X and Y axes to look.</param>
        public void Look(Vector2 amount)
        {
            foreach (string function in lookFunctions)
            {
                WebVerseRuntime.Instance.javascriptHandler.Run(function.Replace("?", amount.x + ", " + amount.y));
            }
        }

        /// <summary>
        /// End a look.
        /// </summary>
        public void EndLook()
        {
            foreach (string function in endLookFunctions)
            {
                WebVerseRuntime.Instance.javascriptHandler.Run(function);
            }
        }

        /// <summary>
        /// Perform a key press.
        /// </summary>
        /// <param name="key">Key being pressed.</param>
        /// <param name="keyCode">Keycode of key being pressed.</param>
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

        /// <summary>
        /// End a key press.
        /// </summary>
        /// <param name="key">Key no longer being pressed.</param>
        /// <param name="keyCode">Keycode of key no longer being pressed.</param>
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

        /// <summary>
        /// Perform a left.
        /// </summary>
        public void Left()
        {
            foreach (string function in leftFunctions)
            {
                WebVerseRuntime.Instance.javascriptHandler.Run(function);
            }
        }

        /// <summary>
        /// End a left.
        /// </summary>
        public void EndLeft()
        {
            foreach (string function in endLeftFunctions)
            {
                WebVerseRuntime.Instance.javascriptHandler.Run(function);
            }
        }

        /// <summary>
        /// Perform a middle.
        /// </summary>
        public void Middle()
        {
            foreach (string function in middleFunctions)
            {
                WebVerseRuntime.Instance.javascriptHandler.Run(function);
            }
        }

        /// <summary>
        /// End a middle.
        /// </summary>
        public void EndMiddle()
        {
            foreach (string function in endMiddleFunctions)
            {
                WebVerseRuntime.Instance.javascriptHandler.Run(function);
            }
        }

        /// <summary>
        /// Perform a right.
        /// </summary>
        public void Right()
        {
            foreach (string function in rightFunctions)
            {
                WebVerseRuntime.Instance.javascriptHandler.Run(function);
            }
        }

        /// <summary>
        /// End a right.
        /// </summary>
        public void EndRight()
        {
            foreach (string function in endRightFunctions)
            {
                WebVerseRuntime.Instance.javascriptHandler.Run(function);
            }
        }
    }
}
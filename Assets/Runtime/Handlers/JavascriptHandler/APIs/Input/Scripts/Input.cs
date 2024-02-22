// Copyright (c) 2019-2024 Five Squared Interactive. All rights reserved.

using FiveSQD.WebVerse.Handlers.Javascript.APIs.Entity;
using FiveSQD.WebVerse.Handlers.Javascript.APIs.WorldTypes;
using FiveSQD.WebVerse.Runtime;

namespace FiveSQD.WebVerse.Handlers.Javascript.APIs.Input
{
    /// <summary>
    /// Input methods.
    /// </summary>
    public class Input
    {
        /// <summary>
        /// Get the current move value.
        /// </summary>
        /// <returns>A Vector2 representation of the current move value.</returns>
        public static Vector2 GetMoveValue()
        {
            UnityEngine.Vector2 moveValue = WebVerseRuntime.Instance.inputManager.moveValue;
            return new Vector2(moveValue.x, moveValue.y);
        }

        /// <summary>
        /// Get the current look value.
        /// </summary>
        /// <returns>A Vector2 representation of the current look value.</returns>
        public static Vector2 GetLookValue()
        {
            UnityEngine.Vector2 lookValue = WebVerseRuntime.Instance.inputManager.lookValue;
            return new Vector2(lookValue.x, lookValue.y);
        }

        /// <summary>
        /// Get the current pressed state of a key.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>Whether or not the key is pressed.</returns>
        public static bool GetKeyValue(string key)
        {
            return WebVerseRuntime.Instance.inputManager.pressedKeys.Contains(key);
        }

        /// <summary>
        /// Get the current pressed state of a key.
        /// </summary>
        /// <param name="keycode">The keycode to check.</param>
        /// <returns>Whether or not the key is pressed.</returns>
        public static bool GetKeyCodeValue(string keycode)
        {
            return WebVerseRuntime.Instance.inputManager.pressedKeyCodes.Contains(keycode);
        }

        /// <summary>
        /// Get the current pressed state of the left mouse button.
        /// </summary>
        /// <returns>Whether or not the left mouse button is pressed.</returns>
        public static bool GetLeft()
        {
            return WebVerseRuntime.Instance.inputManager.leftValue;
        }

        /// <summary>
        /// Get the current pressed state of the middle mouse button.
        /// </summary>
        /// <returns>Whether or not the middle mouse button is pressed.</returns>
        public static bool GetMiddle()
        {
            return WebVerseRuntime.Instance.inputManager.middleValue;
        }

        /// <summary>
        /// Get the current pressed state of the right mouse button.
        /// </summary>
        /// <returns>Whether or not the right mouse button is pressed.</returns>
        public static bool GetRight()
        {
            return WebVerseRuntime.Instance.inputManager.rightValue;
        }

        /// <summary>
        /// Get a raycast from the pointer.
        /// </summary>
        /// <param name="direction">Direction to cast the ray in.</param>
        /// <param name="pointerIndex">Index of the pointer to get raycast from.</param>
        /// <returns>A raycast from the pointer, or null.</returns>
        public static RaycastHitInfo? GetPointerRaycast(Vector3 direction, int pointerIndex = 0)
        {
            System.Tuple<UnityEngine.RaycastHit, UnityEngine.Vector3> hitInfo
                = WebVerseRuntime.Instance.inputManager.GetPointerRaycast(
                    new UnityEngine.Vector3(direction.x, direction.y, direction.z),
                    pointerIndex);

            if (hitInfo != null)
            {
                WorldEngine.Entity.BaseEntity hitEntity = null;
                if (hitEntity = hitInfo.Item1.collider.GetComponentInParent<WorldEngine.Entity.BaseEntity>())
                {
                    BaseEntity hitPublicEntity = EntityAPIHelper.GetPublicEntity(hitEntity);
                    if (hitPublicEntity != null)
                    {
                        return new RaycastHitInfo()
                        {
                            entity = hitPublicEntity,
                            hitPoint = new Vector3(hitInfo.Item1.point.x, hitInfo.Item1.point.y, hitInfo.Item1.point.z),
                            hitPointNormal = new Vector3(hitInfo.Item1.normal.x, hitInfo.Item1.normal.y, hitInfo.Item1.normal.z),
                            origin = new Vector3(hitInfo.Item2.x, hitInfo.Item2.y, hitInfo.Item2.z)
                        };
                    }
                }
            }

            return null;
        }
    }
}
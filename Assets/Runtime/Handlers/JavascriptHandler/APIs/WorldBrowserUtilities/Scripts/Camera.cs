// Copyright (c) 2019-2023 Five Squared Interactive. All rights reserved.

using FiveSQD.WebVerse.Handlers.Javascript.APIs.Entity;
using UnityEngine;

namespace FiveSQD.WebVerse.Handlers.Javascript.APIs.Utilities
{
    /// <summary>
    /// Class for camera manipulation.
    /// </summary>
    public class Camera
    {
        /// <summary>
        /// Attach camera to an entity.
        /// </summary>
        /// <param name="entityToAttachTo">Entity to attach camera to, or null if to make root.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public static bool AttachToEntity(BaseEntity entityToAttachTo)
        {
            if (entityToAttachTo == null)
            {
                WorldEngine.WorldEngine.ActiveWorld.cameraManager.SetParent(null);
            }
            else
            {
                entityToAttachTo.PlaceCameraOn();
            }

            return true;
        }

        /// <summary>
        /// Set position of the camera.
        /// </summary>
        /// <param name="position">Position to apply to camera.</param>
        /// <param name="local">Whether or not the position is local.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public static bool SetPosition(Vector3 position, bool local)
        {
            WorldEngine.WorldEngine.ActiveWorld.cameraManager.SetPosition(position, local);

            return true;
        }

        /// <summary>
        /// Set the rotation of the camera.
        /// </summary>
        /// <param name="rotation">Rotation to apply to camera.</param>
        /// <param name="local">Whether or not the rotation is local.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public static bool SetRotation(Quaternion rotation, bool local)
        {
            WorldEngine.WorldEngine.ActiveWorld.cameraManager.SetRotation(rotation, local);

            return true;
        }

        /// <summary>
        /// Set the Euler rotation of the camera.
        /// </summary>
        /// <param name="rotation">Euler rotation to apply to camera.</param>
        /// <param name="local">Whether or not the Euler rotation is local.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public static bool SetEulerRotation(Vector3 rotation, bool local)
        {
            WorldEngine.WorldEngine.ActiveWorld.cameraManager.SetEulerRotation(rotation, local);

            return true;
        }

        /// <summary>
        /// Set the scale of the camera.
        /// </summary>
        /// <param name="scale">Scale to apply to camera.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public static bool SetScale(Vector3 scale)
        {
            WorldEngine.WorldEngine.ActiveWorld.cameraManager.SetScale(scale);

            return true;
        }
    }
}
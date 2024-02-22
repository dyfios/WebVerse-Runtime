// Copyright (c) 2019-2024 Five Squared Interactive. All rights reserved.

using FiveSQD.WebVerse.Utilities;
using UnityEngine;

namespace FiveSQD.WebVerse.Input.VR
{
    /// <summary>
    /// Class for the VR menu controller.
    /// </summary>
    public class VRMenuController : BaseController
    {
        /// <summary>
        /// Main camera.
        /// </summary>
        [Tooltip("Main camera.")]
        public Camera mainCamera;

        /// <summary>
        /// Menu object.
        /// </summary>
        [Tooltip("Menu object.")]
        public GameObject menuObject;

        /// <summary>
        /// Menu distance in meters.
        /// </summary>
        [Tooltip("Menu distance in meters.")]
        public float menuDistance;

        /// <summary>
        /// Initialize the VR Menu Controller.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Terminate the VR Menu Controller.
        /// </summary>
        public override void Terminate()
        {
            base.Terminate();
        }

        /// <summary>
        /// Toggle the VR Menu.
        /// </summary>
        public void ToggleMenu()
        {
            menuObject.SetActive(!menuObject.activeSelf);

            if (menuObject.activeSelf)
            {
                menuObject.transform.position = mainCamera.transform.TransformPoint(Vector3.forward * menuDistance);
            }
        }
    }
}
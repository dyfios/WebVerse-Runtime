// Copyright (c) 2019-2024 Five Squared Interactive. All rights reserved.

using UnityEngine;
using UnityEngine.InputSystem;

namespace FiveSQD.WebVerse.Input.Focused
{
    /// <summary>
    /// Class for the hand menu activator.
    /// </summary>
    public class HandMenuActivator : MonoBehaviour
    {
        /// <summary>
        /// The hand menu.
        /// </summary>
        [Tooltip("The hand menu.")]
        public GameObject handMenu;

        /// <summary>
        /// Whether or not the menu is world space.
        /// </summary>
        [Tooltip("Whether or not the menu is world space.")]
        public bool worldSpaceMenu;

        /// <summary>
        /// Toggle the hand menu.
        /// </summary>
        public void ToggleHandMenu(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                if (!handMenu.activeSelf)
                {
                    handMenu.transform.position = Camera.main.transform.position;
                    handMenu.transform.rotation = Camera.main.transform.rotation;
                }

                handMenu.SetActive(!handMenu.activeSelf);
            }
        }
    }
}
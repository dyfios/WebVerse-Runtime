// Copyright (c) 2019-2024 Five Squared Interactive. All rights reserved.

using UnityEngine;

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
        /// Toggle the hand menu.
        /// </summary>
        public void ToggleHandMenu()
        {
            handMenu.SetActive(!handMenu.activeSelf);
        }
    }
}
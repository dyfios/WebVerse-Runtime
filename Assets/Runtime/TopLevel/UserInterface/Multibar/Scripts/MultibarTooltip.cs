// Copyright (c) 2019-2024 Five Squared Interactive. All rights reserved.

using TMPro;
using UnityEngine;

namespace FiveSQD.WebVerse.Interface.MultibarMenu
{
    /// <summary>
    /// Class for multibar tooltip.
    /// </summary>
    public class MultibarTooltip : MonoBehaviour
    {
        /// <summary>
        /// The rect transform.
        /// </summary>
        public RectTransform rectTransform;

        /// <summary>
        /// Tooltip text.
        /// </summary>
        public TMP_Text tooltipText;

        /// <summary>
        /// Enable the tooltip.
        /// </summary>
        /// <param name="tooltip">Tooltip text to show.</param>
        /// <param name="position">Position for tooltip.</param>
        public void EnableTooltip(string tooltip, Vector2 position)
        {
            tooltipText.text = tooltip;
            rectTransform.anchoredPosition = position;
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Disable the tooltip.
        /// </summary>
        public void DisableTooltip()
        {
            gameObject.SetActive(false);
        }
    }
}
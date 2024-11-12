// Copyright (c) 2019-2024 Five Squared Interactive. All rights reserved.

using System;
using FiveSQD.WebVerse.Runtime;
using UnityEngine;

namespace FiveSQD.WebVerse.Handlers.Javascript.APIs.Environment
{
    /// <summary>
    /// Class for Environment.
    /// </summary>
    public class Environment
    {
        /// <summary>
        /// Set the sky to a texture.
        /// </summary>
        /// <param name="texture">URI of the texture to set the sky to.</param>
        public static void SetSkyTexture(string skyTextureURI)
        {
            Action<byte[]> onDownloaded = new Action<byte[]>((rawData) =>
            {
                if (rawData != null)
                {
                    Texture2D texture = new Texture2D(2, 2);
                    texture.LoadImage(rawData);
                    WorldEngine.WorldEngine.ActiveWorld.environmentManager.SetSkyTexture(texture);
                }
            });
            WebVerseRuntime.Instance.vemlHandler.DownloadFileWithoutCache(skyTextureURI, onDownloaded);
        }

        /// <summary>
        /// Set the sky to a solid color.
        /// </summary>
        /// <param name="color">Color to set the sky to.</param>
        public static void SetSolidColorSky(WorldTypes.Color color)
        {
            Color convertedColor = new Color(color.r, color.g, color.b, color.a);
            WorldEngine.WorldEngine.ActiveWorld.environmentManager.SetSolidColorSky(convertedColor);
        }
    }
}
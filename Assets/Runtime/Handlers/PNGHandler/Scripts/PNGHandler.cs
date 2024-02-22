// Copyright (c) 2019-2024 Five Squared Interactive. All rights reserved.

using FiveSQD.WebVerse.Handlers.File;
using FiveSQD.WebVerse.Utilities;
using System;
using UnityEngine;
using FiveSQD.WebVerse.Runtime;
#if USE_WEBINTERFACE
using FiveSQD.WebVerse.WebInterface.HTTP;
#endif

namespace FiveSQD.WebVerse.Handlers.PNG
{
    /// <summary>
    /// Class for the PNG Handler.
    /// </summary>
    public class PNGHandler : BaseHandler
    {
        /// <summary>
        /// Reference to the WebVerse Runtime.
        /// </summary>
        public WebVerseRuntime runtime;

        /// <summary>
        /// Initialize the PNG Handler.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Terminate the PNG Handler.
        /// </summary>
        public override void Terminate()
        {
            base.Terminate();
        }

        /// <summary>
        /// Load an image resource as a Texture2D.
        /// </summary>
        /// <param name="resourceURI">URI to get the resource from.</param>
        /// <param name="onLoaded">Action to perform when loading is complete. Provides the loaded
        /// resource as a Texture2D.</param>
        public void LoadImageResourceAsTexture2D(string resourceURI, Action<Texture2D> onLoaded)
        {
            Action onDownloaded = () =>
            {
                Texture2D img = LoadImage(System.IO.Path.Combine(runtime.fileHandler.fileDirectory,
                    FileHandler.ToFileURI(resourceURI)));
                onLoaded.Invoke(img);
            };
            DownloadPNG(resourceURI, onDownloaded);
        }

        /// <summary>
        /// Download a PNG.
        /// </summary>
        /// <param name="uri">URI to get the PNG from.</param>
        /// <param name="onDownloaded">Action to perform when downloading is complete.</param>
        /// <param name="reDownload">Whether or not to redownload the PNG if it already exists.</param>
        public void DownloadPNG(string uri, Action onDownloaded, bool reDownload = false)
        {
#if USE_WEBINTERFACE
            if (reDownload == false)
            {
                if (runtime.fileHandler.FileExistsInFileDirectory(FileHandler.ToFileURI(uri)))
                {
                    Logging.Log("[PNGHandler->DownloadPNG] File " + uri + " already exists. Using stored version.");
                    onDownloaded.Invoke();
                    return;
                }
            }

            Action<int, Texture2D> onDownloadedAction = new Action<int, Texture2D>((code, data) =>
            {
                FinishImageDownload(uri, code, data);
                onDownloaded.Invoke();
            });

            HTTPRequest request = new HTTPRequest(uri, HTTPRequest.HTTPMethod.Get, onDownloadedAction);
            request.Send();
#endif
        }

        /// <summary>
        /// Load an image from a path.
        /// </summary>
        /// <param name="path">Path to load the image from.</param>
        /// <returns>A loaded Texture2D, or null.</returns>
        public Texture2D LoadImage(string path)
        {
            byte[] rawData = System.IO.File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(rawData);
            return texture;
        }

        /// <summary>
        /// Finish the downloading of an image.
        /// </summary>
        /// <param name="uri">URI of the PNG.</param>
        /// <param name="responseCode">Response code from the download.</param>
        /// <param name="rawImage">The raw image that was downloaded.</param>
        private void FinishImageDownload(string uri, int responseCode, Texture2D rawImage)
        {
            Logging.Log("[ImageHandler->FinishImageDownload] Got response " + responseCode + " for request " + uri);

            if (responseCode != 200)
            {
                Logging.Log("[ImageHandler->FinishImageDownload] Error loading file.");
                return;
            }

            string filePath = FileHandler.ToFileURI(uri);
            if (runtime.fileHandler.FileExistsInFileDirectory(filePath))
            {
                runtime.fileHandler.DeleteFileInFileDirectory(filePath);
            }
            runtime.fileHandler.CreateFileInFileDirectory(filePath, DuplicateTexture(rawImage));
        }

        /// <summary>
        /// Duplicate a texture.
        /// </summary>
        /// <param name="source">Source texture.</param>
        /// <returns>The duplicated texture.</returns>
        private Texture2D DuplicateTexture(Texture2D source)
        {
            RenderTexture renderTex = RenderTexture.GetTemporary(
                        source.width,
                        source.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.Linear);

            Graphics.Blit(source, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = new Texture2D(source.width, source.height);
            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);
            return readableText;
        }
    }
}
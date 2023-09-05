using FiveSQD.WebVerse.Handlers.File;
using FiveSQD.WebVerse.Utilities;
using System;
using UnityEngine;
using FiveSQD.WebVerse.Runtime;
using FiveSQD.WebVerse.WebInterface.HTTP;

namespace FiveSQD.WebVerse.Handlers.PNG
{
    public class PNGHandler : BaseHandler
    {
        public WebVerseRuntime runtime;

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Terminate()
        {
            base.Terminate();
        }

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

        public void DownloadPNG(string uri, Action onDownloaded, bool reDownload = false)
        {
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
        }

        public Texture2D LoadImage(string path)
        {
            byte[] rawData = System.IO.File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(rawData);
            return texture;
        }

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
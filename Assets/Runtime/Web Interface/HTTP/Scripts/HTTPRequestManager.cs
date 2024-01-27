using FiveSQD.WebVerse.Utilities;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace FiveSQD.WebVerse.WebInterface.HTTP
{
    public class HTTPRequestManager : BaseManager
    {
        public static HTTPRequestManager instance;

        public override void Initialize()
        {
            instance = this;

            base.Initialize();
        }

        public IEnumerator HandleRequest(UnityWebRequest request, Action<int, byte[]> onFinished)
        {
            if (request != null)
            {
                yield return request.SendWebRequest();

                switch (request.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Logging.LogError("[HTTPRequestManager->HandleRequest]" + request.error);
                        onFinished.Invoke((int) request.responseCode, null);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Logging.LogError("[HTTPRequestManager->HandleRequest]" + request.error);
                        onFinished.Invoke((int) request.responseCode, null);
                        break;
                    case UnityWebRequest.Result.Success:
                        onFinished.Invoke((int) request.responseCode, request.downloadHandler.data);
                        break;
                }
            }
        }

        public IEnumerator HandleRequest(UnityWebRequest request, Action<int, Texture2D> onFinished)
        {
            if (request != null)
            {
                yield return request.SendWebRequest();

                switch (request.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Logging.LogError("[HTTPRequestManager->HandleRequest]" + request.error);
                        onFinished.Invoke((int) request.responseCode, null);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Logging.LogError("[HTTPRequestManager->HandleRequest]" + request.error);
                        onFinished.Invoke((int) request.responseCode, null);
                        break;
                    case UnityWebRequest.Result.Success:
                        Texture2D tex = new Texture2D(2, 2);
                        tex.LoadImage(request.downloadHandler.data);
                        onFinished.Invoke((int) request.responseCode, tex);
                        break;
                }
            }
        }
    }
}
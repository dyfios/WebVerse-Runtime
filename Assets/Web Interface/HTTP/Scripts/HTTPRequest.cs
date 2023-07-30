using BestHTTP;
using System;
using UnityEngine;

namespace FiveSQD.WebVerse.WebInterface.HTTP
{
    public class HTTPRequest
    {
        public enum HTTPMethod { Get = 0, Head = 1, Post = 2, Put = 3, Delete = 4,
            Patch = 5, Merge = 6, Options = 7, Connect = 8, Query = 9 }

        private BestHTTP.HTTPRequest request;

        public HTTPRequest(string uri, HTTPMethod method, Action<int, byte[]> onFinished)
        {
            request = new BestHTTP.HTTPRequest(new Uri(uri), (HTTPMethods) method, new OnRequestFinishedDelegate((req, resp) =>
            {
                if (onFinished != null)
                {
                    if (resp == null)
                    {
                        onFinished.Invoke(-1, null);
                    }
                    else
                    {
                        onFinished.Invoke(resp.StatusCode, resp.Data);
                    }
                }
            }));
        }

        public HTTPRequest(string uri, HTTPMethod method, Action<int, Texture2D> onFinished)
        {
            request = new BestHTTP.HTTPRequest(new Uri(uri), (HTTPMethods) method, new OnRequestFinishedDelegate((req, resp) =>
            {
                if (onFinished != null)
                {
                    if (resp == null)
                    {
                        onFinished.Invoke(-1, null);
                    }
                    else
                    {
                        onFinished.Invoke(resp.StatusCode, resp.DataAsTexture2D);
                    }
                }
            }));
        }

        public void Send()
        {
            if (request != null)
            {
                request.Send();
            }
        }
    }
}
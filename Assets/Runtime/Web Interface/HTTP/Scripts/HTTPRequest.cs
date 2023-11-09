// Copyright (c) 2019-2023 Five Squared Interactive. All rights reserved.

using Best.HTTP;
using System;
using UnityEngine;

namespace FiveSQD.WebVerse.WebInterface.HTTP
{
    /// <summary>
    /// Class for an HTTP request.
    /// </summary>
    public class HTTPRequest
    {
        /// <summary>
        /// Enumeration for HTTP request methods.
        /// </summary>
        public enum HTTPMethod { Get = 0, Head = 1, Post = 2, Put = 3, Delete = 4,
            Patch = 5, Merge = 6, Options = 7, Connect = 8, Query = 9 }

        /// <summary>
        /// Internal HTTP request object.
        /// </summary>
        private Best.HTTP.HTTPRequest request;

        /// <summary>
        /// Constructor for an HTTP request.
        /// </summary>
        /// <param name="uri">URI to send the request to.</param>
        /// <param name="method">Method to use in the request.</param>
        /// <param name="onFinished">Action to perform upon completing the request.</param>
        public HTTPRequest(string uri, HTTPMethod method, Action<int, byte[]> onFinished)
        {
            request = new Best.HTTP.HTTPRequest(new Uri(uri), (HTTPMethods) method, new OnRequestFinishedDelegate((req, resp) =>
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

        /// <summary>
        /// Constructor for an HTTP request.
        /// </summary>
        /// <param name="uri">URI to send the request to.</param>
        /// <param name="method">Method to use in the request.</param>
        /// <param name="onFinished">Action to perform upon completing the request.</param>
        public HTTPRequest(string uri, HTTPMethod method, Action<int, Texture2D> onFinished)
        {
            request = new Best.HTTP.HTTPRequest(new Uri(uri), (HTTPMethods) method, new OnRequestFinishedDelegate((req, resp) =>
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

        /// <summary>
        /// Send the HTTP request.
        /// </summary>
        public void Send()
        {
            if (request != null)
            {
                request.Send();
            }
        }
    }
}
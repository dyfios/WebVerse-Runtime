using FiveSQD.WebVerse.Runtime;
using FiveSQD.WebVerse.Utilities;
using System;

namespace FiveSQD.WebVerse.Handlers.Javascript.APIs.Networking
{
    public class HTTPNetworking
    {
        public struct FetchRequestOptions
        {
            public string body;
            
            public string cache;

            public string credentials;

            public string[] headers;

            public bool keepalive;

            /// <summary>
            /// HTTP Method to use (GET, POST, PUT, DELETE, PATCH, MERGE, OPTIONS, CONNECT, QUERY).
            /// </summary>
            public string method;

            public string mode;

            public string priority;

            public string redirect;

            public string referrer;

            public string referrerPolicy;
        }

        public class Request
        {
            public string body;

            public string cache;

            public string credentials;

            public string[] headers;

            public bool keepalive;

            public string integrity;

            /// <summary>
            /// HTTP Method to use (GET, POST, PUT, DELETE, PATCH, MERGE, OPTIONS, CONNECT, QUERY).
            /// </summary>
            public string method;

            public string mode;

            public string priority;

            public string redirect;

            public string referrer;

            public string referrerPolicy;

            public string resourceURI;

            public Request(string input)
            {
                body = "";
                cache = "default";
                credentials = "same-origin";
                headers = new string[0];
                integrity = "";
                keepalive = false;
                method = "GET";
                mode = "cors";
                priority = "default";
                redirect = "follow";
                referrer = "about:client";
                referrerPolicy = "";
                resourceURI = input;
            }

            public Request(string input, FetchRequestOptions options)
            {
                body = options.body;
                cache = options.cache;
                credentials = options.credentials;
                headers = options.headers;
                integrity = "";
                keepalive = options.keepalive;
                method = options.method;
                mode = options.mode;
                priority = options.priority;
                redirect = options.redirect;
                referrer = options.referrer;
                referrerPolicy = options.referrerPolicy;
                resourceURI = input;
            }
        }

        public class Response
        {
            public int status;

            public string statusText;

            public byte[] data;

            public Response(int status, string statusText, byte[] data) 
            {
                this.status = status;
                this.statusText = statusText;
                this.data = data;
            }
        }

        public static void Fetch(string resource, string onFinished)
        {
            if (string.IsNullOrEmpty(resource))
            {
                Logging.LogWarning("[HTTPNetworking:Fetch] Invalid Resource");
                return;
            }

            Fetch(new Request(resource), onFinished);
            
        }

        public static void Fetch(string resource, FetchRequestOptions options, string onFinished)
        {
            if (string.IsNullOrEmpty(resource))
            {
                Logging.LogWarning("[HTTPNetworking:Fetch] Invalid Resource");
                return;
            }

            Fetch(new Request(resource, options), onFinished);
        }

        public static void Fetch(Request request, string onFinished)
        {
            WebInterface.HTTP.HTTPRequest.HTTPMethod method = WebInterface.HTTP.HTTPRequest.HTTPMethod.Get;
            switch (request.method.ToUpper())
            {
                case "GET":
                    method = WebInterface.HTTP.HTTPRequest.HTTPMethod.Get;
                    break;

                case "POST":
                    method = WebInterface.HTTP.HTTPRequest.HTTPMethod.Post;
                    break;

                case "PUT":
                    method = WebInterface.HTTP.HTTPRequest.HTTPMethod.Put;
                    break;

                case "DELETE":
                    method = WebInterface.HTTP.HTTPRequest.HTTPMethod.Delete;
                    break;

                case "PATCH":
                    method = WebInterface.HTTP.HTTPRequest.HTTPMethod.Patch;
                    break;

                case "MERGE":
                    method = WebInterface.HTTP.HTTPRequest.HTTPMethod.Merge;
                    break;

                case "OPTIONS":
                    method = WebInterface.HTTP.HTTPRequest.HTTPMethod.Options;
                    break;

                case "CONNECT":
                    method = WebInterface.HTTP.HTTPRequest.HTTPMethod.Connect;
                    break;

                case "QUERY":
                    method = WebInterface.HTTP.HTTPRequest.HTTPMethod.Query;
                    break;

                default:
                    Logging.LogWarning("[HTTPNetworking:Fetch] Invalid Method " + request.method);
                    break;
            }

            Action<int, byte[]> onFinishedAction = new Action<int, byte[]>((code, data) =>
            {
                Response resp = new Response(code, "", data);
                if (!string.IsNullOrEmpty(onFinished))
                {
                    WebVerseRuntime.Instance.javascriptHandler.Run(onFinished.Replace("?", "resp"));
                }
            });

            WebInterface.HTTP.HTTPRequest httpReq = new WebInterface.HTTP.HTTPRequest(request.resourceURI, method, onFinishedAction);
        }
    }
}
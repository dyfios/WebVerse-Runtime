// Copyright (c) 2019-2023 Five Squared Interactive. All rights reserved.

#if USE_BESTHTTP
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using FiveSQD.WebVerse.WebInterface.HTTP;
using System;

/// <summary>
/// Unit tests for HTTP.
/// </summary>
public class HTTPTests
{
    private float httpRequestWaitPeriod = 10;

    [UnityTest]
    public IEnumerator HTTPTests_Get()
    {
        // Valid URL.
        int receivedResponse = -1;
        byte[] receivedData = null;
        Action<int, Dictionary<string, string>, byte[]> onGetResponse = new Action<int, Dictionary<string, string>, byte[]>((resp, headers, data) =>
        {
            receivedResponse = resp;
            receivedData = data;
        });
        HTTPRequest request = new HTTPRequest("https://google.com", HTTPRequest.HTTPMethod.Get, onGetResponse);
        request.Send();
        yield return new WaitForSeconds(httpRequestWaitPeriod);
        if (receivedResponse != 200 && receivedResponse != 301)
        {
            Assert.Fail("Received Response was not 200 or 301.");
        }
        Assert.IsNotNull(receivedData);

        // Invalid URL.
        receivedResponse = -1;
        receivedData = null;
        onGetResponse = new Action<int, Dictionary<string, string>, byte[]>((resp, headers, data) =>
        {
            receivedResponse = resp;
            receivedData = data;
        });
        request = new HTTPRequest("https://thisisnotarealwebsite.comqwerty", HTTPRequest.HTTPMethod.Get, onGetResponse);
        request.Send();
        yield return new WaitForSeconds(httpRequestWaitPeriod);
        Assert.AreEqual(-1, receivedResponse);
        Assert.IsNull(receivedData);
    }

    [UnityTest]
    public IEnumerator HTTPTests_Head()
    {
        // Valid URL.
        int receivedResponse = -1;
        byte[] receivedData = null;
        Action<int, Dictionary<string, string>, byte[]> onHeadResponse = new Action<int, Dictionary<string, string>, byte[]>((resp, headers, data) =>
        {
            receivedResponse = resp;
            receivedData = data;
        });
        HTTPRequest request = new HTTPRequest("https://google.com", HTTPRequest.HTTPMethod.Head, onHeadResponse);
        request.Send();
        yield return new WaitForSeconds(httpRequestWaitPeriod);
        if (receivedResponse != 200 && receivedResponse != 301)
        {
            Assert.Fail("Received Response was not 200 or 301.");
        }
        Assert.IsNull(receivedData);

        // Invalid URL.
        receivedResponse = -1;
        receivedData = null;
        onHeadResponse = new Action<int, Dictionary<string, string>, byte[]>((resp, headers, data) =>
        {
            receivedResponse = resp;
            receivedData = data;
            Debug.Log(receivedResponse + " " + receivedData);
        });
        request = new HTTPRequest("https://thisisnotarealwebsite.comqwerty", HTTPRequest.HTTPMethod.Get, onHeadResponse);
        request.Send();
        yield return new WaitForSeconds(httpRequestWaitPeriod);
        Assert.AreEqual(-1, receivedResponse);
        Assert.IsNull(receivedData);
    }

    [UnityTest]
    public IEnumerator HTTPTests_Post()
    {
        // Valid URL.
        int receivedResponse = -1;
        byte[] receivedData = null;
        Action<int, Dictionary<string, string>, byte[]> onPostResponse = new Action<int, Dictionary<string, string>, byte[]>((resp, headers, data) =>
        {
            receivedResponse = resp;
            receivedData = data;
        });
        HTTPRequest request = new HTTPRequest("https://httpbin.org/post", HTTPRequest.HTTPMethod.Post, onPostResponse);
        request.Send();
        yield return new WaitForSeconds(httpRequestWaitPeriod);
        if (receivedResponse != 200 && receivedResponse != 301)
        {
            Assert.Fail("Received Response was not 200 or 301.");
        }
        Assert.IsNotNull(receivedData);

        // Invalid URL.
        receivedResponse = -1;
        receivedData = null;
        onPostResponse = new Action<int, Dictionary<string, string>, byte[]>((resp, headers, data) =>
        {
            receivedResponse = resp;
            receivedData = data;
        });
        request = new HTTPRequest("https://thisisnotarealwebsite.comqwerty", HTTPRequest.HTTPMethod.Post, onPostResponse);
        request.Send();
        yield return new WaitForSeconds(httpRequestWaitPeriod);
        Assert.AreEqual(-1, receivedResponse);
        Assert.IsNull(receivedData);
    }

    [UnityTest]
    public IEnumerator HTTPTests_Put()
    {
        // Valid URL.
        int receivedResponse = -1;
        byte[] receivedData = null;
        Action<int, Dictionary<string, string>, byte[]> onPutResponse = new Action<int, Dictionary<string, string>, byte[]>((resp, headers, data) =>
        {
            receivedResponse = resp;
            receivedData = data;
        });
        HTTPRequest request = new HTTPRequest("https://httpbin.org/put", HTTPRequest.HTTPMethod.Put, onPutResponse);
        request.Send();
        yield return new WaitForSeconds(httpRequestWaitPeriod);
        if (receivedResponse != 200 && receivedResponse != 301)
        {
            Assert.Fail("Received Response was not 200 or 301.");
        }
        Assert.IsNotNull(receivedData);

        // Invalid URL.
        receivedResponse = -1;
        receivedData = null;
        onPutResponse = new Action<int, Dictionary<string, string>, byte[]>((resp, headers, data) =>
        {
            receivedResponse = resp;
            receivedData = data;
        });
        request = new HTTPRequest("https://thisisnotarealwebsite.comqwerty", HTTPRequest.HTTPMethod.Put, onPutResponse);
        request.Send();
        yield return new WaitForSeconds(httpRequestWaitPeriod);
        Assert.AreEqual(-1, receivedResponse);
        Assert.IsNull(receivedData);
    }

    [UnityTest]
    public IEnumerator HTTPTests_Delete()
    {
        // Valid URL.
        int receivedResponse = -1;
        byte[] receivedData = null;
        Action<int, Dictionary<string, string>, byte[]> onDeleteResponse = new Action<int, Dictionary<string, string>, byte[]>((resp, headers, data) =>
        {
            receivedResponse = resp;
            receivedData = data;
        });
        HTTPRequest request = new HTTPRequest("https://httpbin.org/delete", HTTPRequest.HTTPMethod.Delete, onDeleteResponse);
        request.Send();
        yield return new WaitForSeconds(httpRequestWaitPeriod);
        if (receivedResponse != 200 && receivedResponse != 301)
        {
            Assert.Fail("Received Response was not 200 or 301.");
        }
        Assert.IsNotNull(receivedData);

        // Invalid URL.
        receivedResponse = -1;
        receivedData = null;
        onDeleteResponse = new Action<int, Dictionary<string, string>, byte[]>((resp, headers, data) =>
        {
            receivedResponse = resp;
            receivedData = data;
        });
        request = new HTTPRequest("https://thisisnotarealwebsite.comqwerty", HTTPRequest.HTTPMethod.Delete, onDeleteResponse);
        request.Send();
        yield return new WaitForSeconds(httpRequestWaitPeriod);
        Assert.AreEqual(-1, receivedResponse);
        Assert.IsNull(receivedData);
    }

    [UnityTest]
    public IEnumerator HTTPTests_Patch()
    {
        // Valid URL.
        int receivedResponse = -1;
        byte[] receivedData = null;
        Action<int, Dictionary<string, string>, byte[]> onPatchResponse = new Action<int, Dictionary<string, string>, byte[]>((resp, headers, data) =>
        {
            receivedResponse = resp;
            receivedData = data;
        });
        HTTPRequest request = new HTTPRequest("https://httpbin.org/patch", HTTPRequest.HTTPMethod.Patch, onPatchResponse);
        request.Send();
        yield return new WaitForSeconds(httpRequestWaitPeriod);
        if (receivedResponse != 200 && receivedResponse != 301)
        {
            Assert.Fail("Received Response was not 200 or 301.");
        }
        Assert.IsNotNull(receivedData);

        // Invalid URL.
        receivedResponse = -1;
        receivedData = null;
        onPatchResponse = new Action<int, Dictionary<string, string>, byte[]>((resp, headers, data) =>
        {
            receivedResponse = resp;
            receivedData = data;
        });
        request = new HTTPRequest("https://thisisnotarealwebsite.comqwerty", HTTPRequest.HTTPMethod.Patch, onPatchResponse);
        request.Send();
        yield return new WaitForSeconds(httpRequestWaitPeriod);
        Assert.AreEqual(-1, receivedResponse);
        Assert.IsNull(receivedData);
    }
}
#endif
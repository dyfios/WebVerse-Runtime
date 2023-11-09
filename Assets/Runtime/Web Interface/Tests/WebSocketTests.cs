// Copyright (c) 2019-2023 Five Squared Interactive. All rights reserved.

#if USE_BESTHTTP
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using FiveSQD.WebVerse.WebInterface.WebSocket;
using System;

/// <summary>
/// Unit tests for WebSockets.
/// </summary>
public class WebSocketTests
{
    private float waitTime = 3;

    [UnityTest]
    public IEnumerator WebSocketTests_General()
    {
        bool connected = false;
        Action<WebSocket> onOpen = new Action<WebSocket>((ws) =>
        {
            connected = true;
        });

        bool connected2 = false;
        Action<WebSocket> onOpen2 = new Action<WebSocket>((ws) =>
        {
            connected2 = true;
        });

        Action<WebSocket, ushort, string> onClosed
            = new Action<WebSocket, ushort, string>((ws, code, msg) =>
        {
            connected = false;
        });

        Action<WebSocket, ushort, string> onClosed2
            = new Action<WebSocket, ushort, string>((ws, code, msg) =>
        {
            connected2 = false;
        });

        int binary = 0;
        Action<WebSocket, byte[]> onBinary = new Action<WebSocket, byte[]>((ws, data) =>
        {
            binary++;
        });

        int binary2 = 0;
        Action<WebSocket, byte[]> onBinary2 = new Action<WebSocket, byte[]>((ws, data) =>
        {
            binary2++;
        });

        int messages = 0;
        Action<WebSocket, string> onMessage = new Action<WebSocket, string>((ws, msg) =>
        {
            messages++;
        });

        int messages2 = 0;
        Action<WebSocket, string> onMessage2 = new Action<WebSocket, string>((ws, msg) =>
        {
            messages2++;
        });

        Action<WebSocket, string> onError = new Action<WebSocket, string>((ws, msg) =>
        {
            
        });

        Action<WebSocket, string> onError2 = new Action<WebSocket, string>((ws, msg) =>
        {

        });

        WebSocket webSocket = new WebSocket("wss://ws.postman-echo.com/raw",
            onOpen, onClosed, onBinary, onMessage, onError);
        webSocket.AddOnOpenAction(onOpen2);
        webSocket.AddOnClosedAction(onClosed2);
        webSocket.AddOnBinaryAction(onBinary2);
        webSocket.AddOnMessageAction(onMessage2);
        webSocket.AddOnErrorAction(onError2);

        Assert.IsFalse(connected);
        Assert.IsFalse(connected2);
        Assert.IsFalse(webSocket.isOpen);
        webSocket.Open();
        yield return new WaitForSeconds(waitTime);
        Assert.IsTrue(webSocket.isOpen);
        Assert.IsTrue(connected);
        Assert.IsTrue(connected2);

        webSocket.Send("test");
        yield return new WaitForSeconds(waitTime);
        Assert.AreEqual(1, messages);
        Assert.AreEqual(1, messages2);
        webSocket.Send("test");
        yield return new WaitForSeconds(waitTime);
        Assert.AreEqual(2, messages);
        Assert.AreEqual(2, messages2);
        webSocket.Send(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        // TODO: Cannot find good server for this.
        //Assert.AreEqual(0, binary);
        //Assert.AreEqual(0, binary2);
        //yield return new WaitForSeconds(waitTime);
        //Assert.AreEqual(1, binary);
        //Assert.AreEqual(1, binary2);

        webSocket.Close();
        yield return new WaitForSeconds(waitTime);
        webSocket.Send("test");
        Assert.AreEqual(2, messages);
        Assert.AreEqual(2, messages2);
    }
}
#endif
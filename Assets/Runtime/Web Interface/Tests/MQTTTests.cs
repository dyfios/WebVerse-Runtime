// Copyright (c) 2019-2023 Five Squared Interactive. All rights reserved.

#if USE_BESTHTTP
using System.Collections;
using UnityEngine.TestTools;
using FiveSQD.WebVerse.WebInterface.MQTT;
using System;
using UnityEngine;
using NUnit.Framework;

/// <summary>
/// Unit tests for MQTT.
/// </summary>
public class MQTTTests
{
    private float waitPeriod = 3;

    [UnityTest]
    public IEnumerator MQTTTests_TCP()
    {
        bool connected = false;
        Action<MQTTClient> onConnectedAction = new Action<MQTTClient>((client) =>
        {
            connected = true;
        });

        Action<MQTTClient, byte, string> onDisconnectedAction
            = new Action<MQTTClient, byte, string>((client, code, info) =>
        {
            connected = false;
        });

        Action<MQTTClient, MQTTClient.ClientState, MQTTClient.ClientState> onStateChangedAction
            = new Action<MQTTClient, MQTTClient.ClientState, MQTTClient.ClientState>((client, from, to) =>
        {
        });

        Action<MQTTClient, string> onErrorAction = new Action<MQTTClient, string>((client, info) =>
        {

        });

        Action<string> onAcknowledged = new Action<string>((info) =>
        {

        });

        int messagesReceived = 0;
        Action<MQTTClient, string, string, MQTTMessage> onMessage
            = new Action<MQTTClient, string, string, MQTTMessage>((client, topic, topicName, message) =>
        {
            messagesReceived++;
        });

        MQTTClient client = new MQTTClient("test.mosquitto.org", 1883, false, MQTTClient.Transports.TCP,
            onConnectedAction, onDisconnectedAction, onStateChangedAction, onErrorAction, "/webversetest");

        client.Connect();
        yield return new WaitForSeconds(waitPeriod);
        Assert.IsTrue(connected);

        client.Subscribe("testtopic", onAcknowledged, onMessage);
        yield return new WaitForSeconds(waitPeriod);
        Assert.AreEqual(1, messagesReceived);

        client.Publish("testtopic", "test");
        yield return new WaitForSeconds(waitPeriod);
        Assert.AreEqual(2, messagesReceived);
        client.Publish("differenttopic", "test");
        yield return new WaitForSeconds(waitPeriod);
        Assert.AreEqual(2, messagesReceived);
        client.Publish("testtopic", "test");
        yield return new WaitForSeconds(waitPeriod);
        Assert.AreEqual(3, messagesReceived);

        client.UnSubscribe("testtopic", onAcknowledged);
        yield return new WaitForSeconds(waitPeriod);
        client.Publish("testtopic", "test");
        Assert.AreEqual(3, messagesReceived);

        client.Disconnect();
        yield return new WaitForSeconds(waitPeriod);
    }

    [UnityTest]
    public IEnumerator MQTTTests_TCPS()
    {
        bool connected = false;
        Action<MQTTClient> onConnectedAction = new Action<MQTTClient>((client) =>
        {
            connected = true;
        });

        Action<MQTTClient, byte, string> onDisconnectedAction
            = new Action<MQTTClient, byte, string>((client, code, info) =>
            {
                connected = false;
            });

        Action<MQTTClient, MQTTClient.ClientState, MQTTClient.ClientState> onStateChangedAction
            = new Action<MQTTClient, MQTTClient.ClientState, MQTTClient.ClientState>((client, from, to) =>
            {
            });

        Action<MQTTClient, string> onErrorAction = new Action<MQTTClient, string>((client, info) =>
        {

        });

        Action<string> onAcknowledged = new Action<string>((info) =>
        {

        });

        int messagesReceived = 0;
        Action<MQTTClient, string, string, MQTTMessage> onMessage
            = new Action<MQTTClient, string, string, MQTTMessage>((client, topic, topicName, message) =>
            {
                messagesReceived++;
            });

        MQTTClient client = new MQTTClient("test.mosquitto.org", 8883, true, MQTTClient.Transports.TCP,
            onConnectedAction, onDisconnectedAction, onStateChangedAction, onErrorAction, "/webversetest");

        client.Connect();
        yield return new WaitForSeconds(waitPeriod);
        Assert.IsTrue(connected);

        client.Subscribe("testtopic", onAcknowledged, onMessage);
        yield return new WaitForSeconds(waitPeriod);
        Assert.AreEqual(1, messagesReceived);

        client.Publish("testtopic", "test");
        yield return new WaitForSeconds(waitPeriod);
        Assert.AreEqual(2, messagesReceived);
        client.Publish("differenttopic", "test");
        yield return new WaitForSeconds(waitPeriod);
        Assert.AreEqual(2, messagesReceived);
        client.Publish("testtopic", "test");
        yield return new WaitForSeconds(waitPeriod);
        Assert.AreEqual(3, messagesReceived);

        client.UnSubscribe("testtopic", onAcknowledged);
        yield return new WaitForSeconds(waitPeriod);
        client.Publish("testtopic", "test");
        Assert.AreEqual(3, messagesReceived);

        client.Disconnect();
        yield return new WaitForSeconds(waitPeriod);
    }

    [UnityTest]
    public IEnumerator MQTTTests_WS()
    {
        bool connected = false;
        Action<MQTTClient> onConnectedAction = new Action<MQTTClient>((client) =>
        {
            connected = true;
        });

        Action<MQTTClient, byte, string> onDisconnectedAction
            = new Action<MQTTClient, byte, string>((client, code, info) =>
            {
                connected = false;
            });

        Action<MQTTClient, MQTTClient.ClientState, MQTTClient.ClientState> onStateChangedAction
            = new Action<MQTTClient, MQTTClient.ClientState, MQTTClient.ClientState>((client, from, to) =>
            {
            });

        Action<MQTTClient, string> onErrorAction = new Action<MQTTClient, string>((client, info) =>
        {

        });

        Action<string> onAcknowledged = new Action<string>((info) =>
        {

        });

        int messagesReceived = 0;
        Action<MQTTClient, string, string, MQTTMessage> onMessage
            = new Action<MQTTClient, string, string, MQTTMessage>((client, topic, topicName, message) =>
            {
                messagesReceived++;
            });

        MQTTClient client = new MQTTClient("test.mosquitto.org", 8080, false, MQTTClient.Transports.WebSockets,
            onConnectedAction, onDisconnectedAction, onStateChangedAction, onErrorAction, "/webversetest");

        client.Connect();
        yield return new WaitForSeconds(waitPeriod);
        Assert.IsTrue(connected);

        client.Subscribe("testtopic", onAcknowledged, onMessage);
        yield return new WaitForSeconds(waitPeriod);
        Assert.AreEqual(1, messagesReceived);

        client.Publish("testtopic", "test");
        yield return new WaitForSeconds(waitPeriod);
        Assert.AreEqual(2, messagesReceived);
        client.Publish("differenttopic", "test");
        yield return new WaitForSeconds(waitPeriod);
        Assert.AreEqual(2, messagesReceived);
        client.Publish("testtopic", "test");
        yield return new WaitForSeconds(waitPeriod);
        Assert.AreEqual(3, messagesReceived);

        client.UnSubscribe("testtopic", onAcknowledged);
        yield return new WaitForSeconds(waitPeriod);
        client.Publish("testtopic", "test");
        Assert.AreEqual(3, messagesReceived);

        client.Disconnect();
        yield return new WaitForSeconds(waitPeriod);
    }

    [UnityTest]
    public IEnumerator MQTTTests_WSS()
    {
        bool connected = false;
        Action<MQTTClient> onConnectedAction = new Action<MQTTClient>((client) =>
        {
            connected = true;
        });

        Action<MQTTClient, byte, string> onDisconnectedAction
            = new Action<MQTTClient, byte, string>((client, code, info) =>
            {
                connected = false;
            });

        Action<MQTTClient, MQTTClient.ClientState, MQTTClient.ClientState> onStateChangedAction
            = new Action<MQTTClient, MQTTClient.ClientState, MQTTClient.ClientState>((client, from, to) =>
            {
            });

        Action<MQTTClient, string> onErrorAction = new Action<MQTTClient, string>((client, info) =>
        {

        });

        Action<string> onAcknowledged = new Action<string>((info) =>
        {

        });

        int messagesReceived = 0;
        Action<MQTTClient, string, string, MQTTMessage> onMessage
            = new Action<MQTTClient, string, string, MQTTMessage>((client, topic, topicName, message) =>
            {
                messagesReceived++;
            });

        MQTTClient client = new MQTTClient("test.mosquitto.org", 8081, true, MQTTClient.Transports.WebSockets,
            onConnectedAction, onDisconnectedAction, onStateChangedAction, onErrorAction, "/webversetest");

        client.Connect();
        yield return new WaitForSeconds(waitPeriod);
        Assert.IsTrue(connected);

        client.Subscribe("testtopic", onAcknowledged, onMessage);
        yield return new WaitForSeconds(waitPeriod);
        Assert.AreEqual(1, messagesReceived);

        client.Publish("testtopic", "test");
        yield return new WaitForSeconds(waitPeriod);
        Assert.AreEqual(2, messagesReceived);
        client.Publish("differenttopic", "test");
        yield return new WaitForSeconds(waitPeriod);
        Assert.AreEqual(2, messagesReceived);
        client.Publish("testtopic", "test");
        yield return new WaitForSeconds(waitPeriod);
        Assert.AreEqual(3, messagesReceived);

        client.UnSubscribe("testtopic", onAcknowledged);
        yield return new WaitForSeconds(waitPeriod);
        client.Publish("testtopic", "test");
        Assert.AreEqual(3, messagesReceived);

        client.Disconnect();
        yield return new WaitForSeconds(waitPeriod);
    }
}
#endif
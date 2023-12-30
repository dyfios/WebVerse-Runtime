// Copyright (c) 2019-2023 Five Squared Interactive. All rights reserved.

using FiveSQD.WebVerse.Handlers.File;
using FiveSQD.WebVerse.Runtime;
using FiveSQD.WebVerse.Utilities;
using System;
#if USE_WEBINTERFACE
using FiveSQD.WebVerse.WebInterface.HTTP;
#endif
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FiveSQD.WebVerse.Handlers.VEML.Schema.V1_0;
using FiveSQD.WebVerse.VOSSynchronization;
using FiveSQD.WebVerse.WorldEngine.Utilities;
using FiveSQD.WebVerse.WorldEngine.Entity;
using System.Xml;

namespace FiveSQD.WebVerse.Handlers.VEML
{
    /// <summary>
    /// Class for the VEML Handler.
    /// </summary>
    public class VEMLHandler : BaseHandler
    {
        /// <summary>
        /// Enumeration for a VEML version number.
        /// </summary>
        public enum VEMLVersion { Unknown, v1_0 }

        /// <summary>
        /// Reference to the WebVerse runtime.
        /// </summary>
        public WebVerseRuntime runtime;

        /// <summary>
        /// Timeout for requests.
        /// </summary>
        public float timeout = 10;

        /// <summary>
        /// Number of entities that are being loaded.
        /// </summary>
        private int loadingEntities = 0;

        /// <summary>
        /// Initialize the VEML Handler.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Terminate the VEML Handler.
        /// </summary>
        public override void Terminate()
        {
            base.Terminate();
        }

        /// <summary>
        /// Get the name of a world.
        /// </summary>
        /// <param name="resourceURI">URI of the world file.</param>
        /// <param name="onComplete">Action to invoke when the name has been determined. Provides
        /// a string containing the world name.</param>
        public void GetWorldName(string resourceURI, Action<string> onComplete)
        {
            Action onDownloaded = () =>
            {
                Schema.V1_0.veml veml = LoadVEML(Path.Combine(runtime.fileHandler.fileDirectory,
                    FileHandler.ToFileURI(resourceURI)));

                if (veml == null)
                {
                    Logging.Log("[VEMLHandler->GetWorldName] Not a VEML document: "
                        + resourceURI);
                    onComplete.Invoke("ERROR");
                }
                else
                {
                    if (veml.metadata == null)
                    {
                        Logging.Log("[VEMLHandler->GetWorldName] No metadata: "
                            + resourceURI);
                        onComplete.Invoke("ERROR");
                    }
                    else
                    {
                        onComplete.Invoke(veml.metadata.title);
                    }
                }
            };
            DownloadVEML(resourceURI, onDownloaded);
        }

        /// <summary>
        /// Initiate the loading of a VEML document into the current world.
        /// </summary>
        /// <param name="resourceURI">URI of the world file.</param>
        public void LoadVEMLDocumentIntoWorld(string resourceURI)
        {
            Action onDownloaded = () =>
            {
                Schema.V1_0.veml veml = LoadVEML(Path.Combine(runtime.fileHandler.fileDirectory,
                    FileHandler.ToFileURI(resourceURI)));

                if (veml == null)
                {
                    Logging.Log("[VEMLHandler->LoadVEMLDocumentIntoWorld] Not a VEML document: "
                        + resourceURI);
                }
                else
                {
                    StartCoroutine(ApplyVEMLDocument(veml, Path.GetDirectoryName(resourceURI)));
                }
            };
            DownloadVEML(resourceURI, onDownloaded);
        }

        /// <summary>
        /// Download a VEML document.
        /// </summary>
        /// <param name="uri">URI of the VEML file.</param>
        /// <param name="onDownloaded">Action to invoke when the file has been downloaded.</param>
        /// <param name="reDownload">Whether or not to redownload the file if it already exists.</param>
        public void DownloadVEML(string uri, Action onDownloaded, bool reDownload = false)
        {
#if USE_WEBINTERFACE
            if (reDownload == false)
            {
                if (runtime.fileHandler.FileExistsInFileDirectory(FileHandler.ToFileURI(uri)))
                {
                    Logging.Log("[VEMLHandler->DownloadVEML] File " + uri + " already exists. Using stored version.");
                    onDownloaded.Invoke();
                    return;
                }
            }

            Action<int, byte[]> onDownloadedAction = new Action<int, byte[]>((code, data) =>
            {
                FinishVEMLDownload(uri, code, data);
                onDownloaded.Invoke();
            });

            HTTPRequest request = new HTTPRequest(uri, HTTPRequest.HTTPMethod.Get, onDownloadedAction);
            request.Send();
#endif
        }

        /// <summary>
        /// Load a VEML document from a path.
        /// </summary>
        /// <param name="path">Path to load the document from.</param>
        /// <returns>A loaded VEML class, or null.</returns>
        public Schema.V1_0.veml LoadVEML(string path)
        {
            byte[] rawData = System.IO.File.ReadAllBytes(path);

            VEMLVersion version = VEMLVersion.v1_0;
            Schema.V1_0.veml veml = null;
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(Schema.V1_0.veml));
                TextReader reader = new StringReader(System.Text.Encoding.UTF8.GetString(rawData));
                XmlReader xmlReader = XmlReader.Create(reader);

                // Attempt deserialization using v1.0 (latest) of the schema. In future,
                // old XML will be converted to current version before being deserialized.
                if (ser.CanDeserialize(xmlReader))
                {
                    version = VEMLVersion.v1_0;
                    Logging.Log("[VEMLHandler->LoadVEML] Document is VEML v1.0");
                    veml = (Schema.V1_0.veml) ser.Deserialize(xmlReader);
                }

                // Unknown VEML Version. Throw Error.
                else
                {
                    version = VEMLVersion.Unknown;
                    Logging.LogWarning("[VEMLHandler->LoadVEML] Unknown VEML version.");
                    // Attempt deserialization to throw appropriate exceptions.
                    ser.Deserialize(xmlReader);
                    return null;
                }
            }
            catch (Exception e)
            {
                Logging.LogWarning("[VEMLHandler->LoadVEML] VEML document deserialization failure " + e.ToString());
                return null;
            }

            return veml;
        }

        /// <summary>
        /// Load a script resource as a string.
        /// </summary>
        /// <param name="resourceURI">URI of the script resource.</param>
        /// <param name="onLoaded">Action to invoke when loading of the script resource
        /// is complete. Provides a string containing the script resource.</param>
        public void LoadScriptResourceAsString(string resourceURI, Action<string> onLoaded)
        {
            Action onDownloaded = () =>
            {
                string script = LoadScript(Path.Combine(runtime.fileHandler.fileDirectory,
                    FileHandler.ToFileURI(resourceURI)));
                onLoaded.Invoke(script);
            };
            DownloadScript(resourceURI, onDownloaded);
        }

        /// <summary>
        /// Download a script.
        /// </summary>
        /// <param name="uri">URI of the script resource.</param>
        /// <param name="onDownloaded">Action to invoke when downloading of the script is complete.</param>
        /// <param name="reDownload">Whether or not to redownload the script if it already exists.</param>
        public void DownloadScript(string uri, Action onDownloaded, bool reDownload = false)
        {
#if USE_WEBINTERFACE
            if (reDownload == false)
            {
                if (runtime.fileHandler.FileExistsInFileDirectory(FileHandler.ToFileURI(uri)))
                {
                    Logging.Log("[VEMLHandler->DownloadScript] File " + uri + " already exists. Using stored version.");
                    onDownloaded.Invoke();
                    return;
                }
            }

            Action<int, byte[]> onDownloadedAction = new Action<int, byte[]>((code, data) =>
            {
                FinishScriptDownload(uri, code, data);
                onDownloaded.Invoke();
            });

            HTTPRequest request = new HTTPRequest(uri, HTTPRequest.HTTPMethod.Get, onDownloadedAction);
            request.Send();
#endif
        }

        /// <summary>
        /// Load a script from a path.
        /// </summary>
        /// <param name="path">Path to load the script from.</param>
        /// <returns>A string containing the loaded script, or null.</returns>
        public string LoadScript(string path)
        {
            byte[] rawData = System.IO.File.ReadAllBytes(path);
            string script = System.Text.Encoding.UTF8.GetString(rawData);
            return script;
        }

        /// <summary>
        /// Download a file.
        /// </summary>
        /// <param name="uri">URI from which to get the file.</param>
        /// <param name="onDownloaded">Action to invoke when downloading the file is complete. Provides
        /// a byte array containing the file data.</param>
        /// <param name="reDownload">Whether or not to redownload the file if it already exists.</param>
        public void DownloadFile(string uri, Action<byte[]> onDownloaded, bool reDownload = false)
        {
#if USE_WEBINTERFACE
            if (reDownload == false)
            {
                if (runtime.fileHandler.FileExistsInFileDirectory(FileHandler.ToFileURI(uri)))
                {
                    Logging.Log("[VEMLHandler->DownloadFile] File " + uri + " already exists. Using stored version.");

                    onDownloaded.Invoke(runtime.fileHandler.GetFileInFileDirectory(FileHandler.ToFileURI(uri)));
                    return;
                }
            }

            Action<int, byte[]> onDownloadedAction = new Action<int, byte[]>((code, data) =>
            {
                onDownloaded.Invoke(data);
            });

            HTTPRequest request = new HTTPRequest(uri, HTTPRequest.HTTPMethod.Get, onDownloadedAction);
            request.Send();
#endif
        }

        /// <summary>
        /// Finish downloading a script.
        /// </summary>
        /// <param name="uri">URI from which to get the file.</param>
        /// <param name="onDownloaded">Action to invoke when downloading the file is complete. Provides
        /// a byte array containing the file data.</param>
        /// <param name="reDownload">Whether or not to redownload the file if it already exists.</param>
        private void FinishScriptDownload(string uri, int responseCode, byte[] rawData)
        {
            Logging.Log("[VEMLHandler->FinishScriptDownload] Got response " + responseCode + " for request " + uri);

            if (responseCode != 200)
            {
                Logging.Log("[VEMLHandler->FinishScriptDownload] Error loading file.");
                return;
            }

            string filePath = FileHandler.ToFileURI(uri);
            if (runtime.fileHandler.FileExistsInFileDirectory(filePath))
            {
                runtime.fileHandler.DeleteFileInFileDirectory(filePath);
            }
            runtime.fileHandler.CreateFileInFileDirectory(filePath, rawData);
        }

        /// <summary>
        /// Finish downloading a VEML file.
        /// </summary>
        /// <param name="uri">URI which the VEML file is from.</param>
        /// <param name="responseCode">Response code received.</param>
        /// <param name="rawData">Raw data received.</param>
        private void FinishVEMLDownload(string uri, int responseCode, byte[] rawData)
        {
            Logging.Log("[VEMLHandler->FinishVEMLDownload] Got response " + responseCode + " for request " + uri);

            if (responseCode != 200)
            {
                Logging.Log("[VEMLHandler->FinishVEMLDownload] Error loading file.");
                return;
            }

            string filePath = FileHandler.ToFileURI(uri);
            if (runtime.fileHandler.FileExistsInFileDirectory(filePath))
            {
                runtime.fileHandler.DeleteFileInFileDirectory(filePath);
            }
            runtime.fileHandler.CreateFileInFileDirectory(filePath, rawData);
        }

        /// <summary>
        /// Apply a VEML document.
        /// </summary>
        /// <param name="vemlDocument">The VEML document.</param>
        /// <param name="baseURI">Base URI of the VEML document.</param>
        private IEnumerator ApplyVEMLDocument(Schema.V1_0.veml vemlDocument, string baseURI)
        {
            string formattedBaseURI = FormatURI(baseURI);

            string[] scripts = null;

            bool scriptsDoneProcessing = false;
            Action<string[]> onScriptsProcessed = new Action<string[]>((scrs) =>
            {
                scripts = scrs;
                scriptsDoneProcessing = true;
            });

            if (ProcessMetadata(vemlDocument, baseURI, onScriptsProcessed) == false)
            {
                Logging.LogWarning("[VEMLHandler->ApplyVEMLDocument] Error processing metadata.");
                yield break;
            }

            if (ProcessEnvironment(vemlDocument, baseURI) == false)
            {
                Logging.LogWarning("[VEMLHandler->ApplyVEMLDocument] Error processing environment.");
                yield break;
            }


            // Wait for all scripts to download.
            float elapsedTime = 0f;
            while (scriptsDoneProcessing == false && elapsedTime < timeout)
            {
                yield return new WaitForSeconds(0.25f);
                elapsedTime += 0.25f;
            }

            // Wait for all entities to load.
            elapsedTime = 0f;
            while (loadingEntities > 0 && elapsedTime < timeout)
            {
                yield return new WaitForSeconds(0.25f);
                elapsedTime += 0.25f;
            }

            if (scripts != null)
            {
                foreach (string script in scripts)
                {
                    WebVerseRuntime.Instance.javascriptHandler.RunScript(script);
                }
            }
        }

        /// <summary>
        /// Process VEML document metadata.
        /// </summary>
        /// <param name="vemlDocument">The VEML document.</param>
        /// <param name="baseURI">Base URI of the VEML document.</param>
        /// <param name="onScriptsProcessed">Action to invoke when scripts are processed. Provides an array of
        /// strings containing the script contents.</param>
        /// <returns>Whether or not the operation succeeded.</returns>
        private bool ProcessMetadata(Schema.V1_0.veml vemlDocument, string baseURI, Action<string[]> onScriptsProcessed)
        {
            string formattedBaseURI = FormatURI(baseURI);

            // Validate metadata.
            if (vemlDocument.metadata == null)
            {
                Logging.LogWarning("[VEMLHandler->ProcessMetadata] VEML document missing required field: metadata.");
                return false;
            }
            else
            {
                if (string.IsNullOrEmpty(vemlDocument.metadata.title))
                {
                    Logging.LogWarning("[VEMLHandler->ProcessMetadata] VEML document metadata missing required field: title.");
                    return false;
                }
                else
                {

                }

                StartCoroutine(ProcessScripts(vemlDocument, baseURI, onScriptsProcessed));

                if (ProcessInputEvents(vemlDocument, baseURI) == false)
                {
                    Logging.LogWarning("[VEMLHandler->ProcessMetadata] Error processing input events.");
                    return false;
                }

                if (ProcessSynchronizers(vemlDocument, baseURI) == false)
                {
                    Logging.LogWarning("[VEMLHandler->ProcessMetadata] Error processing synchronizers.");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Process VEML document environment.
        /// </summary>
        /// <param name="vemlDocument">The VEML document.</param>
        /// <param name="baseURI">Base URI of the VEML document.</param>
        /// <returns>Whether or not the operation succeeded.</returns>
        private bool ProcessEnvironment(Schema.V1_0.veml vemlDocument, string baseURI)
        {
            string formattedBaseURI = FormatURI(baseURI);

            // Validate environment.
            if (vemlDocument.environment == null)
            {
                Logging.LogWarning("[VEMLHandler->ProcessEnvironment] VEML document missing required field: environment.");
                return false;
            }
            else
            {
                // Validate background.
                if (vemlDocument.environment.background == null)
                {
                    Logging.LogWarning("[VEMLHandler->ProcessEnvironment] VEML document environment missing required field: background.");
                    return false;
                }
                else
                {
                    ProcessBackground(vemlDocument.environment.background.Item, formattedBaseURI);
                }

                if (ProcessEntities(vemlDocument, baseURI) == false)
                {
                    Logging.LogWarning("[VEMLHandler->ProcessEnvironment] Error processing entities.");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Process VEML document metadata.
        /// </summary>
        /// <param name="vemlDocument">The VEML document.</param>
        /// <param name="baseURI">Base URI of the VEML document.</param>
        /// <param name="onProcessed">Action to invoke when scripts are processed. Provides an array of
        /// strings containing the script contents.</param>
        private IEnumerator ProcessScripts(Schema.V1_0.veml vemlDocument, string baseURI, Action<string[]> onProcessed)
        {
            string formattedBaseURI = FormatURI(baseURI);

            Dictionary<Guid, string> scriptsToRun = new Dictionary<Guid, string>();

            // Set up scripts.
            if (vemlDocument.metadata.script != null)
            {
                foreach (string script in vemlDocument.metadata.script)
                {
                    Guid scriptID = Guid.NewGuid();
                    if (script.EndsWith(".js"))
                    {
                        // Path to script has been provided. Load script and place in dictionary.
                        Action<string> onLoadedAction = new Action<string>((scr) =>
                        {
                            scriptsToRun[scriptID] = scr;
                        });
                        scriptsToRun.Add(scriptID, null);

                        LoadScriptResourceAsString(FullyQualifyURI(script, formattedBaseURI), onLoadedAction);
                    }
                    else
                    {
                        // Raw script has been provided. Place in dictionary.
                        scriptsToRun.Add(scriptID, script);
                    }
                }
            }

            // Wait for all scripts to download.
            float elapsedTime = 0f;
            bool allLoaded = true;
            do
            {
                allLoaded = true;
                foreach (string script in scriptsToRun.Values)
                {
                    if (script == null)
                    {
                        allLoaded = false;
                        yield return new WaitForSeconds(0.25f);
                        elapsedTime += 0.25f;
                        break;
                    }
                }
            } while (allLoaded == false && elapsedTime < timeout);

            List<string> scripts = new List<string>();
            foreach (string script in scriptsToRun.Values)
            {
                scripts.Add(script);
            }

            onProcessed.Invoke(scripts.ToArray());

            yield return null;
        }

        /// <summary>
        /// Process VEML document metadata input events.
        /// </summary>
        /// <param name="vemlDocument">The VEML document.</param>
        /// <param name="baseURI">Base URI of the VEML document.</param>
        /// <returns>Whether or not the operation succeeded.</returns>
        private bool ProcessInputEvents(Schema.V1_0.veml vemlDocument, string baseURI)
        {
            // Set up input events.
            if (vemlDocument.metadata.inputevent != null)
            {
                foreach (inputevent inputEvent in vemlDocument.metadata.inputevent)
                {
                    WebVerseRuntime.Instance.inputManager.RegisterInputEvent(inputEvent.input, inputEvent.@event);
                }
            }

            return true;
        }

        /// <summary>
        /// Process VEML document metadata synchronizers.
        /// </summary>
        /// <param name="vemlDocument">The VEML document.</param>
        /// <param name="baseURI">Base URI of the VEML document.</param>
        /// <returns>Whether or not the operation succeeded.</returns>
        private bool ProcessSynchronizers(Schema.V1_0.veml vemlDocument, string baseURI)
        {
            // Set up synchronizers.
            if (vemlDocument.metadata.synchronizationservice != null)
            {
                foreach (synchronizationservice synchronizationservice in vemlDocument.metadata.synchronizationservice)
                {
                    switch (synchronizationservice.type.ToLower())
                    {
                        case "vos":
#if USE_WEBINTERFACE
                            bool tls = false;
                            string hostPortSection = "";
                            if (synchronizationservice.address.StartsWith("vss:"))
                            {
                                tls = false;
                                hostPortSection = synchronizationservice.address.Substring(4);
                            }
                            else if (synchronizationservice.address.StartsWith("vsss:"))
                            {
                                tls = true;
                                hostPortSection = synchronizationservice.address.Substring(5);
                            }
                            else
                            {
                                hostPortSection = synchronizationservice.address;
                            }
                            string[] parts = hostPortSection.Split(':');
                            if (parts.Length != 2)
                            {
                                Logging.LogWarning("[VEMLHandler->ProcessSynchronizers] VEML document contains invalid synchronization service address: "
                                    + synchronizationservice.address);
                                break;
                            }
                            WebVerseRuntime.Instance.vosSynchronizationManager.AddSynchronizerAndSession(
                                synchronizationservice.id, parts[0], int.Parse(parts[1]), tls, WebInterface.MQTT.MQTTClient.Transports.TCP,
                                synchronizationservice.session);
#endif
                            break;

                        default:
                            Logging.LogWarning("[VEMLHandler->ProcessSynchronizers] VEML document defines unknown synchronization service type: "
                                + synchronizationservice.type);
                            break;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Process background.
        /// </summary>
        /// <param name="entry">The entry for the background setting.</param>
        /// <param name="uri">Base URI of the VEML document.</param>
        private void ProcessBackground(string entry, string uri)
        {
            switch (entry.ToLower())
            {
                case "white":
                    WorldEngine.WorldEngine.ActiveWorld.environmentManager.SetSolidColorSky(Color.white);
                    break;

                case "black":
                    WorldEngine.WorldEngine.ActiveWorld.environmentManager.SetSolidColorSky(Color.black);
                    break;

                case "grey":
                case "gray":
                    WorldEngine.WorldEngine.ActiveWorld.environmentManager.SetSolidColorSky(Color.gray);
                    break;

                case "red":
                    WorldEngine.WorldEngine.ActiveWorld.environmentManager.SetSolidColorSky(Color.red);
                    break;

                case "yellow":
                    WorldEngine.WorldEngine.ActiveWorld.environmentManager.SetSolidColorSky(Color.yellow);
                    break;

                case "blue":
                    WorldEngine.WorldEngine.ActiveWorld.environmentManager.SetSolidColorSky(Color.blue);
                    break;

                case "cyan":
                    WorldEngine.WorldEngine.ActiveWorld.environmentManager.SetSolidColorSky(Color.cyan);
                    break;

                case "magenta":
                    WorldEngine.WorldEngine.ActiveWorld.environmentManager.SetSolidColorSky(Color.magenta);
                    break;

                case "green":
                    WorldEngine.WorldEngine.ActiveWorld.environmentManager.SetSolidColorSky(Color.green);
                    break;

                default:
                    uri = uri.Replace("http:\\", "http://").Replace("https:\\", "https://");
                    if (uri.Contains("http:/") && !uri.Contains("http://"))
                    {
                        uri = uri.Replace("http:/", "http://");
                    }
                    if (uri.Contains("https:/") && !uri.Contains("https://"))
                    {
                        uri = uri.Replace("https:/", "https://");
                    }
                    if (entry[0] != '#')
                    {
                        // Assume texture.
                        Action<byte[]> onDownloaded = new Action<byte[]>((rawData) =>
                        {
                            if (rawData != null)
                            {
                                Texture2D texture = new Texture2D(2, 2);
                                texture.LoadImage(rawData);
                                WorldEngine.WorldEngine.ActiveWorld.environmentManager.SetSkyTexture(texture);
                            }
                        });
                        DownloadFile(FullyQualifyURI(entry, uri), onDownloaded);
                    }
                    else
                    {
                        // Assume color code.
                        WorldEngine.WorldEngine.ActiveWorld.environmentManager.SetSolidColorSky(FromHex(entry));
                    }
                    break;
            }
        }

        /// <summary>
        /// Process VEML document environment entities.
        /// </summary>
        /// <param name="vemlDocument">The VEML document.</param>
        /// <param name="baseURI">Base URI of the VEML document.</param>
        /// <returns>Whether or not the operation succeeded.</returns>
        private bool ProcessEntities(Schema.V1_0.veml vemlDocument, string baseURI)
        {
            string formattedBaseURI = FormatURI(baseURI);

            Dictionary<entity, entity> entities;

            // Set up entities.
            entities = new Dictionary<entity, entity>();
            if (vemlDocument.environment.entity != null)
            {
                entity[] es = vemlDocument.environment.entity;
                SetUpEntityIDs(ref es);
                vemlDocument.environment.entity = es;

                loadingEntities = 0;
                entities = GetAllChildEntities(vemlDocument.environment.entity);

                foreach (entity entity in entities.Keys)
                {
                    if (entity.tag == null)
                    {
                        Logging.LogWarning("[VEMLHandler->ProcessEntities] VEML document environment entity missing required field: tag.");
                        return false;
                    }
                    else
                    {

                    }

                    if (entity.transform == null)
                    {
                        Logging.LogWarning("[VEMLHandler->ProcessEntities] VEML document environment entity missing required field: transform.");
                        return false;
                    }
                    else
                    {
                        if (entity.transform is scaletransform)
                        {
                            if (((scaletransform) entity.transform).position == null)
                            {
                                Logging.LogWarning("[VEMLHandler->ProcessEntities] VEML document environment entity transform missing required field: position.");
                                return false;
                            }
                            else
                            {

                            }

                            if (((scaletransform) entity.transform).rotation == null)
                            {
                                Logging.LogWarning("[VEMLHandler->ProcessEntities] VEML document environment entity transform missing required field: rotation.");
                                return false;
                            }
                            else
                            {

                            }

                            if (((scaletransform) entity.transform).scale == null)
                            {
                                Logging.LogWarning("[VEMLHandler->ProcessEntities] VEML document environment entity transform missing required field: scale.");
                                return false;
                            }
                            else
                            {

                            }
                        }
                        else if (entity.transform is sizetransform)
                        {
                            if (((sizetransform) entity.transform).position == null)
                            {
                                Logging.LogWarning("[VEMLHandler->ProcessEntities] VEML document environment entity transform missing required field: position.");
                                return false;
                            }
                            else
                            {

                            }

                            if (((sizetransform) entity.transform).rotation == null)
                            {
                                Logging.LogWarning("[VEMLHandler->ProcessEntities] VEML document environment entity transform missing required field: rotation.");
                                return false;
                            }
                            else
                            {

                            }

                            if (((sizetransform) entity.transform).size == null)
                            {
                                Logging.LogWarning("[VEMLHandler->ProcessEntities] VEML document environment entity transform missing required field: size.");
                                return false;
                            }
                            else
                            {

                            }
                        }
                        else if (entity.transform is canvastransform)
                        {
                            if (((canvastransform) entity.transform).positionpercent == null)
                            {
                                Logging.LogWarning("[VEMLHandler->ProcessEntities] VEML document environment entity transform missing required field: position-percent.");
                                return false;
                            }
                            else
                            {

                            }

                            if (((canvastransform) entity.transform).sizepercent == null)
                            {
                                Logging.LogWarning("[VEMLHandler->ProcessEntities] VEML document environment entity transform missing required field: size-percent.");
                                return false;
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            Logging.LogWarning("[VEMLHandler->ProcessEntities] Invalid transform.");
                            return false;
                        }
                    }

                    // Handle entity specific properties.
                    if (entity is meshentity)
                    {
                        loadingEntities++;
                        if (ProcessMeshEntity((meshentity) entity, baseURI) == false)
                        {
                            LogSystem.LogWarning("[VEMLHandler->ProcessEntity] Error processing mesh entity.");
                            return false;
                        }
                    }
                    else if (entity is containerentity)
                    {
                        loadingEntities++;
                        if (ProcessContainerEntity((containerentity) entity, baseURI) == false)
                        {
                            LogSystem.LogWarning("[VEMLHandler->ProcessEntity] Error processing container entity.");
                            return false;
                        }
                    }
                    else if (entity is characterentity)
                    {
                        loadingEntities++;
                        if (ProcessCharacterEntity((characterentity) entity, baseURI) == false)
                        {
                            LogSystem.LogWarning("[VEMLHandler->ProcessEntity] Error processing character entity.");
                            return false;
                        }
                    }
                    else if (entity is lightentity)
                    {
                        loadingEntities++;
                        if (ProcessLightEntity((lightentity) entity, baseURI) == false)
                        {
                            LogSystem.LogWarning("[VEMLHandler->ProcessEntity] Error processing light entity.");
                            return false;
                        }
                    }
                    else if (entity is terrainentity)
                    {
                        loadingEntities++;
                        if (ProcessTerrainEntity((terrainentity) entity, baseURI) == false)
                        {
                            LogSystem.LogWarning("[VEMLHandler->ProcessEntity] Error processing terrain entity.");
                            return false;
                        }
                    }
                    else if (entity is textentity)
                    {
                        loadingEntities++;
                        if (ProcessTextEntity((textentity) entity, baseURI) == false)
                        {
                            LogSystem.LogWarning("[VEMLHandler->ProcessEntity] Error processing text entity.");
                            return false;
                        }
                    }
                    else if (entity is buttonentity)
                    {
                        loadingEntities++;
                        if (ProcessButtonEntity((buttonentity) entity, baseURI) == false)
                        {
                            LogSystem.LogWarning("[VEMLHandler->ProcessEntity] Error processing button entity.");
                            return false;
                        }
                    }
                    else if (entity is canvasentity)
                    {
                        loadingEntities++;
                        if (ProcessCanvasEntity((canvasentity) entity, baseURI) == false)
                        {
                            LogSystem.LogWarning("[VEMLHandler->ProcessEntity] Error processing canvas entity.");
                            return false;
                        }
                    }
                    else if (entity is inputentity)
                    {
                        loadingEntities++;
                        if (ProcessInputEntity((inputentity) entity, baseURI) == false)
                        {
                            LogSystem.LogWarning("[VEMLHandler->ProcessEntity] Error processing input entity.");
                            return false;
                        }
                    }
                    else if (entity is voxelentity)
                    {
                        loadingEntities++;
                        if (ProcessVoxelEntity((voxelentity) entity, baseURI) == false)
                        {
                            LogSystem.LogWarning("[VEMLHandler->ProcessEntity] Error processing voxel entity.");
                            return false;
                        }
                    }
                    else
                    {
                        Logging.LogWarning("[VEMLHandler->ApplyVEMLDocument] Unknown kind of entity: " + entity.tag);
                    }
                }

                StartCoroutine(ApplyEntityHierarchy(entities, baseURI));
            }

            return true;
        }

        /// <summary>
        /// Process a mesh entity.
        /// </summary>
        /// <param name="entity">The mesh entity.</param>
        /// <param name="baseURI">Base URI of the VEML document.</param>
        /// <returns>Whether or not the operation succeeded.</returns>
        private bool ProcessMeshEntity(meshentity entity, string baseURI)
        {
            string formattedBaseURI = FormatURI(baseURI);

            if (entity.meshresource == null)
            {
                Logging.LogWarning("[VEMLHandler->ProcessMeshEntity] VEML document environment mesh entity missing required field: mesh-resource.");
                return false;
            }

            if (string.IsNullOrEmpty(entity.meshname))
            {
                Logging.LogWarning("[VEMLHandler->ProcessMeshEntity] VEML document environment mesh entity missing required field: mesh-name.");
                return false;
            }

            if (string.IsNullOrEmpty(entity.tag))
            {
                Logging.LogWarning("[VEMLHandler->ProcessMeshEntity] VEML document environment mesh entity missing required field: tag.");
                return false;
            }

            if (string.IsNullOrEmpty(entity.id))
            {
                entity.id = Guid.NewGuid().ToString();
            }

            Action<WorldEngine.Entity.MeshEntity> onLoadedAction
                = new Action<WorldEngine.Entity.MeshEntity>((meshEntity) =>
                {
                    meshEntity.entityTag = entity.tag;
                    meshEntity.SetVisibility(true);
                    meshEntity.SetParent(null);
                    ApplyTransform(meshEntity, entity.transform, true, true, false);
#if USE_WEBINTERFACE
                    if (!string.IsNullOrEmpty(entity.synchronizer))
                    {
                        Tuple<VOSSynchronizer, Guid> synchronizer
                            = WebVerseRuntime.Instance.vosSynchronizationManager.GetSynchronizerAndSession(entity.synchronizer);
                        if (synchronizer == null || synchronizer.Item1 == null)
                        {
                            LogSystem.LogWarning("[VEMLHandler->ProcessMeshEntity] Error synchronizing entity.");
                            return;
                        }
                        synchronizer.Item1.AddSynchronizedEntity(meshEntity, false, entity.meshname);
                    }
#endif
                    Javascript.APIs.Entity.EntityAPIHelper.RegisterPrivateEntity(meshEntity);
                    loadingEntities--;
                });
            List<string> resources = new List<string>();
            foreach (string res in entity.meshresource)
            {
                resources.Add(FullyQualifyURI(res, formattedBaseURI));
            }
            WebVerseRuntime.Instance.gltfHandler.LoadGLTFResourceAsMeshEntity(FullyQualifyURI(entity.meshname, formattedBaseURI),
                resources.ToArray(), Guid.Parse(entity.id), onLoadedAction);

            return true;
        }

        /// <summary>
        /// Process a container entity.
        /// </summary>
        /// <param name="entity">The container entity.</param>
        /// <param name="baseURI">Base URI of the VEML document.</param>
        /// <returns>Whether or not the operation succeeded.</returns>
        private bool ProcessContainerEntity(containerentity entity, string baseURI)
        {
            string formattedBaseURI = FormatURI(baseURI);

            if (string.IsNullOrEmpty(entity.tag))
            {
                Logging.LogWarning("[VEMLHandler->ProcessContainerEntity] VEML document environment container entity missing required field: tag.");
                return false;
            }

            if (string.IsNullOrEmpty(entity.id))
            {
                entity.id = Guid.NewGuid().ToString();
            }

            Vector3 positionValue;
            Quaternion rotationValue;
            Vector3 sizeValue;
            if (entity.transform is scaletransform)
            {
                positionValue = new Vector3((float) ((scaletransform) entity.transform).position.x,
                    (float) ((scaletransform) entity.transform).position.y,
                    (float) ((scaletransform) entity.transform).position.z);
                rotationValue = new Quaternion((float) ((scaletransform) entity.transform).rotation.x,
                    (float) ((scaletransform) entity.transform).rotation.y,
                    (float) ((scaletransform) entity.transform).rotation.z,
                    (float) ((scaletransform) entity.transform).rotation.w);
                sizeValue = new Vector3((float) ((scaletransform) entity.transform).scale.x,
                    (float) ((scaletransform) entity.transform).scale.y,
                    (float) ((scaletransform) entity.transform).scale.z);
            }
            else if (entity.transform is sizetransform)
            {
                LogSystem.LogWarning("[VEMLHandler->ProcessContainerEntity] Size Transform not allowed.");
                return false;
            }
            else if (entity.transform is canvastransform)
            {
                LogSystem.LogWarning("[VEMLHandler->ProcessContainerEntity] Canvas Transform not allowed.");
                return false;
            }
            else
            {
                LogSystem.LogWarning("[VEMLHandler->ProcessContainerEntity] Unknown transform type.");
                return false;
            }

            Action onLoadEvent = new Action(() =>
            {
                BaseEntity loadedEntity = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(entity.id));
                if (loadedEntity == null)
                {
                    LogSystem.LogError("[VEMLHandler->ProcessContainerEntity] Error finding entity to synchronize.");
                    return;
                }

                loadedEntity.SetVisibility(true);
#if USE_WEBINTERFACE
                if (!string.IsNullOrEmpty(entity.synchronizer))
                {
                    Tuple<VOSSynchronizer, Guid> synchronizer
                        = WebVerseRuntime.Instance.vosSynchronizationManager.GetSynchronizerAndSession(entity.synchronizer);
                    if (synchronizer == null || synchronizer.Item1 == null)
                    {
                        LogSystem.LogWarning("[VEMLHandler->ProcessContainerEntity] Error synchronizing entity.");
                        return;
                    }
                    synchronizer.Item1.AddSynchronizedEntity(loadedEntity, false);
                }
#endif
                Javascript.APIs.Entity.EntityAPIHelper.RegisterPrivateEntity(loadedEntity);
                loadingEntities--;
            });

            WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadContainerEntity(null, positionValue, rotationValue,
                sizeValue, Guid.Parse(entity.id), entity.tag, false, onLoadEvent);

            return true;
        }

        /// <summary>
        /// Process a character entity.
        /// </summary>
        /// <param name="entity">The character entity.</param>
        /// <param name="baseURI">Base URI of the VEML document.</param>
        /// <returns>Whether or not the operation succeeded.</returns>
        private bool ProcessCharacterEntity(characterentity entity, string baseURI)
        {
            if (string.IsNullOrEmpty(entity.tag))
            {
                Logging.LogWarning("[VEMLHandler->ProcessCharacterEntity] VEML document environment character entity missing required field: tag.");
                return false;
            }

            if (string.IsNullOrEmpty(entity.id))
            {
                entity.id = Guid.NewGuid().ToString();
            }

            bool isSize;
            Vector3 positionValue;
            Quaternion rotationValue;
            Vector3 sizeValue;
            if (entity.transform is scaletransform)
            {
                isSize = false;
                positionValue = new Vector3((float) ((scaletransform) entity.transform).position.x,
                    (float) ((scaletransform) entity.transform).position.y,
                    (float) ((scaletransform) entity.transform).position.z);
                rotationValue = new Quaternion((float) ((scaletransform) entity.transform).rotation.x,
                    (float) ((scaletransform) entity.transform).rotation.y,
                    (float) ((scaletransform) entity.transform).rotation.z,
                    (float) ((scaletransform) entity.transform).rotation.w);
                sizeValue = new Vector3((float) ((scaletransform) entity.transform).scale.x,
                    (float) ((scaletransform) entity.transform).scale.y,
                    (float) ((scaletransform) entity.transform).scale.z);
            }
            else if (entity.transform is sizetransform)
            {
                isSize = true;
                positionValue = new Vector3((float) ((sizetransform) entity.transform).position.x,
                    (float) ((sizetransform) entity.transform).position.y,
                    (float) ((sizetransform) entity.transform).position.z);
                rotationValue = new Quaternion((float) ((sizetransform) entity.transform).rotation.x,
                    (float) ((sizetransform) entity.transform).rotation.y,
                    (float) ((sizetransform) entity.transform).rotation.z,
                    (float) ((sizetransform) entity.transform).rotation.w);
                sizeValue = new Vector3((float) ((sizetransform) entity.transform).size.x,
                    (float) ((sizetransform) entity.transform).size.y,
                    (float) ((sizetransform) entity.transform).size.z);
            }
            else if (entity.transform is canvastransform)
            {
                LogSystem.LogWarning("[VEMLHandler->ProcessCharacterEntity] Canvas Transform not allowed.");
                return false;
            }
            else
            {
                LogSystem.LogWarning("[VEMLHandler->ProcessCharacterEntity] Unknown transform type.");
                return false;
            }

            Action onLoadEvent = new Action(() =>
            {
                BaseEntity loadedEntity = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(entity.id));
                if (loadedEntity == null)
                {
                    LogSystem.LogError("[VEMLHandler->ProcessCharacterEntity] Error finding entity to synchronize.");
                    return;
                }

                loadedEntity.entityTag = entity.tag;
                loadedEntity.SetVisibility(true);
#if USE_WEBINTERFACE
                if (!string.IsNullOrEmpty(entity.synchronizer))
                {
                    Tuple<VOSSynchronizer, Guid> synchronizer
                        = WebVerseRuntime.Instance.vosSynchronizationManager.GetSynchronizerAndSession(entity.synchronizer);
                    if (synchronizer == null || synchronizer.Item1 == null)
                    {
                        LogSystem.LogWarning("[VEMLHandler->ProcessCharacterEntity] Error synchronizing entity.");
                        return;
                    }
                    synchronizer.Item1.AddSynchronizedEntity(loadedEntity, false);
                }
#endif
                Javascript.APIs.Entity.EntityAPIHelper.RegisterPrivateEntity(loadedEntity);
                loadingEntities--;
            });

            WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadCharacterEntity(null, positionValue, rotationValue,
                sizeValue, Guid.Parse(entity.id), entity.tag, isSize, onLoadEvent);

            return true;
        }

        /// <summary>
        /// Process a light entity.
        /// </summary>
        /// <param name="entity">The light entity.</param>
        /// <param name="baseURI">Base URI of the VEML document.</param>
        /// <returns>Whether or not the operation succeeded.</returns>
        private bool ProcessLightEntity(lightentity entity, string baseURI)
        {
            if (string.IsNullOrEmpty(entity.tag))
            {
                Logging.LogWarning("[VEMLHandler->ProcessLightEntity] VEML document environment light entity missing required field: tag.");
                return false;
            }

            if (string.IsNullOrEmpty(entity.id))
            {
                entity.id = Guid.NewGuid().ToString();
            }

            Vector3 positionValue;
            Quaternion rotationValue;
            if (entity.transform is scaletransform)
            {
                positionValue = new Vector3((float) ((scaletransform) entity.transform).position.x,
                    (float) ((scaletransform) entity.transform).position.y,
                    (float) ((scaletransform) entity.transform).position.z);
                rotationValue = new Quaternion((float) ((scaletransform) entity.transform).rotation.x,
                    (float) ((scaletransform) entity.transform).rotation.y,
                    (float) ((scaletransform) entity.transform).rotation.z,
                    (float) ((scaletransform) entity.transform).rotation.w);
            }
            else if (entity.transform is sizetransform)
            {
                LogSystem.LogWarning("[VEMLHandler->ProcessLightEntity] Size Transform not allowed.");
                return false;
            }
            else if (entity.transform is canvastransform)
            {
                LogSystem.LogWarning("[VEMLHandler->ProcessLightEntity] Canvas Transform not allowed.");
                return false;
            }
            else
            {
                LogSystem.LogWarning("[VEMLHandler->ProcessLightEntity] Unknown transform type.");
                return false;
            }

            Action onLoadEvent = new Action(() =>
            {
                BaseEntity loadedEntity = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(entity.id));
                if (loadedEntity == null)
                {
                    LogSystem.LogError("[VEMLHandler->ProcessLightEntity] Error finding entity to synchronize.");
                    return;
                }

                loadedEntity.SetVisibility(true);
#if USE_WEBINTERFACE
                if (!string.IsNullOrEmpty(entity.synchronizer))
                {
                    Tuple<VOSSynchronizer, Guid> synchronizer
                        = WebVerseRuntime.Instance.vosSynchronizationManager.GetSynchronizerAndSession(entity.synchronizer);
                    if (synchronizer == null || synchronizer.Item1 == null)
                    {
                        LogSystem.LogWarning("[VEMLHandler->ProcessLightEntity] Error synchronizing entity.");
                        return;
                    }
                    synchronizer.Item1.AddSynchronizedEntity(loadedEntity, false);
                }
#endif
                Javascript.APIs.Entity.EntityAPIHelper.RegisterPrivateEntity(loadedEntity);
                loadingEntities--;
            });

            WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadLightEntity(null, positionValue, rotationValue,
                Guid.Parse(entity.id), entity.tag, onLoadEvent);

            return true;
        }

        /// <summary>
        /// Process a terrain entity.
        /// </summary>
        /// <param name="entity">The terrain entity.</param>
        /// <param name="baseURI">Base URI of the VEML document.</param>
        /// <returns>Whether or not the operation succeeded.</returns>
        private bool ProcessTerrainEntity(terrainentity entity, string baseURI)
        {
            if (string.IsNullOrEmpty(entity.tag))
            {
                Logging.LogWarning("[VEMLHandler->ProcessTerrainEntity] VEML document environment terrain entity missing required field: tag.");
                return false;
            }

            if (string.IsNullOrEmpty(entity.id))
            {
                entity.id = Guid.NewGuid().ToString();
            }

            bool isSize;
            Vector3 positionValue;
            Quaternion rotationValue;
            Vector3 sizeValue;
            if (entity.transform is scaletransform)
            {
                isSize = false;
                positionValue = new Vector3((float) ((scaletransform) entity.transform).position.x,
                    (float) ((scaletransform) entity.transform).position.y,
                    (float) ((scaletransform) entity.transform).position.z);
                rotationValue = new Quaternion((float) ((scaletransform) entity.transform).rotation.x,
                    (float) ((scaletransform) entity.transform).rotation.y,
                    (float) ((scaletransform) entity.transform).rotation.z,
                    (float) ((scaletransform) entity.transform).rotation.w);
                sizeValue = new Vector3((float) ((scaletransform) entity.transform).scale.x,
                    (float) ((scaletransform) entity.transform).scale.y,
                    (float) ((scaletransform) entity.transform).scale.z);
            }
            else if (entity.transform is sizetransform)
            {
                isSize = true;
                positionValue = new Vector3((float) ((sizetransform) entity.transform).position.x,
                    (float) ((sizetransform) entity.transform).position.y,
                    (float) ((sizetransform) entity.transform).position.z);
                rotationValue = new Quaternion((float) ((sizetransform) entity.transform).rotation.x,
                    (float) ((sizetransform) entity.transform).rotation.y,
                    (float) ((sizetransform) entity.transform).rotation.z,
                    (float) ((sizetransform) entity.transform).rotation.w);
                sizeValue = new Vector3((float) ((sizetransform) entity.transform).size.x,
                    (float) ((sizetransform) entity.transform).size.y,
                    (float) ((sizetransform) entity.transform).size.z);
            }
            else if (entity.transform is canvastransform)
            {
                LogSystem.LogWarning("[VEMLHandler->ProcessTerrainEntity] Canvas Transform not allowed.");
                return false;
            }
            else
            {
                LogSystem.LogWarning("[VEMLHandler->ProcessTerrainEntity] Unknown transform type.");
                return false;
            }

            Action onLoadEvent = new Action(() =>
            {
                BaseEntity loadedEntity = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(entity.id));
                if (loadedEntity == null)
                {
                    LogSystem.LogError("[VEMLHandler->ProcessTerrainEntity] Error finding entity to synchronize.");
                    return;
                }

                loadedEntity.SetVisibility(true);
#if USE_WEBINTERFACE
                if (!string.IsNullOrEmpty(entity.synchronizer))
                {
                    Tuple<VOSSynchronizer, Guid> synchronizer
                        = WebVerseRuntime.Instance.vosSynchronizationManager.GetSynchronizerAndSession(entity.synchronizer);
                    if (synchronizer == null || synchronizer.Item1 == null)
                    {
                        LogSystem.LogWarning("[VEMLHandler->ProcessTerrainEntity] Error synchronizing entity.");
                        return;
                    }
                    synchronizer.Item1.AddSynchronizedEntity(loadedEntity, false);
                }
#endif
                Javascript.APIs.Entity.EntityAPIHelper.RegisterPrivateEntity(loadedEntity);
                loadingEntities--;
            });

            float[,] heights = ParseCSVHeights(entity.heights);
            WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadTerrainEntity((float) entity.length, (float) entity.width,
                (float) entity.height, heights, null, positionValue, rotationValue,
                sizeValue, Guid.Parse(entity.id), isSize, entity.tag, onLoadEvent);

            return true;
        }

        /// <summary>
        /// Process a text entity.
        /// </summary>
        /// <param name="entity">The text entity.</param>
        /// <param name="baseURI">Base URI of the VEML document.</param>
        /// <returns>Whether or not the operation succeeded.</returns>
        private bool ProcessTextEntity(textentity entity, string baseURI)
        {
            string formattedBaseURI = FormatURI(baseURI);

            if (string.IsNullOrEmpty(entity.tag))
            {
                Logging.LogWarning("[VEMLHandler->ProcessTextEntity] VEML document environment text entity missing required field: tag.");
                return false;
            }

            if (string.IsNullOrEmpty(entity.id))
            {
                entity.id = Guid.NewGuid().ToString();
            }

            Vector2 positionPercentValue;
            Vector2 sizePercentValue;
            if (entity.transform is scaletransform)
            {
                LogSystem.LogWarning("[VEMLHandler->ProcessTextEntity] Scale Transform not allowed.");
                return false;
            }
            else if (entity.transform is sizetransform)
            {
                LogSystem.LogWarning("[VEMLHandler->ProcessTextEntity] Size Transform not allowed.");
                return false;
            }
            else if (entity.transform is canvastransform)
            {
                positionPercentValue = new Vector2(
                    (float) ((canvastransform) entity.transform).positionpercent.x,
                    (float) ((canvastransform) entity.transform).positionpercent.y);
                sizePercentValue = new Vector2(
                    (float) ((canvastransform) entity.transform).sizepercent.x,
                    (float) ((canvastransform) entity.transform).sizepercent.y);
            }
            else
            {
                LogSystem.LogWarning("[VEMLHandler->ProcessTextEntity] Unknown transform type.");
                return false;
            }

            Action onLoadEvent = new Action(() =>
            {
                BaseEntity loadedEntity = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(entity.id));
                if (loadedEntity == null)
                {
                    LogSystem.LogError("[VEMLHandler->ProcessTextEntity] Error finding entity to synchronize.");
                    return;
                }

                loadedEntity.SetVisibility(true);
#if USE_WEBINTERFACE
                if (!string.IsNullOrEmpty(entity.synchronizer))
                {
                    Tuple<VOSSynchronizer, Guid> synchronizer
                        = WebVerseRuntime.Instance.vosSynchronizationManager.GetSynchronizerAndSession(entity.synchronizer);
                    if (synchronizer == null || synchronizer.Item1 == null)
                    {
                        LogSystem.LogWarning("[VEMLHandler->ProcessTextEntity] Error synchronizing entity.");
                        return;
                    }
                    synchronizer.Item1.AddSynchronizedEntity(loadedEntity, false);
                }
#endif
                Javascript.APIs.Entity.EntityAPIHelper.RegisterPrivateEntity(loadedEntity);
                loadingEntities--;
            });

            WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadTextEntity(entity.text, (int) entity.fontsize,
                null, positionPercentValue, sizePercentValue, Guid.Parse(entity.id), entity.tag, onLoadEvent);

            return true;
        }

        /// <summary>
        /// Process a button entity.
        /// </summary>
        /// <param name="entity">The button entity.</param>
        /// <param name="baseURI">Base URI of the VEML document.</param>
        /// <returns>Whether or not the operation succeeded.</returns>
        private bool ProcessButtonEntity(buttonentity entity, string baseURI)
        {
            string formattedBaseURI = FormatURI(baseURI);

            if (string.IsNullOrEmpty(entity.tag))
            {
                Logging.LogWarning("[VEMLHandler->ProcessButtonEntity] VEML document environment button entity missing required field: tag.");
                return false;
            }

            if (string.IsNullOrEmpty(entity.id))
            {
                entity.id = Guid.NewGuid().ToString();
            }

            Vector2 positionPercentValue;
            Vector2 sizePercentValue;
            if (entity.transform is scaletransform)
            {
                LogSystem.LogWarning("[VEMLHandler->ProcessButtonEntity] Scale Transform not allowed.");
                return false;
            }
            else if (entity.transform is sizetransform)
            {
                LogSystem.LogWarning("[VEMLHandler->ProcessButtonEntity] Size Transform not allowed.");
                return false;
            }
            else if (entity.transform is canvastransform)
            {
                positionPercentValue = new Vector2(
                    (float) ((canvastransform) entity.transform).positionpercent.x,
                    (float) ((canvastransform) entity.transform).positionpercent.y);
                sizePercentValue = new Vector2(
                    (float) ((canvastransform) entity.transform).sizepercent.x,
                    (float) ((canvastransform) entity.transform).sizepercent.y);
            }
            else
            {
                LogSystem.LogWarning("[VEMLHandler->ProcessButtonEntity] Unknown transform type.");
                return false;
            }

            Action onClickEvent = new Action(() =>
            {
                if (!string.IsNullOrEmpty(entity.onclickevent))
                {
                    WebVerseRuntime.Instance.javascriptHandler.Run(entity.onclickevent);
                }
            });

            Action onLoadEvent = new Action(() =>
            {
                BaseEntity loadedEntity = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(entity.id));
                if (loadedEntity == null)
                {
                    LogSystem.LogError("[VEMLHandler->ProcessButtonEntity] Error finding entity to synchronize.");
                    return;
                }

                loadedEntity.SetVisibility(true);
#if USE_WEBINTERFACE
                if (!string.IsNullOrEmpty(entity.synchronizer))
                {
                    Tuple<VOSSynchronizer, Guid> synchronizer
                        = WebVerseRuntime.Instance.vosSynchronizationManager.GetSynchronizerAndSession(entity.synchronizer);
                    if (synchronizer == null || synchronizer.Item1 == null)
                    {
                        LogSystem.LogWarning("[VEMLHandler->ProcessButtonEntity] Error synchronizing entity.");
                        return;
                    }
                    synchronizer.Item1.AddSynchronizedEntity(loadedEntity, false);
                }
#endif
                Javascript.APIs.Entity.EntityAPIHelper.RegisterPrivateEntity(loadedEntity);
                loadingEntities--;
            });

            WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadButtonEntity(null, positionPercentValue,
                sizePercentValue, onClickEvent, Guid.Parse(entity.id), entity.tag, onLoadEvent);

            return true;
        }

        /// <summary>
        /// Process a canvas entity.
        /// </summary>
        /// <param name="entity">The canvas entity.</param>
        /// <param name="baseURI">Base URI of the VEML document.</param>
        /// <returns>Whether or not the operation succeeded.</returns>
        private bool ProcessCanvasEntity(canvasentity entity, string baseURI)
        {
            string formattedBaseURI = FormatURI(baseURI);

            if (string.IsNullOrEmpty(entity.tag))
            {
                Logging.LogWarning("[VEMLHandler->ProcessCanvasEntity] VEML document environment canvas entity missing required field: tag.");
                return false;
            }

            if (string.IsNullOrEmpty(entity.id))
            {
                entity.id = Guid.NewGuid().ToString();
            }

            bool isSize;
            Vector3 positionValue;
            Quaternion rotationValue;
            Vector3 sizeValue;
            if (entity.transform is scaletransform)
            {
                isSize = false;
                positionValue = new Vector3((float) ((scaletransform) entity.transform).position.x,
                    (float) ((scaletransform) entity.transform).position.y,
                    (float) ((scaletransform) entity.transform).position.z);
                rotationValue = new Quaternion((float) ((scaletransform) entity.transform).rotation.x,
                    (float) ((scaletransform) entity.transform).rotation.y,
                    (float) ((scaletransform) entity.transform).rotation.z,
                    (float) ((scaletransform) entity.transform).rotation.w);
                sizeValue = new Vector3((float) ((scaletransform) entity.transform).scale.x,
                    (float) ((scaletransform) entity.transform).scale.y,
                    (float) ((scaletransform) entity.transform).scale.z);
            }
            else if (entity.transform is sizetransform)
            {
                isSize = true;
                positionValue = new Vector3((float) ((sizetransform) entity.transform).position.x,
                    (float) ((sizetransform) entity.transform).position.y,
                    (float) ((sizetransform) entity.transform).position.z);
                rotationValue = new Quaternion((float) ((sizetransform) entity.transform).rotation.x,
                    (float) ((sizetransform) entity.transform).rotation.y,
                    (float) ((sizetransform) entity.transform).rotation.z,
                    (float) ((sizetransform) entity.transform).rotation.w);
                sizeValue = new Vector3((float) ((sizetransform) entity.transform).size.x,
                    (float) ((sizetransform) entity.transform).size.y,
                    (float) ((sizetransform) entity.transform).size.z);
            }
            else if (entity.transform is canvastransform)
            {
                LogSystem.LogWarning("[VEMLHandler->ProcessCanvasEntity] Canvas Transform not allowed.");
                return false;
            }
            else
            {
                LogSystem.LogWarning("[VEMLHandler->ProcessCanvasEntity] Unknown transform type.");
                return false;
            }

            Action onLoadEvent = new Action(() =>
            {
                BaseEntity loadedEntity = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(entity.id));
                if (loadedEntity == null)
                {
                    LogSystem.LogError("[VEMLHandler->ProcessCanvasEntity] Error finding entity to synchronize.");
                    return;
                }

                loadedEntity.SetVisibility(true);

                // Default to screen canvas.
                ((CanvasEntity) loadedEntity).MakeScreenCanvas();

#if USE_WEBINTERFACE
                if (!string.IsNullOrEmpty(entity.synchronizer))
                {
                    Tuple<VOSSynchronizer, Guid> synchronizer
                        = WebVerseRuntime.Instance.vosSynchronizationManager.GetSynchronizerAndSession(entity.synchronizer);
                    if (synchronizer == null || synchronizer.Item1 == null)
                    {
                        LogSystem.LogWarning("[VEMLHandler->ProcessCanvasEntity] Error synchronizing entity.");
                        return;
                    }
                    synchronizer.Item1.AddSynchronizedEntity(loadedEntity, false);
                }
#endif
                Javascript.APIs.Entity.EntityAPIHelper.RegisterPrivateEntity(loadedEntity);
                loadingEntities--;
            });

            WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadCanvasEntity(null, positionValue, rotationValue,
                sizeValue, Guid.Parse(entity.id), isSize, entity.tag, onLoadEvent);

            return true;
        }

        /// <summary>
        /// Process an input entity.
        /// </summary>
        /// <param name="entity">The input entity.</param>
        /// <param name="baseURI">Base URI of the VEML document.</param>
        /// <returns>Whether or not the operation succeeded.</returns>
        private bool ProcessInputEntity(inputentity entity, string baseURI)
        {
            string formattedBaseURI = FormatURI(baseURI);

            if (string.IsNullOrEmpty(entity.tag))
            {
                Logging.LogWarning("[VEMLHandler->ProcessInputEntity] VEML document environment input entity missing required field: tag.");
                return false;
            }

            if (string.IsNullOrEmpty(entity.id))
            {
                entity.id = Guid.NewGuid().ToString();
            }

            Vector2 positionPercentValue;
            Vector2 sizePercentValue;
            if (entity.transform is scaletransform)
            {
                LogSystem.LogWarning("[VEMLHandler->ProcessInputEntity] Scale Transform not allowed.");
                return false;
            }
            else if (entity.transform is sizetransform)
            {
                LogSystem.LogWarning("[VEMLHandler->ProcessInputEntity] Size Transform not allowed.");
                return false;
            }
            else if (entity.transform is canvastransform)
            {
                positionPercentValue = new Vector2(
                    (float) ((canvastransform) entity.transform).positionpercent.x,
                    (float) ((canvastransform) entity.transform).positionpercent.y);
                sizePercentValue = new Vector2(
                    (float) ((canvastransform) entity.transform).sizepercent.x,
                    (float) ((canvastransform) entity.transform).sizepercent.y);
            }
            else
            {
                LogSystem.LogWarning("[VEMLHandler->ProcessInputEntity] Unknown transform type.");
                return false;
            }

            Action onLoadEvent = new Action(() =>
            {
                BaseEntity loadedEntity = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(entity.id));
                if (loadedEntity == null)
                {
                    LogSystem.LogError("[VEMLHandler->ProcessInputEntity] Error finding entity to synchronize.");
                    return;
                }

                loadedEntity.SetVisibility(true);
#if USE_WEBINTERFACE
                if (!string.IsNullOrEmpty(entity.synchronizer))
                {
                    Tuple<VOSSynchronizer, Guid> synchronizer
                        = WebVerseRuntime.Instance.vosSynchronizationManager.GetSynchronizerAndSession(entity.synchronizer);
                    if (synchronizer == null || synchronizer.Item1 == null)
                    {
                        LogSystem.LogWarning("[VEMLHandler->ProcessInputEntity] Error synchronizing entity.");
                        return;
                    }
                    synchronizer.Item1.AddSynchronizedEntity(loadedEntity, false);
                }
#endif
                Javascript.APIs.Entity.EntityAPIHelper.RegisterPrivateEntity(loadedEntity);
                loadingEntities--;
            });

            WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadInputEntity(null, positionPercentValue,
                sizePercentValue, Guid.Parse(entity.id), entity.tag, onLoadEvent);

            return true;
        }

        /// <summary>
        /// Process a voxel entity.
        /// </summary>
        /// <param name="entity">The voxel entity.</param>
        /// <param name="baseURI">Base URI of the VEML document.</param>
        /// <returns>Whether or not the operation succeeded.</returns>
        private bool ProcessVoxelEntity(voxelentity entity, string baseURI)
        {
            if (string.IsNullOrEmpty(entity.tag))
            {
                Logging.LogWarning("[VEMLHandler->ProcessVoxelEntity] VEML document environment voxel entity missing required field: tag.");
                return false;
            }

            if (string.IsNullOrEmpty(entity.id))
            {
                entity.id = Guid.NewGuid().ToString();
            }

            Vector3 positionValue;
            Quaternion rotationValue;
            Vector3 sizeValue;
            if (entity.transform is scaletransform)
            {
                positionValue = new Vector3((float) ((scaletransform) entity.transform).position.x,
                    (float) ((scaletransform) entity.transform).position.y,
                    (float) ((scaletransform) entity.transform).position.z);
                rotationValue = new Quaternion((float) ((scaletransform) entity.transform).rotation.x,
                    (float) ((scaletransform) entity.transform).rotation.y,
                    (float) ((scaletransform) entity.transform).rotation.z,
                    (float) ((scaletransform) entity.transform).rotation.w);
                sizeValue = new Vector3((float) ((scaletransform) entity.transform).scale.x,
                    (float) ((scaletransform) entity.transform).scale.y,
                    (float) ((scaletransform) entity.transform).scale.z);
            }
            else if (entity.transform is sizetransform)
            {
                LogSystem.LogWarning("[VEMLHandler->ProcessVoxelEntity] Size Transform not allowed.");
                return false;
            }
            else if (entity.transform is canvastransform)
            {
                LogSystem.LogWarning("[VEMLHandler->ProcessVoxelEntity] Canvas Transform not allowed.");
                return false;
            }
            else
            {
                LogSystem.LogWarning("[VEMLHandler->ProcessVoxelEntity] Unknown transform type.");
                return false;
            }

            Action onLoadEvent = new Action(() =>
            {
                BaseEntity loadedEntity = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(entity.id));
                if (loadedEntity == null)
                {
                    LogSystem.LogError("[VEMLHandler->ProcessVoxelEntity] Error finding entity to synchronize.");
                    return;
                }

                loadedEntity.SetVisibility(true);
#if USE_WEBINTERFACE
                if (!string.IsNullOrEmpty(entity.synchronizer))
                {
                    Tuple<VOSSynchronizer, Guid> synchronizer
                        = WebVerseRuntime.Instance.vosSynchronizationManager.GetSynchronizerAndSession(entity.synchronizer);
                    if (synchronizer == null || synchronizer.Item1 == null)
                    {
                        LogSystem.LogWarning("[VEMLHandler->ProcessVoxelEntity] Error synchronizing entity.");
                        return;
                    }
                    synchronizer.Item1.AddSynchronizedEntity(loadedEntity, false);
                }
#endif
                Javascript.APIs.Entity.EntityAPIHelper.RegisterPrivateEntity(loadedEntity);
                loadingEntities--;
            });

            WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadVoxelEntity(null, positionValue, rotationValue,
                sizeValue, Guid.Parse(entity.id), entity.tag, onLoadEvent);

            return true;
        }

        /// <summary>
        /// Apply the hierarchy of entities.
        /// </summary>
        /// <param name="entities">Dictionary of entities and their parents.</param>
        /// <param name="baseURI">Base URI of the VEML document.</param>
        private IEnumerator ApplyEntityHierarchy(Dictionary<entity, entity> entities, string baseURI)
        {
            if (entities == null)
            {
                LogSystem.Log("[VEMLHandler->ApplyEntityHierarchy] No entities found.");
            }

            float elapsedTime = 0;
            while (loadingEntities > 0)
            {
                if (elapsedTime > timeout)
                {
                    Logging.LogError("[VEMLHandler->ApplyEntityHierarchy] Load timed out.");
                    yield break;
                }
                elapsedTime += 0.25f;
                yield return new WaitForSeconds(0.25f);
            }

            foreach (KeyValuePair<entity, entity> entityParentRelationship in entities)
            {
                if (entityParentRelationship.Key == null)
                {
                    LogSystem.LogError("[VEMLHandler->ApplyEntityHierarchy] Invalid entity parent relationship.");
                    yield break;
                }

                if (string.IsNullOrEmpty(entityParentRelationship.Key.id))
                {
                    LogSystem.LogError("[VEMLHandler->ApplyEntityHierarchy] Invalid entity.");
                    yield break;
                }

                BaseEntity entityToSet =
                    WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(entityParentRelationship.Key.id));

                BaseEntity entityParent = entityParentRelationship.Value == null ? null :
                    string.IsNullOrEmpty(entityParentRelationship.Value.id) ? null :
                    WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(entityParentRelationship.Value.id));

                if (entityToSet == null)
                {
                    LogSystem.LogError("[VEMLHandler->ApplyEntityHierarchy] Error finding entity to set parent of.");
                    yield break;
                }

                if (entityToSet is UIElementEntity)
                {
                    if (entityParent is not UIEntity)
                    {
                        LogSystem.LogError("[VEMLHandler->ApplyEntityHierarchy] UI Element Entity not parented by UI Entity.");
                        yield break;
                    }
                    ((UIElementEntity) entityToSet).SetParent((UIEntity) entityParent);

                    Vector2 posPercent = new Vector2((float) ((canvastransform) entityParentRelationship.Key.transform).positionpercent.x,
                        (float) ((canvastransform) entityParentRelationship.Key.transform).positionpercent.y);
                    Vector2 sizePercent = new Vector2((float) ((canvastransform) entityParentRelationship.Key.transform).sizepercent.x,
                        (float) ((canvastransform) entityParentRelationship.Key.transform).sizepercent.y);
                    ((UIElementEntity) entityToSet).SetPositionPercent(posPercent, false);
                    ((UIElementEntity) entityToSet).SetSizePercent(sizePercent, false);
                }
                else
                {
                    entityToSet.SetParent(entityParent);
                }
            }
        }

        /// <summary>
        /// Get a color from a hex string.
        /// </summary>
        /// <param name="hex">The VEML-compliant hex color string.</param>
        /// <returns>A color matching the hex string.</returns>
        private Color FromHex(string hex)
        {
            hex = hex.Replace("#", "");
            if (hex.Length == 6)
            {
                // RGB.
                int r = Convert.ToInt32(hex.Substring(0, 2), 16);
                int g = Convert.ToInt32(hex.Substring(2, 2), 16);
                int b = Convert.ToInt32(hex.Substring(4, 2), 16);
                return new Color((float) r / 255, (float) g / 255, (float) b / 255, 1);
            }
            else if (hex.Length == 8)
            {
                // RGBA.
                int r = Convert.ToInt32(hex.Substring(0, 2), 16);
                int g = Convert.ToInt32(hex.Substring(2, 2), 16);
                int b = Convert.ToInt32(hex.Substring(4, 2), 16);
                int a = Convert.ToInt32(hex.Substring(6, 2), 16);
                return new Color((float) r / 255, (float) g / 255, (float) b / 255, (float) a / 255);
            }
            else
            {
                Logging.LogWarning("[VEMLHandler->FromHex] Invalid hex color.");
                return new Color(0.5f, 0.5f, 0.5f, 0.5f);
            }
        }

        /// <summary>
        /// Sets up entity IDs for entities.
        /// </summary>
        /// <param name="entities">A reference to an array of entities to set up the IDs of. Will
        /// assign an ID to each entity for which one does not already exist.</param>
        private void SetUpEntityIDs(ref entity[] entities)
        {
            Queue<entity> entityQueue = new Queue<entity>();
            foreach (entity entity in entities)
            {
                entityQueue.Enqueue(entity);
            }

            while (entityQueue.Count > 0)
            {
                entity entity = entityQueue.Dequeue();
                if (entity.entity1 != null)
                {
                    foreach (entity child in entity.entity1)
                    {
                        entityQueue.Enqueue(child);
                    }
                }

                if (entity.id == null)
                {
                    entity.id = Guid.NewGuid().ToString();
                }
            }
        }

        /// <summary>
        /// Get all child entity relationships for the provided entities.
        /// </summary>
        /// <param name="entities">List of entities for which to get the child entities of.</param>
        /// <returns>A dictionary of the provided entities (and any children) and their parents.</returns>
        private Dictionary<entity, entity> GetAllChildEntities(entity[] entities)
        {
            Dictionary<entity, entity> foundEntities = new Dictionary<entity, entity>();
            foreach (entity entity in entities)
            {
                Dictionary<entity, entity> newEntities = GetAllChildEntities(entity);
                foreach (KeyValuePair<entity, entity> newEntity in newEntities)
                {
                    foundEntities.Add(newEntity.Key, newEntity.Value);
                }
            }
            return foundEntities;
        }

        /// <summary>
        /// Get all child entity relationships for the provided entity.
        /// </summary>
        /// <param name="entity">Entity for which to get the child entities of.</param>
        /// <returns>A dictionary of the provided entity (and any children) and their parents.</returns>
        private Dictionary<entity, entity> GetAllChildEntities(entity entity)
        {
            Dictionary<entity, entity> entities = new Dictionary<entity, entity>();

            Queue<KeyValuePair<entity, entity>> entityQueue = new Queue<KeyValuePair<entity, entity>>();
            entityQueue.Enqueue(new KeyValuePair<entity, entity>(entity, null));
            while (entityQueue.Count > 0)
            {
                KeyValuePair<entity, entity> item = entityQueue.Dequeue();
                if (item.Key.entity1 != null)
                {
                    foreach (entity child in item.Key.entity1)
                    {
                        entityQueue.Enqueue(new KeyValuePair<entity, entity>(child, item.Key));
                    }
                }
                entities.Add(item.Key, item.Value);
            }

            return entities;
        }

        /// <summary>
        /// Format a URI to be valid. Will replace single forward slashes in the protocol heading
        /// with double forward slashes and replace back slashes with forward slashes.
        /// </summary>
        /// <param name="unformattedURI">URI to format.</param>
        /// <returns>The formatted URI. Will replace single forward slashes in the protocol heading
        /// with double forward slashes and replace back slashes with forward slashes.</returns>
        private string FormatURI(string unformattedURI)
        {
            string uri = unformattedURI;
            if (uri.Contains("http:/") && !uri.Contains("http://"))
            {
                uri = uri.Replace("http:/", "http://");
            }
            if (uri.Contains("http:\\") && !uri.Contains("http:\\\\"))
            {
                uri = uri.Replace("http:\\", "http://");
            }
            if (uri.Contains("https:/") && !uri.Contains("https://"))
            {
                uri = uri.Replace("https:/", "https://");
            }
            if (uri.Contains("https:\\") && !uri.Contains("https:\\\\"))
            {
                uri = uri.Replace("https:\\", "https://");
            }
            uri = uri.Replace("\\", "/");
            return uri;
        }

        /// <summary>
        /// Attempts to return a fully qualified URI.
        /// </summary>
        /// <param name="rawURI"></param>
        /// <param name="uriBase"></param>
        /// <returns>If URI is already fully qualified, returns
        /// raw string. Otherwise, will prepend raw string with URI base.</returns>
        private string FullyQualifyURI(string rawURI, string uriBase)
        {
            if (Uri.IsWellFormedUriString(rawURI, UriKind.Absolute))
            {
                return rawURI;
            }
            else
            {
                return uriBase + "/" + rawURI;
            }
        }

        /// <summary>
        /// Apply a transform to an entity.
        /// </summary>
        /// <param name="entity">Entity to apply transform to.</param>
        /// <param name="tf">Transform to apply.</param>
        /// <param name="scaleTransformValid">Whether or not a scale transform will be treated as valid.</param>
        /// <param name="sizeTransformValid">Whether or not a size transform will be treated as valid.</param>
        /// <param name="canvasTransformValid">Whether or not a canvas transform will be treated as valid.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        private bool ApplyTransform(BaseEntity entity, basetransform tf,
            bool scaleTransformValid, bool sizeTransformValid, bool canvasTransformValid)
        {
            if (tf is scaletransform)
            {
                if (scaleTransformValid == false)
                {
                    LogSystem.LogWarning("[VEMLHandler->ApplyTransform] Scale Transform not allowed.");
                    return false;
                }

                entity.SetPosition(new Vector3((float) ((scaletransform) tf).position.x,
                    (float) ((scaletransform) tf).position.y, (float) ((scaletransform) tf).position.z), true, false);
                entity.SetRotation(new Quaternion((float) ((scaletransform) tf).rotation.x, (float) ((scaletransform) tf).rotation.y,
                    (float) ((scaletransform) tf).rotation.z, (float) ((scaletransform) tf).rotation.w), true, false);
                entity.SetScale(new Vector3((float) ((scaletransform) tf).scale.x,
                    (float) ((scaletransform) tf).scale.y, (float) ((scaletransform) tf).scale.z), false);
            }
            else if (tf is sizetransform)
            {
                if (sizeTransformValid == false)
                {
                    LogSystem.LogWarning("[VEMLHandler->ApplyTransform] Size Transform not allowed.");
                    return false;
                }

                entity.SetPosition(new Vector3((float) ((sizetransform) tf).position.x,
                    (float) ((sizetransform) tf).position.y, (float) ((sizetransform) tf).position.z), true, false);
                entity.SetRotation(new Quaternion((float) ((sizetransform) tf).rotation.x, (float) ((sizetransform) tf).rotation.y,
                    (float) ((sizetransform) tf).rotation.z, (float) ((sizetransform) tf).rotation.w), true, false);
                entity.SetSize(new Vector3((float) ((sizetransform) tf).size.x,
                    (float) ((sizetransform) tf).size.y, (float) ((sizetransform) tf).size.z), false);
            }
            else if (tf is canvastransform)
            {
                if (canvasTransformValid == false)
                {
                    LogSystem.LogWarning("[VEMLHandler->ApplyTransform] Canvas Transform not allowed.");
                    return false;
                }

                if (entity is not UIElementEntity)
                {
                    LogSystem.LogError("[VEMLHandler->ApplyTransform] Canvas Transform must be applied to UI Element Entity.");
                    return false;
                }

                ((UIElementEntity) entity).SetPositionPercent(new Vector2((float) ((canvastransform) tf).positionpercent.x,
                    (float) ((canvastransform) tf).positionpercent.y), false);
                ((UIElementEntity) entity).SetSizePercent(new Vector3((float) ((canvastransform) tf).sizepercent.x,
                    (float) ((canvastransform) tf).sizepercent.y), false);
            }
            else
            {
                LogSystem.LogWarning("[VEMLHandler->ApplyTransform] Unknown transform type.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Parse VEML-compliant CSV formatted heights.
        /// </summary>
        /// <param name="csvHeights">VEML-compliant CSV formatted heights.</param>
        /// <returns>2D float array representation of the VEML-compliant CSV formatted heights.</returns>
        private float[,] ParseCSVHeights(string csvHeights)
        {
            int numCols = 0;
            string[] rows = csvHeights.Split(';');
            int numRows = rows.Length;
            foreach (string row in rows)
            {
                string[] cols = row.Split(",");
                int colLength = cols.Length;
                if (colLength > numCols)
                {
                    colLength = numCols;
                }
            }

            float[,] heights = new float[numRows, numCols];
            for (int i = 0; i < numRows; i++)
            {
                string[] cols = rows[i].Split(",");
                for (int j = 0; j < cols.Length; j++)
                {
                    heights[i, j] = float.Parse(cols[j]);
                }
            }

            return heights;
        }
    }
}
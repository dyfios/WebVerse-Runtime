// Copyright (c) 2019-2024 Five Squared Interactive. All rights reserved.

using FiveSQD.WebVerse.Runtime;
using FiveSQD.WebVerse.Utilities;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Vuplex.WebView;

namespace FiveSQD.WebVerse.Input.Focused
{
    /// <summary>
    /// Class for the hand menu controller.
    /// </summary>
    public class HandMenuController : BaseController
    {
        /// <summary>
        /// Main camera.
        /// </summary>
        [Tooltip("Main camera.")]
        public Camera mainCamera;

        /// <summary>
        /// Menu object.
        /// </summary>
        [Tooltip("Menu object.")]
        public GameObject menuObject;

        /// <summary>
        /// Menu distance in meters.
        /// </summary>
        [Tooltip("Menu distance in meters.")]
        public float menuDistance;

        /// <summary>
        /// Path to the History Menu HTML.
        /// </summary>
        [Tooltip("Path to the History Menu HTML.")]
        public string historyMenuHTMLPath;

        /// <summary>
        /// Path to the Settings Menu HTML.
        /// </summary>
        [Tooltip("Path to the Settings Menu HTML.")]
        public string settingsMenuHTMLPath;

        /// <summary>
        /// Path to the About Menu HTML.
        /// </summary>
        [Tooltip("Path to the About Menu HTML.")]
        public string aboutMenuHTMLPath;

        /// <summary>
        /// Menu Grid.
        /// </summary>
        [Tooltip("Menu Grid.")]
        public GameObject menuGrid;

        /// <summary>
        /// Mode Menu.
        /// </summary>
        [Tooltip("Mode Menu.")]
        public GameObject modeMenu;

        /// <summary>
        /// Exit Menu.
        /// </summary>
        [Tooltip("Exit Menu.")]
        public GameObject exitMenu;

        /// <summary>
        /// WebView Object.
        /// </summary>
        [Tooltip("WebView Object.")]
        public GameObject webViewObject;

        /// <summary>
        /// Back Button Object.
        /// </summary>
        [Tooltip("Back Button Object.")]
        public GameObject backButtonObject;

        /// <summary>
        /// Canvas WebView Prefab.
        /// </summary>
        [Tooltip("Canvas WebView Prefab.")]
        public CanvasWebViewPrefab webView;

        /// <summary>
        /// MultiBar Controller.
        /// </summary>
        [Tooltip("MultiBar Controller.")]
        public MultiBarController multiBarController;

        /// <summary>
        /// String containing history information.
        /// </summary>
        private string historyString;

        /// <summary>
        /// URL load queue.
        /// </summary>
        private Queue<string> urlsToLoad;

        /// <summary>
        /// Whether or not WebView is initialized.
        /// </summary>
        private bool webViewInitialized;

        /// <summary>
        /// Initialize the VR Menu Controller.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            urlsToLoad = new Queue<string>();
            multiBarController.Initialize();
            webViewInitialized = false;
        }

        /// <summary>
        /// Terminate the VR Menu Controller.
        /// </summary>
        public override void Terminate()
        {
            multiBarController.Terminate();
            base.Terminate();
        }

        /// <summary>
        /// Toggle the VR Menu.
        /// </summary>
        public void ToggleMenu()
        {
            menuObject.SetActive(!menuObject.activeSelf);

            if (menuObject.activeSelf)
            {
                //menuObject.transform.position = mainCamera.transform.TransformPoint(Vector3.forward * menuDistance);
                OpenMenuGrid();
            }
        }

        /// <summary>
        /// Open the Menu Grid.
        /// </summary>
        public void OpenMenuGrid()
        {
            menuGrid.SetActive(true);
            modeMenu.SetActive(false);
            exitMenu.SetActive(false);
            webViewObject.SetActive(false);
            backButtonObject.SetActive(false);
        }

        /// <summary>
        /// Close the Menu Grid.
        /// </summary>
        public void CloseMenuGrid()
        {
            menuGrid.SetActive(false);
            modeMenu.SetActive(false);
            exitMenu.SetActive(false);
            webViewObject.SetActive(false);
            backButtonObject.SetActive(false);
        }

        /// <summary>
        /// Toggle the Menu Grid.
        /// </summary>
        public void ToggleMenuGrid()
        {
            if (menuGrid.activeSelf)
            {
                CloseMenuGrid();
            }
            else
            {
                OpenMenuGrid();
            }
        }

        /// <summary>
        /// Open the mode menu.
        /// </summary>
        public void OpenModeMenu()
        {
            menuGrid.SetActive(false);
            modeMenu.SetActive(true);
            exitMenu.SetActive(false);
            webViewObject.SetActive(false);
            backButtonObject.SetActive(true);
        }

        /// <summary>
        /// Open the history menu.
        /// </summary>
        public void OpenHistoryMenu()
        {
            menuGrid.SetActive(false);
            modeMenu.SetActive(false);
            exitMenu.SetActive(false);
            webViewObject.SetActive(true);
            if (string.IsNullOrEmpty(historyString))
            {
                urlsToLoad.Enqueue(Path.Combine(Application.dataPath, historyMenuHTMLPath + "?history=[]"));
            }
            else
            {
                urlsToLoad.Enqueue(Path.Combine(Application.dataPath, historyMenuHTMLPath + "?history="
                    + historyString.Replace("\"", "&34")).Replace("&34", "\""));
            }
            backButtonObject.SetActive(true);
        }

        /// <summary>
        /// Open the settings menu.
        /// </summary>
        public void OpenSettingsMenu()
        {
            menuGrid.SetActive(false);
            modeMenu.SetActive(false);
            exitMenu.SetActive(false);
            webViewObject.SetActive(true);
            urlsToLoad.Enqueue(Path.Combine(Application.dataPath, settingsMenuHTMLPath + "?storage_entries="
                + Runtime.WebVerseRuntime.Instance.localStorageManager.maxEntries + "&storage_key_length="
                + Runtime.WebVerseRuntime.Instance.localStorageManager.maxKeyLength + "&storage_entry_length="
                + Runtime.WebVerseRuntime.Instance.localStorageManager.maxEntryLength + "&daemon_port="
                + (Runtime.WebVerseRuntime.Instance.webVerseDaemonManager == null ? ""
                : Runtime.WebVerseRuntime.Instance.webVerseDaemonManager.port)));
            backButtonObject.SetActive(true);
        }

        /// <summary>
        /// Open the about menu.
        /// </summary>
        public void OpenAboutMenu()
        {
            menuGrid.SetActive(false);
            modeMenu.SetActive(false);
            exitMenu.SetActive(false);
            webViewObject.SetActive(true);
            urlsToLoad.Enqueue(Path.Combine(Application.dataPath, aboutMenuHTMLPath));
            backButtonObject.SetActive(true);
        }

        /// <summary>
        /// Open the exit menu.
        /// </summary>
        public void OpenExitMenu()
        {
            menuGrid.SetActive(false);
            modeMenu.SetActive(false);
            exitMenu.SetActive(true);
            webViewObject.SetActive(false);
            backButtonObject.SetActive(true);
        }

        /// <summary>
        /// Update the history.
        /// </summary>
        /// <param name="history">History string to use.</param>
        public void UpdateHistory(string history)
        {
            historyString = history;
        }

        /// <summary>
        /// Close WebVerse.
        /// </summary>
        public void CloseWebVerse()
        {
            WebVerseRuntime.Instance.webVerseDaemonManager.SendCloseRequest();
        }

        /// <summary>
        /// Change the tab mode.
        /// </summary>
        /// <param name="runtimeType">The runtime type to switch to:
        ///                           - teardown: Return to tabbed browser.
        ///                           - desktop:  Open in Desktop mode.
        ///                           - steamvr:  Open in SteamVR mode.</param>
        public void ChangeTabMode(string runtimeType)
        {
            WebVerseRuntime.Instance.webVerseDaemonManager.SendFocusedTabRequest(
                WebVerseRuntime.Instance.currentURL, runtimeType);
        }

        private void Update()
        {
#if VUPLEX_INCLUDED
            if (webView != null && webView.WebView != null)
            {
                if (webViewInitialized == false)
                {
                    webViewInitialized = true;
                    webView.WebView.MessageEmitted += (sender, eventArgs) =>
                    {
                        if (eventArgs.Value.StartsWith("WEBVERSE.INTERNAL.LOADURL."))
                        {
                            WebVerseRuntime.Instance.LoadURL(eventArgs.Value.Replace("WEBVERSE.INTERNAL.LOADURL.", ""));
                        }
                        else if (eventArgs.Value.StartsWith("WEBVERSE.INTERNAL.UPDATESETTINGS."))
                        {
                            string shortenedString = eventArgs.Value.Replace("WEBVERSE.INTERNAL.UPDATESETTINGS.", "");
                            string[] parms = shortenedString.Split(".");
                            if (parms.Length == 3)
                            {
                                WebVerseRuntime.Instance.webVerseDaemonManager.SendSettingsUpdateRequest(
                                    int.Parse(parms[0]), int.Parse(parms[1]), int.Parse(parms[2]));
                            }
                        }
                    };
                }

                if (urlsToLoad.Count > 0)
                {
                    webView.WebView.LoadUrl(urlsToLoad.Dequeue());
                }
            }
#endif
        }
    }
}
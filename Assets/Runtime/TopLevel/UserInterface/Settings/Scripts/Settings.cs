// Copyright (c) 2019-2024 Five Squared Interactive. All rights reserved.

using UnityEngine;
using TMPro;
using FiveSQD.WebVerse.Runtime;
using FiveSQD.WebVerse.Utilities;

namespace FiveSQD.WebVerse.Interface.Settings
{
    /// <summary>
    /// Class for settings menu.
    /// </summary>
    public class Settings : MonoBehaviour
    {
        /// <summary>
        /// Desktop mode script.
        /// </summary>
        public DesktopMode desktopMode;

        /// <summary>
        /// Home URL input.
        /// </summary>
        public TMP_InputField homeURLInput;

        /// <summary>
        /// Storage mode dropdown.
        /// </summary>
        public TMP_Dropdown storageModeDropdown;

        /// <summary>
        /// Max storage entries input.
        /// </summary>
        public TMP_InputField maxStorageEntriesInput;

        /// <summary>
        /// Max storage key length input.
        /// </summary>
        public TMP_InputField maxStorageKeyLengthInput;

        /// <summary>
        /// Max storage entry length input.
        /// </summary>
        public TMP_InputField maxStorageEntryLengthInput;

        /// <summary>
        /// Cache directory input.
        /// </summary>
        public TMP_InputField cacheDirectoryInput;

        /// <summary>
        /// World load timeout input.
        /// </summary>
        public TMP_InputField worldLoadTimeoutInput;

        /// <summary>
        /// Initialize settings menu.
        /// </summary>
        public void Initialize()
        {
            homeURLInput.text = desktopMode.desktopSettings.GetHomeURL();
            string storageMode = desktopMode.desktopSettings.GetStorageMode();
            if (storageMode == "persistent")
            {
                storageModeDropdown.value = 0;
            }
            else if (storageMode == "cache")
            {
                storageModeDropdown.value = 1;
            }
            else
            {
                Logging.LogError("[Settings->Initialize] Invalid Storage Mode.");
            }
            maxStorageEntriesInput.text = desktopMode.desktopSettings.GetMaxStorageEntries().ToString();
            maxStorageKeyLengthInput.text = desktopMode.desktopSettings.GetMaxStorageKeyLength().ToString();
            maxStorageEntryLengthInput.text = desktopMode.desktopSettings.GetMaxStorageEntryLength().ToString();
            cacheDirectoryInput.text = desktopMode.desktopSettings.GetCacheDirectory();
            worldLoadTimeoutInput.text = desktopMode.desktopSettings.GetWorldLoadTimeout().ToString();
        }

        /// <summary>
        /// Terminate settings menu.
        /// </summary>
        public void Terminate()
        {

        }

        /// <summary>
        /// Return from settings menu.
        /// </summary>
        public void Return()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Update home.
        /// </summary>
        /// <param name="home">New home to use.</param>
        public void UpdateHome(string home)
        {
            desktopMode.desktopSettings.SetHomeURL(home);
        }

        /// <summary>
        /// Update storage mode.
        /// </summary>
        /// <param name="storageMode">New storage mode to use.</param>
        public void UpdateStorageMode(int storageMode)
        {
            string modeString;
            if (storageMode == 0)
            {
                modeString = "persistent";
            }
            else if (storageMode == 1)
            {
                modeString = "cache";
            }
            else
            {
                Logging.LogError("[Settings->UpdateStorageMode] Invalid storage mode.");
                return;
            }
            desktopMode.desktopSettings.SetStorageMode(modeString);
        }

        /// <summary>
        /// Update max entries.
        /// </summary>
        /// <param name="maxEntries">New max entries to use.</param>
        public void UpdateMaxEntries(string maxEntries)
        {
            uint maxEntriesVal = uint.Parse(maxEntries);
            desktopMode.desktopSettings.SetMaxStorageEntries(maxEntriesVal);
        }

        /// <summary>
        /// Update max key length.
        /// </summary>
        /// <param name="maxKeyLen">New max key length to use.</param>
        public void UpdateMaxKeyLen(string maxKeyLen)
        {
            uint maxKeyLenVal = uint.Parse(maxKeyLen);
            desktopMode.desktopSettings.SetMaxStorageKeyLength(maxKeyLenVal);
        }

        /// <summary>
        /// Update max entry length.
        /// </summary>
        /// <param name="maxEntryLen">New max entry length to use.</param>
        public void UpdateMaxEntryLen(string maxEntryLen)
        {
            uint maxEntryLenVal = uint.Parse(maxEntryLen);
            desktopMode.desktopSettings.SetMaxStorageEntryLength(maxEntryLenVal);
        }

        /// <summary>
        /// Update cache directory.
        /// </summary>
        /// <param name="cacheDirectory">New cache directory to use.</param>
        public void UpdateCacheDirectory(string cacheDirectory)
        {
            desktopMode.desktopSettings.SetCacheDirectory(cacheDirectory);
        }

        /// <summary>
        /// Update world load timeout.
        /// </summary>
        /// <param name="worldLoadTimeout">New world load timeout to use.</param>
        public void UpdateWorldLoadTimeout(string worldLoadTimeout)
        {
            uint worldLoadTimeoutVal = uint.Parse(worldLoadTimeout);
            desktopMode.desktopSettings.SetWorldLoadTimeout(worldLoadTimeoutVal);
        }
    }
}
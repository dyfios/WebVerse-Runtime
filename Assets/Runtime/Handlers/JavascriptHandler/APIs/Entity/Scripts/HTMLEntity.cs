// Copyright (c) 2019-2024 Five Squared Interactive. All rights reserved.

using System;
using FiveSQD.WebVerse.Runtime;
using FiveSQD.WebVerse.Utilities;
using FiveSQD.WebVerse.Handlers.Javascript.APIs.WorldTypes;

namespace FiveSQD.WebVerse.Handlers.Javascript.APIs.Entity
{
    public class HTMLEntity : BaseEntity
    {
        /// <summary>
        /// Create an HTML entity.
        /// </summary>
        /// <param name="parent">Parent of the entity to create.</param>
        /// <param name="position">Position of the entity relative to its parent.</param>
        /// <param name="rotation">Rotation of the entity relative to its parent.</param>
        /// <param name="scale">Scale of the entity relative to its parent.</param>
        /// <param name="isSize">Whether or not the scale parameter is a size.</param>
        /// <param name="id">ID of the entity. One will be created if not provided.</param>
        /// <param name="tag">Tag of the entity.</param>
        /// <param name="onMessage">Action to invoke upon receiving a world message.</param>
        /// <param name="onLoaded">Action to perform on load. This takes a single parameter containing the created
        /// HTML entity object.</param>
        /// <returns>The ID of the HTML entity object.</returns>
        public static HTMLEntity Create(BaseEntity parent,
            Vector3 position, Quaternion rotation, Vector3 scale, bool isSize = false,
            string id = null, string tag = null, string onMessage = null, string onLoaded = null)
        {
            Guid guid;
            if (string.IsNullOrEmpty(id))
            {
                guid = Guid.NewGuid();
            }
            else
            {
                guid = Guid.Parse(id);
            }

            WorldEngine.Entity.BaseEntity pBE = EntityAPIHelper.GetPrivateEntity(parent);
            UnityEngine.Vector3 pos = new UnityEngine.Vector3(position.x, position.y, position.z);
            UnityEngine.Quaternion rot = new UnityEngine.Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);
            UnityEngine.Vector3 scl = new UnityEngine.Vector3(scale.x, scale.y, scale.z);

            HTMLEntity he = new HTMLEntity(false);

            Action<string> onMessageAction = null;
            if (onMessage != null)
            {
                onMessageAction = (msg) =>
                {
                    WebVerseRuntime.Instance.javascriptHandler.Run(onMessage.Replace("?", "'" + msg + "'"));
                };
            }

            Action onLoadAction = null;
            onLoadAction = () =>
            {
                he.internalEntity = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(guid);
                EntityAPIHelper.AddEntityMapping(he.internalEntity, he);
                if (!string.IsNullOrEmpty(onLoaded))
                {
                    WebVerseRuntime.Instance.javascriptHandler.Run(onLoaded.Replace("?", "ce"));
                }
            };

            WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadHTMLEntity(pBE, pos, rot, scl, guid, isSize, tag,
                onMessageAction, onLoadAction);

            return he;
        }

        /// <summary>
        /// Create an HTML entity.
        /// </summary>
        /// <param name="parent">Parent of the entity to create.</param>
        /// <param name="positionPercent">Position of the entity as a percentage of its parent canvas.</param>
        /// <param name="sizePercent">Size of the entity as a percentage of its parent canvas.</param>
        /// <param name="id">ID of the entity. One will be created if not provided.</param>
        /// <param name="tag">Tag of the entity.</param>
        /// <param name="onMessage">Action to invoke upon receiving a world message.</param>
        /// <param name="onLoaded">Action to perform on load. This takes a single parameter containing the created
        /// HTML entity object.</param>
        /// <returns>The ID of the HTML entity object.</returns>
        public static HTMLEntity Create(BaseEntity parent, Vector2 positionPercent, Vector2 sizePercent,
            string id = null, string tag = null, string onMessage = null, string onLoaded = null)
        {
            Guid guid;
            if (string.IsNullOrEmpty(id))
            {
                guid = Guid.NewGuid();
            }
            else
            {
                guid = Guid.Parse(id);
            }

            WorldEngine.Entity.CanvasEntity pCE = (WorldEngine.Entity.CanvasEntity) EntityAPIHelper.GetPrivateEntity(parent);
            if (pCE == null)
            {
                Logging.LogWarning("[HTMLEntity->Create] Invalid parent entity.");
                return null;
            }

            UnityEngine.Vector2 pos = new UnityEngine.Vector2(positionPercent.x, positionPercent.y);
            UnityEngine.Vector2 scl = new UnityEngine.Vector2(sizePercent.x, sizePercent.y);

            HTMLEntity he = new HTMLEntity(true);

            Action<string> onMessageAction = null;
            if (onMessage != null)
            {
                onMessageAction = (msg) =>
                {
                    WebVerseRuntime.Instance.javascriptHandler.Run(onMessage.Replace("?", "'" + msg + "'"));
                };
            }

            Action onLoadAction = null;
            onLoadAction = () =>
            {
                he.internalEntity = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(guid);
                EntityAPIHelper.AddEntityMapping(he.internalEntity, he);
                if (!string.IsNullOrEmpty(onLoaded))
                {
                    WebVerseRuntime.Instance.javascriptHandler.Run(onLoaded.Replace("?", "ce"));
                }
            };
            
            WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadHTMLUIElementEntity(pCE, pos, scl, guid, tag,
                onMessageAction, onLoadAction);

            return he;
        }

        internal HTMLEntity(bool canvasEntity)
        {
            if (canvasEntity)
            {
                internalEntityType = typeof(WorldEngine.Entity.HTMLUIElementEntity);
            }
            else
            {
                internalEntityType = typeof(WorldEngine.Entity.HTMLEntity);
            }
        }

        /// <summary>
        /// Load content from a URL.
        /// </summary>
        /// <param name="url">URL to load content from.</param>
        /// <returns>Whether or not the setting was successful.</returns>
        public bool LoadFromURL(string url)
        {
            if (IsValid() == false)
            {
                Logging.LogError("[HTMLEntity:LoadFromURL] Unknown entity.");
                return false;
            }

            if (internalEntity is WorldEngine.Entity.HTMLEntity)
            {
                return ((WorldEngine.Entity.HTMLEntity) internalEntity).LoadFromURL(
                    VEML.VEMLUtilities.FullyQualifyURI(url, WebVerseRuntime.Instance.currentBasePath));
            }
            else if (internalEntity is WorldEngine.Entity.HTMLUIElementEntity)
            {
                return ((WorldEngine.Entity.HTMLUIElementEntity) internalEntity).LoadFromURL(
                    VEML.VEMLUtilities.FullyQualifyURI(url, WebVerseRuntime.Instance.currentBasePath));
            }
            else
            {
                Logging.LogError("[HTMLEntity:LoadFromURL] Invalid internal entity.");
                return false;
            }
        }

        /// <summary>
        /// Load content from a URL.
        /// </summary>
        /// <param name="url">URL to load content from.</param>
        /// <returns>Whether or not the setting was successful.</returns>
        public bool LoadHTML(string html)
        {
            if (IsValid() == false)
            {
                Logging.LogError("[HTMLEntity:LoadHTML] Unknown entity.");
                return false;
            }

            if (internalEntity is WorldEngine.Entity.HTMLEntity)
            {
                return ((WorldEngine.Entity.HTMLEntity) internalEntity).LoadHTML(html);
            }
            else if (internalEntity is WorldEngine.Entity.HTMLUIElementEntity)
            {
                return ((WorldEngine.Entity.HTMLUIElementEntity) internalEntity).LoadHTML(html);
            }
            else
            {
                Logging.LogError("[HTMLEntity:LoadHTML] Invalid internal entity.");
                return false;
            }
        }

        /// <summary>
        /// Get the current URL.
        /// </summary>
        /// <returns>The current URL, or null.</returns>
        public string GetURL()
        {
            if (IsValid() == false)
            {
                Logging.LogError("[HTMLEntity:GetURL] Unknown entity.");
                return null;
            }

            if (internalEntity is WorldEngine.Entity.HTMLEntity)
            {
                return ((WorldEngine.Entity.HTMLEntity) internalEntity).GetURL();
            }
            else if (internalEntity is WorldEngine.Entity.HTMLUIElementEntity)
            {
                return ((WorldEngine.Entity.HTMLUIElementEntity) internalEntity).GetURL();
            }
            else
            {
                Logging.LogError("[HTMLEntity:GetURL] Invalid internal entity.");
                return null;
            }
        }

        /// <summary>
        /// Execute JavaScript logic.
        /// </summary>
        /// <param name="logic">Logic to execute.</param>
        /// <param name="onComplete">Action to invoke upon completion. Provides return
        /// from JavaScript as string.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public bool ExecuteJavaScript(string logic, string onComplete)
        {
            if (IsValid() == false)
            {
                Logging.LogError("[HTMLEntity:ExecuteJavaScript] Unknown entity.");
                return false;
            }

            Action<string> onCompleteAction = null;
            if (!string.IsNullOrEmpty(onComplete))
            {
                onCompleteAction = (result) =>
                {
                    if (WebVerseRuntime.Instance.inputManager.inputEnabled)
                    {
                        WebVerseRuntime.Instance.javascriptHandler.Run(onComplete.Replace("?", result));
                    }
                };
            }
            
            if (internalEntity is WorldEngine.Entity.HTMLEntity)
            {
                return ((WorldEngine.Entity.HTMLEntity) internalEntity).ExecuteJavaScript(logic, onCompleteAction);
            }
            else if (internalEntity is WorldEngine.Entity.HTMLUIElementEntity)
            {
                return ((WorldEngine.Entity.HTMLUIElementEntity) internalEntity).ExecuteJavaScript(logic, onCompleteAction);
            }
            else
            {
                Logging.LogError("[HTMLEntity:ExecuteJavaScript] Invalid internal entity.");
                return false;
            }
        }
    }
}
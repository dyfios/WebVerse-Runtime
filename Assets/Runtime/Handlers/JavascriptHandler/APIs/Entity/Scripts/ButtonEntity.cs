// Copyright (c) 2019-2024 Five Squared Interactive. All rights reserved.

using System;
using FiveSQD.WebVerse.Runtime;
using FiveSQD.WebVerse.Utilities;
using FiveSQD.WebVerse.Handlers.Javascript.APIs.WorldTypes;

namespace FiveSQD.WebVerse.Handlers.Javascript.APIs.Entity
{
    /// <summary>
    /// Class for a button entity.
    /// </summary>
    public class ButtonEntity : BaseEntity
    {
        /// <summary>
        /// Create a button entity.
        /// </summary>
        /// <param name="parent">Parent canvas of the entity to create.</param>
        /// <param name="onClick">Action to perform on button click.</param>
        /// <param name="positionPercent">Position of the entity within its canvas.</param>
        /// <param name="sizePercent">Size of the entity relative to its canvas.</param>
        /// <param name="id">ID of the entity. One will be created if not provided.</param>
        /// <param name="tag">Tag of the entity.</param>
        /// <param name="onLoaded">Action to perform on load. This takes a single parameter containing the created
        /// button entity object.</param>
        /// <returns>The ID of the button entity object.</returns>
        public static ButtonEntity Create(CanvasEntity parent, string onClick,
            Vector2 positionPercent, Vector2 sizePercent,
            string id = null, string tag = null, string onLoaded = null)
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
                Logging.LogWarning("[ButtonEntity->Create] Invalid parent entity.");
                return null;
            }

            UnityEngine.Vector2 pos = new UnityEngine.Vector2(positionPercent.x, positionPercent.y);
            UnityEngine.Vector2 size = new UnityEngine.Vector2(sizePercent.x, sizePercent.y);

            ButtonEntity be = new ButtonEntity();

            System.Action onClickAction = null;
            if (!string.IsNullOrEmpty(onClick))
            {
                onClickAction = () =>
                {
                    if (WebVerseRuntime.Instance.inputManager.inputEnabled)
                    {
                        WebVerseRuntime.Instance.javascriptHandler.Run(onClick);
                    }
                };
            }

            System.Action onLoadAction = null;
            onLoadAction = () =>
            {
                be.internalEntity = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(guid);
                EntityAPIHelper.AddEntityMapping(be.internalEntity, be);
                if (!string.IsNullOrEmpty(onLoaded))
                {
                    WebVerseRuntime.Instance.javascriptHandler.CallWithParams(onLoaded, new object[] { be });
                }
            };

            WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadButtonEntity(pCE, pos, size, onClickAction, guid, tag, onLoadAction);

            return be;
        }

        internal ButtonEntity()
        {
            internalEntityType = typeof(WorldEngine.Entity.ButtonEntity);
        }

        /// <summary>
        /// Set the onClick event for the button entity.
        /// </summary>
        /// <param name="onClick">Action to perform on click.</param>
        /// <returns>Whether or not the setting was successful.</returns>
        public bool SetOnClick(string onClick)
        {
            if (IsValid() == false)
            {
                Logging.LogError("[ButtonEntity:SetOnClick] Unknown entity.");
                return false;
            }

            System.Action onClickAction = null;
            if (!string.IsNullOrEmpty(onClick))
            {
                onClickAction = () =>
                {
                    if (WebVerseRuntime.Instance.inputManager.inputEnabled)
                    {
                        WebVerseRuntime.Instance.javascriptHandler.Run(onClick);
                    }
                };
            }

            ((WorldEngine.Entity.ButtonEntity) internalEntity).SetOnClick(onClickAction);

            return true;
        }
    }
}
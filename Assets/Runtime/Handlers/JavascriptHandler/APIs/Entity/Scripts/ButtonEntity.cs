// Copyright (c) 2019-2023 Five Squared Interactive. All rights reserved.

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
        public static System.Guid Create(CanvasEntity parent, string onClick,
            Vector2 positionPercent, Vector2 sizePercent,
            System.Guid? id = null, string tag = null, string onLoaded = null)
        {
            WorldEngine.Entity.CanvasEntity pCE = (WorldEngine.Entity.CanvasEntity) EntityAPIHelper.GetPrivateEntity(parent);
            UnityEngine.Vector2 pos = new UnityEngine.Vector2(positionPercent.x, positionPercent.y);
            UnityEngine.Vector2 size = new UnityEngine.Vector2(sizePercent.x, sizePercent.y);

            ButtonEntity be = new ButtonEntity();

            System.Action onClickAction = null;
            if (!string.IsNullOrEmpty(onClick))
            {
                onClickAction = () =>
                {
                    WebVerseRuntime.Instance.javascriptHandler.Run(onClick);
                };
            }

            System.Action onLoadAction = null;
            if (!string.IsNullOrEmpty(onLoaded))
            {
                onLoadAction = () =>
                {
                    if (id.HasValue == false)
                    {
                        Logging.LogError("[ButtonEntity:Create] Unable to finish entity creation.");
                    }
                    else
                    {
                        be.internalEntity = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(id.Value);
                        EntityAPIHelper.AddEntityMapping(be.internalEntity, be);
                        WebVerseRuntime.Instance.javascriptHandler.Run(onLoaded.Replace("?", "be"));
                    }
                };
            }

            return WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadButtonEntity(pCE, pos, size, onClickAction, id, tag, onLoadAction);
        }

        internal ButtonEntity()
        {
            internalEntityType = typeof(ButtonEntity);
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
                    WebVerseRuntime.Instance.javascriptHandler.Run(onClick);
                };
            }

            ((WorldEngine.Entity.ButtonEntity) internalEntity).SetOnClick(onClickAction);

            return true;
        }
    }
}
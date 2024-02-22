// Copyright (c) 2019-2024 Five Squared Interactive. All rights reserved.

using FiveSQD.WebVerse.Runtime;
using FiveSQD.WebVerse.Handlers.Javascript.APIs.WorldTypes;
using System;

namespace FiveSQD.WebVerse.Handlers.Javascript.APIs.Entity
{
    /// <summary>
    /// Class for a character entity.
    /// </summary>
    public class CharacterEntity : BaseEntity
    {
        /// <summary>
        /// Create a character entity.
        /// </summary>
        /// <param name="parent">Parent of the entity to create.</param>
        /// <param name="position">Position of the entity relative to its parent.</param>
        /// <param name="rotation">Rotation of the entity relative to its parent.</param>
        /// <param name="scale">Scale of the entity relative to its parent.</param>
        /// <param name="isSize">Whether or not the scale parameter is a size.</param>
        /// <param name="tag">Tag of the character entity.</param>
        /// <param name="id">ID of the entity. One will be created if not provided.</param>
        /// <param name="onLoaded">Action to perform on load. This takes a single parameter containing the created
        /// character entity object.</param>
        /// <returns>The character entity object.</returns>
        public static CharacterEntity Create(BaseEntity parent, 
            Vector3 position, Quaternion rotation, Vector3 scale, bool isSize = false,
            string tag = null, string id = null, string onLoaded = null)
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

            CharacterEntity ce = new CharacterEntity();

            System.Action onLoadAction = null;
            onLoadAction = () =>
            {
                ce.internalEntity = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(guid);
                EntityAPIHelper.AddEntityMapping(ce.internalEntity, ce);
                if (!string.IsNullOrEmpty(onLoaded))
                {
                    WebVerseRuntime.Instance.javascriptHandler.Run(onLoaded.Replace("?", "ce"));
                }
            };

            WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadCharacterEntity(pBE, pos, rot, scl, guid, tag, isSize, onLoadAction);

            return ce;
        }

        internal CharacterEntity()
        {
            internalEntityType = typeof(CharacterEntity);
        }
    }
}
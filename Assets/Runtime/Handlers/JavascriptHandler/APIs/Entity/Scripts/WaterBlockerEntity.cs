// Copyright (c) 2019-2024 Five Squared Interactive. All rights reserved.

using System;
using FiveSQD.WebVerse.Runtime;
using FiveSQD.WebVerse.Handlers.Javascript.APIs.WorldTypes;

namespace FiveSQD.WebVerse.Handlers.Javascript.APIs.Entity
{
    /// <summary>
    /// Class for a water blocker entity.
    /// </summary>
    public class WaterBlockerEntity : BaseEntity
    {
        /// <summary>
        /// Create a water blocker entity.
        /// </summary>
        /// <param name="parent">Parent of the entity to create.</param>
        /// <param name="position">Position of the entity relative to its parent.</param>
        /// <param name="rotation">Rotation of the entity relative to its parent.</param>
        /// <param name="scale">Scale of the entity relative to its parent.</param>
        /// <param name="id">ID of the entity. One will be created if not provided.</param>
        /// <param name="tag">Tag of the entity.</param>
        /// <param name="onLoaded">Action to perform on load. This takes a single parameter containing the created
        /// water entity object.</param>
        /// <returns>The ID of the water blocker entity object.</returns>
        public static WaterBlockerEntity CreateWaterBlocker(BaseEntity parent,
            Vector3 position, Quaternion rotation, Vector3 scale, string id = null, string tag = null,
            string onLoaded = null)
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

            WaterBlockerEntity we = new WaterBlockerEntity();

            System.Action onLoadAction = null;
            onLoadAction = () =>
            {
                we.internalEntity = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(guid);
                EntityAPIHelper.AddEntityMapping(we.internalEntity, we);
                if (!string.IsNullOrEmpty(onLoaded))
                {
                    WebVerseRuntime.Instance.javascriptHandler.CallWithParams(onLoaded, new object[] { we });
                }
            };

            WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadWaterBlockerEntity(
                null, UnityEngine.Vector3.zero, UnityEngine.Quaternion.identity, guid, tag, onLoadAction);

            return we;
        }

        public WaterBlockerEntity()
        {
            internalEntityType = typeof(WorldEngine.Entity.WaterBlockerEntity);
        }
    }
}
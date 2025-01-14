// Copyright (c) 2019-2025 Five Squared Interactive. All rights reserved.

using System;
using FiveSQD.WebVerse.Runtime;
using FiveSQD.WebVerse.Handlers.Javascript.APIs.WorldTypes;

namespace FiveSQD.WebVerse.Handlers.Javascript.APIs.Entity
{
    /// <summary>
    /// Class for a container entity.
    /// </summary>
    public class ContainerEntity : BaseEntity
    {
        /// <summary>
        /// Create a container entity.
        /// </summary>
        /// <param name="parent">Parent of the entity to create.</param>
        /// <param name="position">Position of the entity relative to its parent.</param>
        /// <param name="rotation">Rotation of the entity relative to its parent.</param>
        /// <param name="scale">Scale of the entity relative to its parent.</param>
        /// <param name="isSize">Whether or not the scale parameter is a size.</param>
        /// <param name="tag">Tag of the container entity.</param>
        /// <param name="id">ID of the entity. One will be created if not provided.</param>
        /// <param name="onLoaded">Action to perform on load. This takes a single parameter containing the created
        /// container entity object.</param>
        /// <returns>The ID of the container entity object.</returns>
        public static ContainerEntity Create(BaseEntity parent,
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

            ContainerEntity ce = new ContainerEntity();

            System.Action onLoadAction = null;
                onLoadAction = () =>
                {
                    ce.internalEntity = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(guid);
                    EntityAPIHelper.AddEntityMapping(ce.internalEntity, ce);
                    if (ce.internalEntity != null)
                    {
                        ce.internalEntity.entityTag = tag;
                    }
                    if (!string.IsNullOrEmpty(onLoaded))
                    {
                        WebVerseRuntime.Instance.javascriptHandler.CallWithParams(onLoaded, new object[] { ce });
                    }
                };

            WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadContainerEntity(pBE, pos, rot, scl, guid, tag, isSize, onLoadAction);

            return ce;
        }

        internal ContainerEntity()
        {
            internalEntityType = typeof(WorldEngine.Entity.ContainerEntity);
        }
    }
}
// Copyright (c) 2019-2023 Five Squared Interactive. All rights reserved.

using FiveSQD.WebVerse.Runtime;
using FiveSQD.WebVerse.Utilities;
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
        public static System.Guid Create(BaseEntity parent,
            Vector3 position, Quaternion rotation, Vector3 scale, bool isSize = false,
            string tag = null, System.Guid? id = null, string onLoaded = null)
        {
            WorldEngine.Entity.BaseEntity pBE = EntityAPIHelper.GetPrivateEntity(parent);
            UnityEngine.Vector3 pos = new UnityEngine.Vector3(position.x, position.y, position.z);
            UnityEngine.Quaternion rot = new UnityEngine.Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);
            UnityEngine.Vector3 scl = new UnityEngine.Vector3(scale.x, scale.y, scale.z);

            ContainerEntity ce = new ContainerEntity();

            System.Action onLoadAction = null;
            if (!string.IsNullOrEmpty(onLoaded))
            {
                onLoadAction = () =>
                {
                    if (id.HasValue == false)
                    {
                        Logging.LogError("[ContainerEntity:Create] Unable to finish entity creation.");
                    }
                    else
                    {
                        ce.internalEntity = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(id.Value);
                        EntityAPIHelper.AddEntityMapping(ce.internalEntity, ce);
                        WebVerseRuntime.Instance.javascriptHandler.Run(onLoaded.Replace("?", "ce"));
                        if (ce.internalEntity != null)
                        {
                            ce.internalEntity.entityTag = tag;
                        }
                    }
                };
            }

            return WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadContainerEntity(pBE, pos, rot, scl, id, tag, isSize, onLoadAction);
        }

        internal ContainerEntity()
        {
            internalEntityType = typeof(CharacterEntity);
        }
    }
}
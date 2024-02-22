// Copyright (c) 2019-2024 Five Squared Interactive. All rights reserved.

using System;
using FiveSQD.WebVerse.Runtime;
using FiveSQD.WebVerse.Utilities;
using FiveSQD.WebVerse.Handlers.Javascript.APIs.WorldTypes;

namespace FiveSQD.WebVerse.Handlers.Javascript.APIs.Entity
{
    /// <summary>
    /// Class for a mesh entity.
    /// </summary>
    public class MeshEntity : BaseEntity
    {
        /// <summary>
        /// Create a mesh entity.
        /// </summary>
        /// <param name="parent">Parent of the entity to create.</param>
        /// <param name="meshObject">Path to the mesh object to load for this entity.</param>
        /// <param name="meshResources">Paths to mesh resources for this entity.</param>
        /// <param name="position">Position of the entity relative to its parent.</param>
        /// <param name="rotation">Rotation of the entity relative to its parent.</param>
        /// <param name="id">ID of the entity. One will be created if not provided.</param>
        /// <param name="onLoaded">Action to perform on load. This takes a single parameter containing the created
        /// mesh entity object.</param>
        /// <returns>The ID of the mesh entity object.</returns>
        public static MeshEntity Create(BaseEntity parent, string meshObject, string[] meshResources,
            Vector3 position, Quaternion rotation,
            string id = null, string onLoaded = null)
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

            MeshEntity me = new MeshEntity();

            System.Action<WorldEngine.Entity.MeshEntity> onEntityLoadedAction =
                new System.Action<WorldEngine.Entity.MeshEntity>((meshEntity) =>
            {
                if (meshEntity == null)
                {
                    Logging.LogError("[MeshEntity:Create] Error loading mesh entity.");
                }
                else
                {
                    meshEntity.SetParent(pBE);
                    meshEntity.SetPosition(pos, true);
                    meshEntity.SetRotation(rot, true);

                    me.internalEntity = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(guid);
                    EntityAPIHelper.AddEntityMapping(me.internalEntity, me);
                    if (!string.IsNullOrEmpty(onLoaded))
                    {
                        WebVerseRuntime.Instance.javascriptHandler.Run(onLoaded.Replace("?", "me"));
                    }
                }

            });

            WebVerseRuntime.Instance.gltfHandler.LoadGLTFResourceAsMeshEntity(
                meshObject, meshResources, guid, onEntityLoadedAction);

            return me;
        }

        internal MeshEntity()
        {
            internalEntityType = typeof(MeshEntity);
        }
    }
}
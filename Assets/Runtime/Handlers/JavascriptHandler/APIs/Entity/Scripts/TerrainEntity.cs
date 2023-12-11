// Copyright (c) 2019-2023 Five Squared Interactive. All rights reserved.

using System;
using FiveSQD.WebVerse.Runtime;
using FiveSQD.WebVerse.Handlers.Javascript.APIs.WorldTypes;

namespace FiveSQD.WebVerse.Handlers.Javascript.APIs.Entity
{
    /// <summary>
    /// Class for a terrain entity.
    /// </summary>
    public class TerrainEntity : BaseEntity
    {
        /// <summary>
        /// Create a terrain entity.
        /// </summary>
        /// <param name="parent">Parent of the entity to create.</param>
        /// <param name="length">Length of the terrain in terrain units.</param>
        /// <param name="width">Width of the terrain in terrain units.</param>
        /// <param name="height">Height of the terrain in terrain units.</param>
        /// <param name="heights">2D array of heights for the terrain.</param>
        /// <param name="position">Position of the entity relative to its parent.</param>
        /// <param name="rotation">Rotation of the entity relative to its parent.</param>
        /// <param name="scale">Scale of the entity relative to its parent.</param>
        /// <param name="isSize">Whether or not the scale parameter is a size.</param>
        /// <param name="id">ID of the entity. One will be created if not provided.</param>
        /// <param name="tag">Tag of the entity.</param>
        /// <param name="onLoaded">Action to perform on load. This takes a single parameter containing the created
        /// terrain entity object.</param>
        /// <returns>The ID of the terrain entity object.</returns>
        public static TerrainEntity Create(BaseEntity parent, float length, float width, float height, float[,] heights,
            Vector3 position, Quaternion rotation, Vector3 scale, bool isSize = false,
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

            WorldEngine.Entity.BaseEntity pBE = EntityAPIHelper.GetPrivateEntity(parent);
            UnityEngine.Vector3 pos = new UnityEngine.Vector3(position.x, position.y, position.z);
            UnityEngine.Quaternion rot = new UnityEngine.Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);
            UnityEngine.Vector3 scl = new UnityEngine.Vector3(scale.x, scale.y, scale.z);

            TerrainEntity te = new TerrainEntity();

            System.Action onLoadAction = null;
            onLoadAction = () =>
            {
                te.internalEntity = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(guid);
                EntityAPIHelper.AddEntityMapping(te.internalEntity, te);
                if (!string.IsNullOrEmpty(onLoaded))
                {
                    WebVerseRuntime.Instance.javascriptHandler.Run(onLoaded.Replace("?", "te"));
                }
            };

            WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadTerrainEntity(length, width, height, heights,
                pBE, pos, rot, scl, guid, isSize, tag, onLoadAction);

            return te;
        }

        internal TerrainEntity()
        {
            internalEntityType = typeof(TerrainEntity);
        }
    }
}
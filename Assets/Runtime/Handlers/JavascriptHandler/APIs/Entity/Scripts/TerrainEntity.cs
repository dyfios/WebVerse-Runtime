// Copyright (c) 2019-2024 Five Squared Interactive. All rights reserved.

using System;
using FiveSQD.WebVerse.Runtime;
using FiveSQD.WebVerse.Handlers.Javascript.APIs.WorldTypes;
using FiveSQD.WebVerse.Utilities;
using FiveSQD.WebVerse.WorldEngine.Entity;

namespace FiveSQD.WebVerse.Handlers.Javascript.APIs.Entity
{
    /// <summary>
    /// Class for a terrain entity.
    /// </summary>
    public class TerrainEntity : BaseEntity
    {
        /// <summary>
        /// Create a heightmap terrain entity.
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
        /// <returns>The heightmap terrain entity object.</returns>
        public static TerrainEntity CreateHeightmap(BaseEntity parent, float length, float width, float height, float[,] heights,
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

            Action onLoadAction = null;
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
                pBE, pos, rot, guid, tag, onLoadAction);

            return te;
        }

        /// <summary>
        /// Create a hybrid terrain entity.
        /// </summary>
        /// <param name="parent">Parent of the entity to create.</param>
        /// <param name="length">Length of the terrain in terrain units.</param>
        /// <param name="width">Width of the terrain in terrain units.</param>
        /// <param name="height">Height of the terrain in terrain units.</param>
        /// <param name="heights">2D array of heights for the terrain.</param>
        /// <param name="layers">Layers for the terrain.</param>
        /// <param name="layerMasks">Layer masks for the terrain.</param>
        /// <param name="position">Position of the entity relative to its parent.</param>
        /// <param name="rotation">Rotation of the entity relative to its parent.</param>
        /// <param name="scale">Scale of the entity relative to its parent.</param>
        /// <param name="isSize">Whether or not the scale parameter is a size.</param>
        /// <param name="id">ID of the entity. One will be created if not provided.</param>
        /// <param name="tag">Tag of the entity.</param>
        /// <param name="onLoaded">Action to perform on load. This takes a single parameter containing the created
        /// terrain entity object.</param>
        /// <returns>The hybrid terrain entity object.</returns>
        public static TerrainEntity CreateHybrid(BaseEntity parent, float length, float width, float height, float[][] heights,
            TerrainEntityLayer[] layers, TerrainEntityLayerMaskCollection layerMasks, Vector3 position, Quaternion rotation,
            Vector3 scale, bool isSize = false, string id = null, string tag = null, string onLoaded = null)
        {
            return EntityAPIHelper.LoadHybridTerrainEntityAsync(parent, length, width, height, heights,
                layers, layerMasks, position, rotation, scale, isSize, id, tag, onLoaded);
        }

        public static void Test()
        {

        }

        /// <summary>
        /// Build on the terrain entity. Only valid for hybrid terrain entities.
        /// </summary>
        /// <param name="position">Position at which to build.</param>
        /// <param name="brushType">Type of brush to use.</param>
        /// <param name="layer">Layer to build on.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public bool Build(Vector3 position, TerrainEntityBrushType brushType, int layer)
        {
            if (IsValid() == false)
            {
                Logging.LogError("[TerrainEntity:Build] Unknown entity.");
                return false;
            }

            if (internalEntity is not HybridTerrainEntity)
            {
                Logging.LogWarning("[TerrainEntity:Build] Operation only valid on hybrid terrain entities.");
                return false;
            }

            WorldEngine.Entity.Terrain.TerrainEntityBrushType internalBrushType;
            switch (brushType)
            {
                case TerrainEntityBrushType.sphere:
                    internalBrushType = WorldEngine.Entity.Terrain.TerrainEntityBrushType.sphere;
                    break;

                case TerrainEntityBrushType.roundedCube:
                default:
                    internalBrushType = WorldEngine.Entity.Terrain.TerrainEntityBrushType.roundedCube;
                    break;
            }
            return ((HybridTerrainEntity) internalEntity).Build(
                new UnityEngine.Vector3(position.x, position.y, position.z), internalBrushType, layer);
        }

        /// <summary>
        /// Dig on the terrain entity. Only valid for hybrid terrain entities.
        /// </summary>
        /// <param name="position">Position at which to dig.</param>
        /// <param name="brushType">Type of brush to use.</param>
        /// <param name="layer">Layer to dig on.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public bool Dig(Vector3 position, TerrainEntityBrushType brushType, int layer)
        {
            if (IsValid() == false)
            {
                Logging.LogError("[TerrainEntity:Dig] Unknown entity.");
                return false;
            }

            if (internalEntity is not HybridTerrainEntity)
            {
                Logging.LogWarning("[TerrainEntity:Dig] Operation only valid on hybrid terrain entities.");
                return false;
            }

            WorldEngine.Entity.Terrain.TerrainEntityBrushType internalBrushType;
            switch (brushType)
            {
                case TerrainEntityBrushType.sphere:
                    internalBrushType = WorldEngine.Entity.Terrain.TerrainEntityBrushType.sphere;
                    break;

                case TerrainEntityBrushType.roundedCube:
                default:
                    internalBrushType = WorldEngine.Entity.Terrain.TerrainEntityBrushType.roundedCube;
                    break;
            }
            return ((HybridTerrainEntity) internalEntity).Dig(
                new UnityEngine.Vector3(position.x, position.y, position.z), internalBrushType, layer);
        }

        /// <summary>
        /// Get the height at a given x and y index.
        /// </summary>
        /// <param name="xIndex">X index at which to get the height.</param>
        /// <param name="yIndex">Y index at which to get the height.</param>
        /// <returns>The height at the given x and y index.</returns>
        public float GetHeight(int xIndex, int yIndex)
        {
            if (IsValid() == false)
            {
                Logging.LogError("[TerrainEntity:GetHeight] Unknown entity.");
                return 0;
            }

            if (internalEntity is HybridTerrainEntity)
            {
                return ((HybridTerrainEntity) internalEntity).GetHeight(xIndex, yIndex);
            }
            else if (internalEntity is WorldEngine.Entity.TerrainEntity)
            {
                return ((WorldEngine.Entity.TerrainEntity) internalEntity).GetHeight(xIndex, yIndex);
            }
            else
            {
                Logging.LogError("[TerrainEntity:GetHeight] Invalid entity type.");
                return 0;
            }
        }

        /// <summary>
        /// Get the layer mask for a given terrain entity layer.
        /// </summary>
        /// <param name="index">Index for which to get the layer mask.</param>
        /// <returns>A layer mask (a 2d array of values between 0 and 1 corresponding to the
        /// intensity of the given layer).</returns>
        public float[,] GetLayerMask(int index)
        {
            if (IsValid() == false)
            {
                Logging.LogError("[TerrainEntity:GetLayerMask] Unknown entity.");
                return null;
            }

            if (internalEntity is HybridTerrainEntity)
            {
                return ((HybridTerrainEntity) internalEntity).GetLayerMask(index);
            }
            else if (internalEntity is WorldEngine.Entity.TerrainEntity)
            {
                return ((WorldEngine.Entity.TerrainEntity) internalEntity).GetLayerMask(index);
            }
            else
            {
                Logging.LogError("[TerrainEntity:GetLayerMask] Invalid entity type.");
                return null;
            }
        }

        /// <summary>
        /// Get the block at a given position.
        /// </summary>
        /// <param name="position">Position to get the block at.</param>
        /// <returns>Block at the given position (array containing the operation
        /// (as a string) and layer index (as an int)). Operations: build, dig, unset.</returns>
        public object[] GetBlockAtPosition(Vector3 position)
        {
            if (IsValid() == false)
            {
                Logging.LogError("[TerrainEntity:GetBlockAtPosition] Unknown entity.");
                return null;
            }

            if (internalEntity is not HybridTerrainEntity)
            {
                Logging.LogWarning("[TerrainEntity:GetBlockAtPosition] Operation only valid on hybrid terrain entities.");
                return null;
            }

            Tuple<HybridTerrainEntity.VoxelOperation, int> block = ((HybridTerrainEntity) internalEntity).GetBlockAtPosition(
                new UnityEngine.Vector3(position.x, position.y, position.z));

            if (block == null)
            {
                return null;
            }

            return new object[] {
                (block.Item1 == HybridTerrainEntity.VoxelOperation.Build ? "build"
                : block.Item1 == HybridTerrainEntity.VoxelOperation.Dig ? "dig" : "unset"),
                block.Item2
            };
        }

        internal TerrainEntity()
        {
            internalEntityType = typeof(TerrainEntity);
        }
    }
}
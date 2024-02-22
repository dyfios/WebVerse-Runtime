// Copyright (c) 2019-2024 Five Squared Interactive. All rights reserved.

using FiveSQD.WebVerse.Runtime;
using FiveSQD.WebVerse.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FiveSQD.WebVerse.Handlers.Javascript.APIs.Entity
{
    /// <summary>
    /// A helper class for the Entity API.
    /// </summary>
    public class EntityAPIHelper : MonoBehaviour
    {
        /// <summary>
        /// Dictionary of Entity API references and internal entity references.
        /// </summary>
        private static Dictionary<WorldEngine.Entity.BaseEntity, BaseEntity> loadedEntities;

        /// <summary>
        /// Instance of the Entity API Helper.
        /// </summary>
        private static EntityAPIHelper instance;

        /// <summary>
        /// Initialize the entity dictionary.
        /// </summary>
        public static void InitializeEntityMapping()
        {
            loadedEntities = new Dictionary<WorldEngine.Entity.BaseEntity, BaseEntity>();
        }

        /// <summary>
        /// Add an entity mapping.
        /// </summary>
        /// <param name="internalEntity">Internal entity reference.</param>
        /// <param name="publicEntity">API entity reference.</param>
        public static void AddEntityMapping(WorldEngine.Entity.BaseEntity internalEntity, BaseEntity publicEntity)
        {
            if (loadedEntities.ContainsKey(internalEntity))
            {
                Logging.LogWarning("[EntityAPIHelper:AddEntityMapping] Key already exists.");
                return;
            }
            loadedEntities.Add(internalEntity, publicEntity);
        }

        /// <summary>
        /// Remove an entity mapping.
        /// </summary>
        /// <param name="internalEntity">Internal entity reference.</param>
        public static void RemoveEntityMapping(WorldEngine.Entity.BaseEntity internalEntity)
        {
            if (!loadedEntities.ContainsKey(internalEntity))
            {
                Logging.LogWarning("[EntityAPIHelper:RemoveEntityMapping] Key does not exist.");
                return;
            }

            loadedEntities.Remove(internalEntity);
        }

        /// <summary>
        /// Get the API entity reference for an internal entity.
        /// </summary>
        /// <param name="internalEntity">Internal entity reference.</param>
        public static BaseEntity GetPublicEntity(WorldEngine.Entity.BaseEntity internalEntity)
        {
            if (internalEntity == null)
            {
                return null;
            }

            if (loadedEntities.ContainsKey(internalEntity))
            {
                return loadedEntities[internalEntity];
            }

            return null;
        }

        /// <summary>
        /// Get the internal entity reference for an API entity.
        /// </summary>
        /// <param name="publicEntity">API entity reference.</param>
        public static WorldEngine.Entity.BaseEntity GetPrivateEntity(BaseEntity publicEntity)
        {
            if (publicEntity == null)
            {
                return null;
            }

            return loadedEntities.ContainsValue(publicEntity) ?
                loadedEntities.FirstOrDefault(x => x.Value == publicEntity).Key : null;
        }

        /// <summary>
        /// Set information for a voxel block type.
        /// </summary>
        /// <param name="id">Block ID.</param>
        /// <param name="info">Information to apply to voxel block type.</param>
        /// <param name="internalEntity">Internal entity reference.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public static bool SetBlockInfoAsync(int id, VoxelBlockInfo info, WorldEngine.Entity.VoxelEntity internalEntity)
        {
            if (info == null)
            {
                Logging.LogWarning("[EntityAPIHelper:SetBlockInfoAsync] Invalid Block Info.");
                return false;
            }

            if (instance == null)
            {
                Logging.LogError("[EntityAPIHelper:SetBlockInfoAsync] Entity API Helper not set up.");
                return false;
            }
            instance.StartCoroutine(instance.SetBlockInfoCoroutine(id, info, internalEntity));
            return true;
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
        /// <param name="timeout">Timeout for PNG loads.</param>
        /// <returns>The hybrid terrain entity object.</returns>
        public static TerrainEntity LoadHybridTerrainEntityAsync(BaseEntity parent, float length, float width,
            float height, float[][] heights, TerrainEntityLayer[] layers, TerrainEntityLayerMaskCollection layerMasks,
            WorldTypes.Vector3 position, WorldTypes.Quaternion rotation, WorldTypes.Vector3 scale, bool isSize = false,
            string id = null, string tag = null, string onLoaded = null, float timeout = 10)
        {
            TerrainEntity te = new TerrainEntity();

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
            Vector3 pos = new Vector3(position.x, position.y, position.z);
            Quaternion rot = new Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);
            Vector3 scl = new Vector3(scale.x, scale.y, scale.z);

            if (heights == null || heights[0] == null)
            {
                Logging.LogWarning("[EntityAPIHelper->LoadHybridTerrainEntityAsync] Invalid heights array.");
                return null;
            }

            float[,] processedHeights = new float[heights.Length, heights[0].Length];
            for (int i = 0; i < heights.Length; i++)
            {
                for (int j = 0; j < heights[0].Length; j++)
                {
                    processedHeights[i, j] = heights[i][j];
                }
            }

            if (layers == null || layers.Length < 1)
            {
                Logging.LogWarning("[EntityAPIHelper->LoadHybridTerrainEntityAsync] Invalid layers array.");
                return null;
            }

            if (layerMasks == null)
            {
                Logging.LogWarning("[EntityAPIHelper->LoadHybridTerrainEntityAsync] Invalid layer masks.");
                return null;
            }

            instance.StartCoroutine(instance.LoadHybridTerrainEntityCoroutine(te, pBE, guid, length, width, height, processedHeights, layers,
                layerMasks.ToFloatArrays(), pos, rot, scl, isSize, tag, onLoaded, timeout));

            return te;
        }

        public static bool RegisterPrivateEntity(WorldEngine.Entity.BaseEntity entityToRegister)
        {
            if (entityToRegister is WorldEngine.Entity.ButtonEntity)
            {
                ButtonEntity be = new ButtonEntity();
                be.internalEntity = entityToRegister;
                be.internalEntityType = typeof(ButtonEntity);
                AddEntityMapping(entityToRegister, be);
                return true;
            }
            else if (entityToRegister is WorldEngine.Entity.CanvasEntity)
            {
                CanvasEntity ce = new CanvasEntity();
                ce.internalEntity = entityToRegister;
                ce.internalEntityType = typeof(CanvasEntity);
                AddEntityMapping(entityToRegister, ce);
                return true;
            }
            else if (entityToRegister is WorldEngine.Entity.CharacterEntity)
            {
                CharacterEntity ce = new CharacterEntity();
                ce.internalEntity = entityToRegister;
                ce.internalEntityType = typeof(CanvasEntity);
                AddEntityMapping(entityToRegister, ce);
                return true;
            }
            else if (entityToRegister is WorldEngine.Entity.ContainerEntity)
            {
                ContainerEntity ce = new ContainerEntity();
                ce.internalEntity = entityToRegister;
                ce.internalEntityType = typeof(ContainerEntity);
                AddEntityMapping(entityToRegister, ce);
                return true;
            }
            else if (entityToRegister is WorldEngine.Entity.InputEntity)
            {
                InputEntity ie = new InputEntity();
                ie.internalEntity = entityToRegister;
                ie.internalEntityType = typeof(InputEntity);
                AddEntityMapping(entityToRegister, ie);
                return true;
            }
            else if (entityToRegister is WorldEngine.Entity.LightEntity)
            {
                LightEntity le = new LightEntity();
                le.internalEntity = entityToRegister;
                le.internalEntityType = typeof(LightEntity);
                AddEntityMapping(entityToRegister, le);
                return true;
            }
            else if (entityToRegister is WorldEngine.Entity.MeshEntity)
            {
                MeshEntity me = new MeshEntity();
                me.internalEntity = entityToRegister;
                me.internalEntityType = typeof(MeshEntity);
                AddEntityMapping(entityToRegister, me);
                return true;
            }
            else if (entityToRegister is WorldEngine.Entity.TerrainEntity)
            {
                TerrainEntity te = new TerrainEntity();
                te.internalEntity = entityToRegister;
                te.internalEntityType = typeof(TerrainEntity);
                AddEntityMapping(entityToRegister, te);
                return true;
            }
            else if (entityToRegister is WorldEngine.Entity.TextEntity)
            {
                TextEntity te = new TextEntity();
                te.internalEntity = entityToRegister;
                te.internalEntityType = typeof(TextEntity);
                AddEntityMapping(entityToRegister, te);
                return true;
            }
            else if (entityToRegister is WorldEngine.Entity.VoxelEntity)
            {
                VoxelEntity ve = new VoxelEntity();
                ve.internalEntity = entityToRegister;
                ve.internalEntityType = typeof(VoxelEntity);
                AddEntityMapping(entityToRegister, ve);
                return true;
            }
            else
            {
                Logging.LogError("[EntityAPIHelper->RegisterPrivateEntity] Invalid entity type.");
                return false;
            }
        }

        /// <summary>
        /// Initialize the Entity API Helper.
        /// </summary>
        public void Initialize()
        {
            instance = this;
        }

        /// <summary>
        /// Set information for a voxel block type in a coroutine.
        /// </summary>
        /// <param name="id">Block ID.</param>
        /// <param name="info">Information to apply to voxel block type.</param>
        /// <param name="internalEntity">Internal entity reference.</param>
        /// <param name="timeout">Timeout period after which to abort setting the block information.</param>
        private IEnumerator SetBlockInfoCoroutine(int id, VoxelBlockInfo info, WorldEngine.Entity.VoxelEntity internalEntity, float timeout = 10)
        {
            if (info == null)
            {
                Logging.LogWarning("[EntityAPIHelper:SetBlockInfoCoroutine] Invalid Block Info.");
                yield return null;
            }

            WorldEngine.Entity.Voxels.BlockInfo blockInfo = new WorldEngine.Entity.Voxels.BlockInfo(info.id);

            foreach (int key in blockInfo.subTypes.Keys)
            {
                uint completedRequests = 0;

                Texture2D leftTex = null;
                Texture2D rightTex = null;
                Texture2D topTex = null;
                Texture2D bottomTex = null;
                Texture2D frontTex = null;
                Texture2D backTex = null;

                Action<Texture2D> lOnDownloaded = new Action<Texture2D>((tex) =>
                {
                    leftTex = tex;
                    completedRequests++;
                });
                WebVerseRuntime.Instance.pngHandler.LoadImageResourceAsTexture2D(info.subTypes[key].leftTex, lOnDownloaded);

                Action<Texture2D> rOnDownloaded = new Action<Texture2D>((tex) =>
                {
                    rightTex = tex;
                    completedRequests++;
                });
                WebVerseRuntime.Instance.pngHandler.LoadImageResourceAsTexture2D(info.subTypes[key].rightTex, rOnDownloaded);

                Action<Texture2D> fOnDownloaded = new Action<Texture2D>((tex) =>
                {
                    frontTex = tex;
                    completedRequests++;
                });
                WebVerseRuntime.Instance.pngHandler.LoadImageResourceAsTexture2D(info.subTypes[key].frontTex, fOnDownloaded);

                Action<Texture2D> bOnDownloaded = new Action<Texture2D>((tex) =>
                {
                    backTex = tex;
                    completedRequests++;
                });
                WebVerseRuntime.Instance.pngHandler.LoadImageResourceAsTexture2D(info.subTypes[key].backTex, bOnDownloaded);

                Action<Texture2D> tOnDownloaded = new Action<Texture2D>((tex) =>
                {
                    topTex = tex;
                    completedRequests++;
                });
                WebVerseRuntime.Instance.pngHandler.LoadImageResourceAsTexture2D(info.subTypes[key].topTex, tOnDownloaded);

                Action<Texture2D> boOnDownloaded = new Action<Texture2D>((tex) =>
                {
                    bottomTex = tex;
                    completedRequests++;
                });
                WebVerseRuntime.Instance.pngHandler.LoadImageResourceAsTexture2D(info.subTypes[key].bottomTex, boOnDownloaded);

                float elapsedTime = 0;
                do
                {
                    yield return new WaitForSeconds(0.25f);
                    elapsedTime += 0.25f;
                } while (elapsedTime < timeout && completedRequests < 6);

                blockInfo.AddSubType(blockInfo.subTypes[key].id, blockInfo.subTypes[key].invisible,
                    topTex, bottomTex, leftTex, rightTex, frontTex, backTex);
            }

            internalEntity.SetBlockInfo(id, blockInfo);
        }

        /// <summary>
        /// Create a hybrid terrain entity in a coroutine.
        /// </summary>
        /// <param name="te">Public terrain entity object.</param>
        /// <param name="pBE">Parent entity.</param>
        /// <param name="guid">ID for the terrain entity.</param>
        /// <param name="length">Length of the terrain in terrain units.</param>
        /// <param name="width">Width of the terrain in terrain units.</param>
        /// <param name="height">Height of the terrain in terrain units.</param>
        /// <param name="heights">2D array of heights for the terrain.</param>
        /// <param name="layers">Layers for the terrain.</param>
        /// <param name="layerMasks">Layer masks for the terrain.</param>
        /// <param name="pos">Position of the entity relative to its parent.</param>
        /// <param name="rot">Rotation of the entity relative to its parent.</param>
        /// <param name="scl">Scale of the entity relative to its parent.</param>
        /// <param name="isSize">Whether or not the scale parameter is a size.</param>
        /// <param name="tag">Tag of the entity.</param>
        /// <param name="onLoaded">Action to perform on load. This takes a single parameter containing the created
        /// terrain entity object.</param>
        /// <param name="timeout">Timeout for PNG loads.</param>
        /// <returns>Coroutine.</returns>
        private IEnumerator LoadHybridTerrainEntityCoroutine(TerrainEntity te, WorldEngine.Entity.BaseEntity pBE, Guid guid,
            float length, float width, float height, float[,] heights, TerrainEntityLayer[] layers, float[][,] layerMasks,
            Vector3 pos, Quaternion rot, Vector3 scl, bool isSize = false,
            string tag = null, string onLoaded = null, float timeout = 10)
        {
            Dictionary<int, float[,]> formattedMasks = new Dictionary<int, float[,]>();
            if (layerMasks == null)
            {
                int layerIdx = 0;
                foreach (float[,] layerMask in layerMasks)
                {
                    formattedMasks.Add(layerIdx++, layerMask);
                }
            }

            Action onLoadAction = null;
            onLoadAction = () =>
            {
                te.internalEntity = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(guid);
                AddEntityMapping(te.internalEntity, te);
                if (!string.IsNullOrEmpty(onLoaded))
                {
                    WebVerseRuntime.Instance.javascriptHandler.Run(onLoaded.Replace("?", "te"));
                }
            };

            List<WorldEngine.Entity.Terrain.TerrainEntityLayer> formattedLayers
                    = new List<WorldEngine.Entity.Terrain.TerrainEntityLayer>();
            if (layers != null)
            {
                foreach (TerrainEntityLayer layer in layers)
                {
                    uint completedRequests = 0;

                    WorldEngine.Entity.Terrain.TerrainEntityLayer newFormattedLayer
                        = new WorldEngine.Entity.Terrain.TerrainEntityLayer();
                    newFormattedLayer.metallic = layer.metallic;
                    newFormattedLayer.smoothness = layer.smoothness;
                    newFormattedLayer.specular = new Color(layer.specular.r,
                        layer.specular.g, layer.specular.b, layer.specular.a);

                    if (!string.IsNullOrEmpty(layer.diffuseTexture))
                    {
                        WebVerseRuntime.Instance.pngHandler.LoadImageResourceAsTexture2D(layer.diffuseTexture,
                        new Action<Texture2D>((tex) =>
                        {
                            newFormattedLayer.diffuse = tex;
                            completedRequests++;
                        }));
                    }
                    else
                    {
                        completedRequests++;
                    }

                    if (!string.IsNullOrEmpty(layer.normalTexture))
                    {
                        WebVerseRuntime.Instance.pngHandler.LoadImageResourceAsTexture2D(layer.normalTexture,
                        new Action<Texture2D>((tex) =>
                        {
                            newFormattedLayer.normal = tex;
                            completedRequests++;
                        }));
                    }
                    else
                    {
                        completedRequests++;
                    }

                    if (!string.IsNullOrEmpty(layer.maskTexture))
                    {
                        WebVerseRuntime.Instance.pngHandler.LoadImageResourceAsTexture2D(layer.maskTexture,
                        new Action<Texture2D>((tex) =>
                        {
                            newFormattedLayer.mask = tex;
                            completedRequests++;
                        }));
                    }
                    else
                    {
                        completedRequests++;
                    }

                    float elapsedTime = 0;
                    do
                    {
                        yield return new WaitForSeconds(0.25f);
                        elapsedTime += 0.25f;
                    } while (elapsedTime < timeout && completedRequests < 3);

                    formattedLayers.Add(newFormattedLayer);
                }
            }

            WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadHybridTerrainEntity(length, width, height, heights,
                formattedLayers.ToArray(), formattedMasks, pBE, pos, rot, scl, guid, isSize, tag, onLoadAction);
        }
    }
}
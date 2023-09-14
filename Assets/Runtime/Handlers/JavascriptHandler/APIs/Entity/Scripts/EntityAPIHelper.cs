// Copyright (c) 2019-2023 Five Squared Interactive. All rights reserved.

using FiveSQD.WebVerse.Runtime;
using FiveSQD.WebVerse.Utilities;
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

                System.Action<Texture2D> lOnDownloaded = new System.Action<Texture2D>((tex) =>
                {
                    leftTex = tex;
                    completedRequests++;
                });
                WebVerseRuntime.Instance.pngHandler.LoadImageResourceAsTexture2D(info.subTypes[key].leftTex, lOnDownloaded);

                System.Action<Texture2D> rOnDownloaded = new System.Action<Texture2D>((tex) =>
                {
                    rightTex = tex;
                    completedRequests++;
                });
                WebVerseRuntime.Instance.pngHandler.LoadImageResourceAsTexture2D(info.subTypes[key].rightTex, rOnDownloaded);

                System.Action<Texture2D> fOnDownloaded = new System.Action<Texture2D>((tex) =>
                {
                    frontTex = tex;
                    completedRequests++;
                });
                WebVerseRuntime.Instance.pngHandler.LoadImageResourceAsTexture2D(info.subTypes[key].frontTex, fOnDownloaded);

                System.Action<Texture2D> bOnDownloaded = new System.Action<Texture2D>((tex) =>
                {
                    backTex = tex;
                    completedRequests++;
                });
                WebVerseRuntime.Instance.pngHandler.LoadImageResourceAsTexture2D(info.subTypes[key].backTex, bOnDownloaded);

                System.Action<Texture2D> tOnDownloaded = new System.Action<Texture2D>((tex) =>
                {
                    topTex = tex;
                    completedRequests++;
                });
                WebVerseRuntime.Instance.pngHandler.LoadImageResourceAsTexture2D(info.subTypes[key].topTex, tOnDownloaded);

                System.Action<Texture2D> boOnDownloaded = new System.Action<Texture2D>((tex) =>
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
    }
}
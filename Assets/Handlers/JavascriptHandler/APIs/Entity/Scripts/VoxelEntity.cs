using FiveSQD.WebVerse.Runtime;
using FiveSQD.WebVerse.Utilities;
using FiveSQD.WebVerse.Handlers.Javascript.APIs.WorldTypes;

namespace FiveSQD.WebVerse.Handlers.Javascript.APIs.Entity
{
    /// <summary>
    /// Class for a voxel entity.
    /// </summary>
    public class VoxelEntity : BaseEntity
    {
        /// <summary>
        /// Create a terrain entity.
        /// </summary>
        /// <param name="parent">Parent of the entity to create.</param>
        /// <param name="position">Position of the entity relative to its parent.</param>
        /// <param name="rotation">Rotation of the entity relative to its parent.</param>
        /// <param name="scale">Scale of the entity relative to its parent.</param>
        /// <param name="id">ID of the entity. One will be created if not provided.</param>
        /// <param name="onLoaded">Action to perform on load. This takes a single parameter containing the created
        /// terrain entity object.</param>
        /// <returns>The ID of the terrain entity object.</returns>
        public static System.Guid Create(BaseEntity parent,
            Vector3 position, Quaternion rotation, Vector3 scale,
            System.Guid? id = null, string onLoaded = null)
        {
            WorldEngine.Entity.BaseEntity pBE = EntityAPIHelper.GetPrivateEntity(parent);
            UnityEngine.Vector3 pos = new UnityEngine.Vector3(position.x, position.y, position.z);
            UnityEngine.Quaternion rot = new UnityEngine.Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);
            UnityEngine.Vector3 scl = new UnityEngine.Vector3(scale.x, scale.y, scale.z);

            VoxelEntity ve = new VoxelEntity();

            System.Action onLoadAction = null;
            if (!string.IsNullOrEmpty(onLoaded))
            {
                onLoadAction = () =>
                {
                    if (id.HasValue == false)
                    {
                        Logging.LogError("[VoxelEntity:Create] Unable to finish entity creation.");
                    }
                    else
                    {
                        ve.internalEntity = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(id.Value);
                        EntityAPIHelper.AddEntityMapping(ve.internalEntity, ve);
                        WebVerseRuntime.Instance.javascriptHandler.Run(onLoaded.Replace("?", "ve"));
                    }
                };
            }

            return WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadVoxelEntity(pBE, pos, rot, scl, id, onLoadAction);
        }

        public VoxelEntity()
        {
            internalEntityType = typeof(VoxelEntity);
        }

        /// <summary>
        /// Set the blockinfo for a given block with an ID.
        /// </summary>
        /// <param name="id">ID of the block.</param>
        /// <param name="info">Info for the block.</param>
        public bool SetBlockInfo(int id, VoxelBlockInfo info)
        {
            if (IsValid() == false)
            {
                Logging.LogError("[VoxelEntity:SetBlockInfo] Unknown entity.");
                return false;
            }

            if (info == null)
            {
                Logging.LogWarning("[VoxelEntity:SetBlockInfo] Invalid Block Info.");
                return false;
            }

            return EntityAPIHelper.SetBlockInfoAsync(id, info, (WorldEngine.Entity.VoxelEntity) internalEntity);
        }

        /// <summary>
        /// Set the block at a given coordinate.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="z">Z coordinate.</param>
        /// <param name="type">Block type at coordinate.</param>
        /// <param name="subType">Block subtype at coordinate.</param>
        public bool SetBlock(int x, int y, int z, int type, int subType)
        {
            if (IsValid() == false)
            {
                Logging.LogError("[VoxelEntity:SetBlock] Unknown entity.");
                return false;
            }

            ((WorldEngine.Entity.VoxelEntity) internalEntity).SetBlock(x, y, z, type, subType);
            return true;
        }

        /// <summary>
        /// Get the block at a given coordinate.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="z">Z coordinate.</param>
        /// <returns>Integer array with the first element being the block type,
        /// and the second being the block subtype.</returns>
        public int[] GetBlock(int x, int y, int z)
        {
            if (IsValid() == false)
            {
                Logging.LogError("[VoxelEntity:GetBlock] Unknown entity.");
                return null;
            }

            return ((WorldEngine.Entity.VoxelEntity) internalEntity).GetBlock(x, y, z);
        }

        /// <summary>
        /// Whether or not the voxel entity contains a given chunk.
        /// </summary>
        /// <param name="x">X index of the chunk.</param>
        /// <param name="y">Y index of the chunk.</param>
        /// <param name="z">Z index of the chunk.</param>
        /// <returns>Whether or not the chunk exists.</returns>
        public bool ContainsChunk(int x, int y, int z)
        {
            if (IsValid() == false)
            {
                Logging.LogError("[VoxelEntity:ContainsChunk] Unknown entity.");
                return false;
            }

            return ((WorldEngine.Entity.VoxelEntity) internalEntity).ContainsChunk(x, y, z);
        }
    }
}
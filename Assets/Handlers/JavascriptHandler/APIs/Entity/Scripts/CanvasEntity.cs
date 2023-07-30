using FiveSQD.WebVerse.Runtime;
using FiveSQD.WebVerse.Utilities;
using FiveSQD.WebVerse.Handlers.Javascript.APIs.WorldTypes;

namespace FiveSQD.WebVerse.Handlers.Javascript.APIs.Entity
{
    /// <summary>
    /// Class for a canvas entity.
    /// </summary>
    public class CanvasEntity : BaseEntity
    {
        /// <summary>
        /// Create a canvas entity.
        /// </summary>
        /// <param name="parent">Parent of the entity to create.</param>
        /// <param name="position">Position of the entity relative to its parent.</param>
        /// <param name="rotation">Rotation of the entity relative to its parent.</param>
        /// <param name="scale">Scale of the entity relative to its parent.</param>
        /// <param name="isSize">Whether or not the scale parameter is a size.</param>
        /// <param name="id">ID of the entity. One will be created if not provided.</param>
        /// <param name="onLoaded">Action to perform on load. This takes a single parameter containing the created
        /// canvas entity object.</param>
        /// <returns>The ID of the canvas entity object.</returns>
        public static System.Guid Create(BaseEntity parent,
            Vector3 position, Quaternion rotation, Vector3 scale, bool isSize = false,
            System.Guid? id = null, string onLoaded = null)
        {
            WorldEngine.Entity.BaseEntity pBE = EntityAPIHelper.GetPrivateEntity(parent);
            UnityEngine.Vector3 pos = new UnityEngine.Vector3(position.x, position.y, position.z);
            UnityEngine.Quaternion rot = new UnityEngine.Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);
            UnityEngine.Vector3 scl = new UnityEngine.Vector3(scale.x, scale.y, scale.z);

            CanvasEntity ce = new CanvasEntity();

            System.Action onLoadAction = null;
            if (!string.IsNullOrEmpty(onLoaded))
            {
                onLoadAction = () =>
                {
                    if (id.HasValue == false)
                    {
                        Logging.LogError("[CanvasEntity:Create] Unable to finish entity creation.");
                    }
                    else
                    {
                        ce.internalEntity = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(id.Value);
                        EntityAPIHelper.AddEntityMapping(ce.internalEntity, ce);
                        WebVerseRuntime.Instance.javascriptHandler.Run(onLoaded.Replace("?", "ce"));
                    }
                };
            }

            return WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadCanvasEntity(pBE, pos, rot, scl, id, isSize, onLoadAction);
        }

        internal CanvasEntity()
        {
            internalEntityType = typeof(CanvasEntity);
        }

        /// <summary>
        /// Make the canvas a world canvas.
        /// </summary>
        /// <param name="synchronize">Whether or not to synchronize the setting.</param>
        /// <returns>Whether or not the setting was successful.</returns>
        public bool MakeWorldCanvas()
        {
            if (IsValid() == false)
            {
                Logging.LogError("[CanvasEntity:MakeWorldCanvas] Unknown entity.");
                return false;
            }

            return ((WorldEngine.Entity.CanvasEntity) internalEntity).MakeWorldCanvas();
        }

        /// <summary>
        /// Make the canvas a screen canvas.
        /// </summary>
        /// <param name="synchronize">Whether or not to synchronize the setting.</param>
        /// <returns>Whether or not the setting was successful.</returns>
        public bool MakeScreenCanvas(bool synchronize = true)
        {
            if (IsValid() == false)
            {
                Logging.LogError("[CanvasEntity:MakeScreenCanvas] Unknown entity.");
                return false;
            }

            return ((WorldEngine.Entity.CanvasEntity) internalEntity).MakeScreenCanvas();
        }

        /// <summary>
        /// Returns whether or not the canvas entity is a screen canvas.
        /// </summary>
        /// <returns>Whether or not the canvas entity is a screen canvas.</returns>
        public bool IsScreenCanvas()
        {
            if (IsValid() == false)
            {
                Logging.LogError("[CanvasEntity:IsScreenCanvas] Unknown entity.");
                return false;
            }

            return ((WorldEngine.Entity.CanvasEntity) internalEntity).IsScreenCanvas();
        }
    }
}
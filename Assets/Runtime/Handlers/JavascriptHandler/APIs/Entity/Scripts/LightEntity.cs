using FiveSQD.WebVerse.Utilities;
using FiveSQD.WebVerse.Handlers.Javascript.APIs.WorldTypes;
using FiveSQD.WebVerse.Runtime;

namespace FiveSQD.WebVerse.Handlers.Javascript.APIs.Entity
{
    /// <summary>
    /// Class for a light entity.
    /// </summary>
    public class LightEntity : BaseEntity
    {
        /// <summary>
        /// Create a light entity.
        /// </summary>
        /// <param name="parent">Parent of the entity to create.</param>
        /// <param name="position">Position of the entity relative to its parent.</param>
        /// <param name="rotation">Rotation of the entity relative to its parent.</param>
        /// <param name="id">ID of the entity. One will be created if not provided.</param>
        /// <param name="tag">Tag of the entity.</param>
        /// <param name="onLoaded">Action to perform on load. This takes a single parameter containing the created
        /// light entity object.</param>
        /// <returns>The ID of the light entity object.</returns>
        public static System.Guid Create(BaseEntity parent,
            Vector3 position, Quaternion rotation,
            System.Guid? id = null, string tag = null, string onLoaded = null)
        {
            WorldEngine.Entity.BaseEntity pBE = EntityAPIHelper.GetPrivateEntity(parent);
            UnityEngine.Vector3 pos = new UnityEngine.Vector3(position.x, position.y, position.z);
            UnityEngine.Quaternion rot = new UnityEngine.Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);

            LightEntity le = new LightEntity();

            System.Action onLoadAction = null;
            if (!string.IsNullOrEmpty(onLoaded))
            {
                onLoadAction = () =>
                {
                    if (id.HasValue == false)
                    {
                        Logging.LogError("[LightEntity:Create] Unable to finish entity creation.");
                    }
                    else
                    {
                        le.internalEntity = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(id.Value);
                        EntityAPIHelper.AddEntityMapping(le.internalEntity, le);
                        WebVerseRuntime.Instance.javascriptHandler.Run(onLoaded.Replace("?", "le"));
                    }
                };
            }

            return WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadLightEntity(pBE, pos, rot, id, tag, onLoadAction);
        }

        internal LightEntity()
        {
            internalEntityType = typeof(LightEntity);
        }

        /// <summary>
        /// Set the light type for the light entity.
        /// </summary>
        /// <param name="type">Light type to apply.</param>
        /// <returns>Whether or not the setting was successful.</returns>
        public bool SetLightType(LightType type)
        {
            if (IsValid() == false)
            {
                Logging.LogError("[LightEntity:SetLightType] Unknown entity.");
                return false;
            }

            switch (type)
            {
                case LightType.Spot:
                    return ((WorldEngine.Entity.LightEntity) internalEntity).SetLightType(WorldEngine.Entity.LightEntity.LightType.Spot);


                case LightType.Directional:
                    return ((WorldEngine.Entity.LightEntity) internalEntity).SetLightType(WorldEngine.Entity.LightEntity.LightType.Directional);

                case LightType.Point:
                    return ((WorldEngine.Entity.LightEntity) internalEntity).SetLightType(WorldEngine.Entity.LightEntity.LightType.Point);

                default:
                    Logging.LogError("[LightEntity:SetLightType] Unknown light type.");
                    return false;
            }
        }

        /// <summary>
        /// Set the properties for the light.
        /// </summary>
        /// <param name="color">Color to apply to the light.</param>
        /// <param name="temperature">Temperature to apply to the light.</param>
        /// <param name="intensity">Intensity to apply to the light.</param>
        /// <returns>Whether or not the setting was successful.</returns>
        public bool SetLightProperties(Color color, int temperature, float intensity)
        {
            if (IsValid() == false)
            {
                Logging.LogError("[LightEntity:SetLightProperties] Unknown entity.");
                return false;
            }

            UnityEngine.Color32 col = new UnityEngine.Color(color.r, color.g, color.b, color.a);
            return ((WorldEngine.Entity.LightEntity) internalEntity).SetLightProperties(col, temperature, intensity);
        }

        /// <summary>
        /// Set the properties for the light.
        /// </summary>
        /// <param name="range">Range to apply to the light.</param>
        /// <param name="innerSpotAngle">Inner spot angle to apply to the light.</param>
        /// <param name="outerSpotAngle">Outer spot angle to apply to the light.</param>
        /// <param name="color">Color to apply to the light.</param>
        /// <param name="temperature">Temperature to apply to the light.</param>
        /// <param name="intensity">Intensity to apply to the light.</param>
        /// <returns>Whether or not the setting was successful.</returns>
        public bool SetLightProperties(float range, float innerSpotAngle, float outerSpotAngle,
            Color color, int temperature, float intensity)
        {
            if (IsValid() == false)
            {
                Logging.LogError("[LightEntity:SetLightProperties] Unknown entity.");
                return false;
            }

            UnityEngine.Color32 col = new UnityEngine.Color(color.r, color.g, color.b, color.a);
            return ((WorldEngine.Entity.LightEntity) internalEntity).SetLightProperties(
                range, innerSpotAngle, outerSpotAngle, col, temperature, intensity);
        }

        /// <summary>
        /// Set the properties for the light.
        /// </summary>
        /// <param name="range">Range to apply to the light.</param>
        /// <param name="intensity">Intensity to apply to the light.</param>
        /// <returns>Whether or not the setting was successful.</returns>
        public bool SetLightProperties(float range, float intensity)
        {
            if (IsValid() == false)
            {
                Logging.LogError("[LightEntity:SetLightProperties] Unknown entity.");
                return false;
            }

            return ((WorldEngine.Entity.LightEntity) internalEntity).SetLightProperties(range, intensity);
        }

        /// <summary>
        /// Get the properties for the light.
        /// </summary>
        /// <returns>The light properties of the light.</returns>
        public LightProperties GetLightProperties()
        {
            if (IsValid() == false)
            {
                Logging.LogError("[LightEntity:GetLightProperties] Unknown entity.");
                return new LightProperties()
                { color = Color.black, innerSpotAngle = 0, intensity = 0, outerSpotAngle = 0, range = 0, temperature = 0 };
            }

            WorldEngine.Entity.LightEntity.LightProperties props = ((WorldEngine.Entity.LightEntity) internalEntity).GetLightProperties();

            return new LightProperties()
            {
                color = new Color(props.color.r, props.color.g, props.color.b, props.color.a),
                temperature = props.temperature,
                intensity = props.intensity,
                range = props.range,
                innerSpotAngle = props.innerSpotAngle,
                outerSpotAngle = props.outerSpotAngle
            };
        }
    }
}
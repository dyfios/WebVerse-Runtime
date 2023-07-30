using FiveSQD.WebVerse.Runtime;
using FiveSQD.WebVerse.Utilities;
using FiveSQD.WebVerse.Handlers.Javascript.APIs.WorldTypes;

namespace FiveSQD.WebVerse.Handlers.Javascript.APIs.Entity
{
    /// <summary>
    /// Class for an input entity.
    /// </summary>
    public class InputEntity : BaseEntity
    {
        /// <summary>
        /// Create an input entity.
        /// </summary>
        /// <param name="parent">Parent canvas of the entity to create.</param>
        /// <param name="positionPercent">Position of the entity within its canvas.</param>
        /// <param name="sizePercent">Size of the entity relative to its canvas.</param>
        /// <param name="id">ID of the entity. One will be created if not provided.</param>
        /// <param name="onLoaded">Action to perform on load. This takes a single parameter containing the created
        /// input entity object.</param>
        /// <returns>The ID of the input entity object.</returns>
        public static System.Guid Create(CanvasEntity parent,
            Vector2 positionPercent, Vector2 sizePercent,
            System.Guid? id = null, string onLoaded = null)
        {
            WorldEngine.Entity.CanvasEntity pCE = (WorldEngine.Entity.CanvasEntity) EntityAPIHelper.GetPrivateEntity(parent);
            UnityEngine.Vector2 pos = new UnityEngine.Vector2(positionPercent.x, positionPercent.y);
            UnityEngine.Vector2 size = new UnityEngine.Vector2(sizePercent.x, sizePercent.y);

            InputEntity ie = new InputEntity();

            System.Action onLoadAction = null;
            if (!string.IsNullOrEmpty(onLoaded))
            {
                onLoadAction = () =>
                {
                    if (id.HasValue == false)
                    {
                        Logging.LogError("[InputEntity:Create] Unable to finish entity creation.");
                    }
                    ie.internalEntity = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(id.Value);
                    EntityAPIHelper.AddEntityMapping(ie.internalEntity, ie);
                    WebVerseRuntime.Instance.javascriptHandler.Run(onLoaded.Replace("?", "ie"));
                };
            }

            return WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadInputEntity(pCE, pos, size, id, onLoadAction);
        }

        internal InputEntity()
        {
            internalEntityType = typeof(InputEntity);
        }

        /// <summary>
        /// Get the text for the input entity.
        /// </summary>
        /// <returns>Text of the input entity.</returns>
        public string GetText()
        {
            if (IsValid() == false)
            {
                Logging.LogError("[InputEntity:GetText] Unknown entity.");
                return null;
            }

            return ((WorldEngine.Entity.InputEntity) internalEntity).GetText();
        }

        /// <summary>
        /// Set the text for the input entity.
        /// </summary>
        /// <param name="text">Text for the input entity.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public bool SetText(string text)
        {
            if (IsValid() == false)
            {
                Logging.LogError("[InputEntity:SetText] Unknown entity.");
                return false;
            }

            return ((WorldEngine.Entity.InputEntity) internalEntity).SetText(text);
        }
    }
}
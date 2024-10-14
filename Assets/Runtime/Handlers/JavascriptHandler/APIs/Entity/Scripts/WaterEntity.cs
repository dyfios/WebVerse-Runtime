// Copyright (c) 2019-2024 Five Squared Interactive. All rights reserved.

using System;
using FiveSQD.WebVerse.Runtime;
using FiveSQD.WebVerse.Handlers.Javascript.APIs.Utilities;
using FiveSQD.WebVerse.Handlers.Javascript.APIs.WorldTypes;

namespace FiveSQD.WebVerse.Handlers.Javascript.APIs.Entity
{
    /// <summary>
    /// Class for a water entity.
    /// </summary>
    public class WaterEntity : BaseEntity
    {
        /// <summary>
        /// Create a water entity.
        /// </summary>
        /// <param name="parent">Parent of the entity to create.</param>
        /// <param name="shallowColor">Color for the shallow zone.</param>
        /// <param name="deepColor">Color for the deep zone.</param>
        /// <param name="specularColor">Specular color.</param>
        /// <param name="scatteringColor">Scattering color.</param>
        /// <param name="deepStart">Start of deep zone.</param>
        /// <param name="deepEnd">End of deep zone.</param>
        /// <param name="distortion">Distortion factor (range 0-128).</param>
        /// <param name="smoothness">Smoothness factor (range 0-1).</param>
        /// <param name="numWaves">Number of waves (range 1-32).</param>
        /// <param name="waveAmplitude">Wave amplitude (range 0-1).</param>
        /// <param name="waveSteepness">Wave steepness (range 0-1).</param>
        /// <param name="waveSpeed">Wave speed.</param>
        /// <param name="waveLength">Wave length.</param>
        /// <param name="position">Position of the entity relative to its parent.</param>
        /// <param name="rotation">Rotation of the entity relative to its parent.</param>
        /// <param name="scale">Scale of the entity relative to its parent.</param>
        /// <param name="id">ID of the entity. One will be created if not provided.</param>
        /// <param name="tag">Tag of the entity.</param>
        /// <param name="onLoaded">Action to perform on load. This takes a single parameter containing the created
        /// water entity object.</param>
        /// <returns>The ID of the water entity object.</returns>
        public static WaterEntity CreateWaterBody(BaseEntity parent,
            Color shallowColor, Color deepColor, Color specularColor, Color scatteringColor,
            float deepStart, float deepEnd, float distortion, float smoothness, float numWaves,
            float waveAmplitude, float waveSteepness, float waveSpeed, float waveLength,
            Vector3 position, Quaternion rotation, Vector3 scale, string id = null, string tag = null,
            string onLoaded = null)
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

            WaterEntity we = new WaterEntity();

            System.Action onLoadAction = null;
            onLoadAction = () =>
            {
                we.internalEntity = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(guid);
                EntityAPIHelper.AddEntityMapping(we.internalEntity, we);
                we.internalEntity.SetScale(scl, false);
                if (!string.IsNullOrEmpty(onLoaded))
                {
                    WebVerseRuntime.Instance.javascriptHandler.CallWithParams(onLoaded, new object[] { we });
                }
            };

            WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadWaterBodyEntity(
                new UnityEngine.Color(shallowColor.r, shallowColor.g, shallowColor.b, shallowColor.a),
                new UnityEngine.Color(deepColor.r, deepColor.g, deepColor.b, deepColor.a),
                new UnityEngine.Color(specularColor.r, specularColor.g, specularColor.b, specularColor.a),
                new UnityEngine.Color(scatteringColor.r, scatteringColor.g, scatteringColor.b, scatteringColor.a),
                deepStart, deepEnd, distortion, smoothness, numWaves, waveAmplitude, waveSteepness, waveSpeed,
                waveLength, null, pos, rot, guid, tag, onLoadAction);

            return we;
        }

        public WaterEntity()
        {
            internalEntityType = typeof(WorldEngine.Entity.WaterBodyEntity);
        }

        /// <summary>
        /// Set properties for the water body.
        /// </summary>
        /// <param name="shallowColor">Color for the shallow zone.</param>
        /// <param name="deepColor">Color for the deep zone.</param>
        /// <param name="specularColor">Specular color.</param>
        /// <param name="scatteringColor">Scattering color.</param>
        /// <param name="deepStart">Start of deep zone.</param>
        /// <param name="deepEnd">End of deep zone.</param>
        /// <param name="distortion">Distortion factor (range 0-128).</param>
        /// <param name="smoothness">Smoothness factor (range 0-1).</param>
        /// <param name="numWaves">Number of waves (range 1-32).</param>
        /// <param name="waveAmplitude">Wave amplitude (range 0-1).</param>
        /// <param name="waveSteepness">Wave steepness (range 0-1).</param>
        /// <param name="waveSpeed">Wave speed.</param>
        /// <param name="waveLength">Wave length.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public bool SetProperties(Color shallowColor, Color deepColor, Color specularColor, Color scatteringColor,
            float deepStart, float deepEnd, float distortion, float smoothness, float numWaves,
            float waveAmplitude, float waveSteepness, float waveSpeed, float waveLength)
        {
            if (IsValid() == false)
            {
                Logging.LogError("[WaterEntity:SetProperties] Unknown entity.");
                return false;
            }

            return ((WorldEngine.Entity.WaterBodyEntity) internalEntity).SetProperties(
                new UnityEngine.Color(shallowColor.r, shallowColor.g, shallowColor.b, shallowColor.a),
                new UnityEngine.Color(deepColor.r, deepColor.g, deepColor.b, deepColor.a),
                new UnityEngine.Color(specularColor.r, specularColor.g, specularColor.b, specularColor.a),
                new UnityEngine.Color(scatteringColor.r, scatteringColor.g, scatteringColor.b, scatteringColor.a),
                deepStart, deepEnd, distortion, smoothness, numWaves, waveAmplitude, waveSteepness, waveSpeed,
                waveLength);
        }
    }
}
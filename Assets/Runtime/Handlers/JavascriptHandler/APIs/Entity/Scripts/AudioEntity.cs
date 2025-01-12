// Copyright (c) 2019-2025 Five Squared Interactive. All rights reserved.

using System;
using FiveSQD.WebVerse.Handlers.Javascript.APIs.WorldTypes;
using FiveSQD.WebVerse.Runtime;

namespace FiveSQD.WebVerse.Handlers.Javascript.APIs.Entity
{
    /// <summary>
    /// Class for an audio entity.
    /// </summary>
    public class AudioEntity : BaseEntity
    {
        /// <summary>
        /// Whether or not to loop the audio clip.
        /// </summary>
        public bool loop
        {
            get
            {
                return ((WorldEngine.Entity.AudioEntity) internalEntity).loop;
            }
            set
            {
                ((WorldEngine.Entity.AudioEntity) internalEntity).loop = value;
            }
        }

        /// <summary>
        /// Priority for the audio clip. Values between 0 and 256, with 0 being highest priority.
        /// </summary>
        public int priority
        {
            get
            {
                return ((WorldEngine.Entity.AudioEntity) internalEntity).priority;
            }
            set
            {
                ((WorldEngine.Entity.AudioEntity) internalEntity).priority = value;
            }
        }

        /// <summary>
        /// Volume for the audio clip. Values between 0 and 1, with 1 being highest volume.
        /// </summary>
        public float volume
        {
            get
            {
                return ((WorldEngine.Entity.AudioEntity) internalEntity).volume;
            }
            set
            {
                ((WorldEngine.Entity.AudioEntity) internalEntity).volume = value;
            }
        }

        /// <summary>
        /// Pitch for the audio clip. Values between -3 and 3.
        /// </summary>
        public float pitch
        {
            get
            {
                return ((WorldEngine.Entity.AudioEntity) internalEntity).pitch;
            }
            set
            {
                ((WorldEngine.Entity.AudioEntity) internalEntity).pitch = value;
            }
        }

        /// <summary>
        /// Audio pan for the audio clip if playing in stereo. Values between -1 and 1, with -1
        /// being furthest to the left and 1 being furthest to the right.
        /// </summary>
        public float stereoPan
        {
            get
            {
                return ((WorldEngine.Entity.AudioEntity) internalEntity).stereoPan;
            }
            set
            {
                ((WorldEngine.Entity.AudioEntity) internalEntity).stereoPan = value;
            }
        }

        /// <summary>
        /// Create a audio entity.
        /// </summary>
        /// <param name="parent">Parent of the entity to create.</param>
        /// <param name="position">Position of the entity relative to its parent.</param>
        /// <param name="rotation">Rotation of the entity relative to its parent.</param>
        /// <param name="id">ID of the entity. One will be created if not provided.</param>
        /// <param name="tag">Tag of the entity.</param>
        /// <param name="onLoaded">Action to perform on load. This takes a single parameter containing the created
        /// audio entity object.</param>
        /// <returns>The ID of the audio entity object.</returns>
        public static AudioEntity Create(BaseEntity parent,
            Vector3 position, Quaternion rotation,
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

            AudioEntity ae = new AudioEntity();

            System.Action onLoadAction = null;
            onLoadAction = () =>
            {
                ae.internalEntity = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(guid);
                EntityAPIHelper.AddEntityMapping(ae.internalEntity, ae);
                if (!string.IsNullOrEmpty(onLoaded))
                {
                    WebVerseRuntime.Instance.javascriptHandler.CallWithParams(onLoaded, new object[] { ae });
                }
            };

            WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadAudioEntity(pBE, pos, rot, guid, tag, onLoadAction);

            return ae;
        }

        internal AudioEntity()
        {
            internalEntityType = typeof(WorldEngine.Entity.AudioEntity);
        }

        /// <summary>
        /// Load an audio clip from a .wav file.
        /// </summary>
        /// <param name="filePath">Path to audio clip file.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public bool LoadAudioClipFromWAV(string filePath)
        {
            EntityAPIHelper.LoadAudioFromFileAsync(filePath, (WorldEngine.Entity.AudioEntity) internalEntity);
            return true;
        }

        /// <summary>
        /// Play the audio.
        /// </summary>
        /// <returns>Whether or not the operation was successful.</returns>
        public bool Play()
        {
            ((WorldEngine.Entity.AudioEntity) internalEntity).Play();

            return true;
        }

        /// <summary>
        /// Stop playing the audio.
        /// </summary>
        /// <returns>Whether or not the operation was successful.</returns>
        public bool Stop()
        {
            ((WorldEngine.Entity.AudioEntity) internalEntity).Stop();

            return true;
        }

        /// <summary>
        /// Toggle pausing the audio.
        /// </summary>
        /// <param name="pause">Whether or not to pause.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public bool TogglePause(bool pause)
        {
            ((WorldEngine.Entity.AudioEntity) internalEntity).TogglePause(pause);

            return true;
        }
    }
}
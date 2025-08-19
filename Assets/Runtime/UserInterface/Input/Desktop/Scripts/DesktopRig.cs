// Copyright (c) 2019-2025 Five Squared Interactive. All rights reserved.

using System.Collections.Generic;
using FiveSQD.WebVerse.WorldEngine.Entity;
using FiveSQD.WebVerse.Runtime;
using FiveSQD.WebVerse.Utilities;
using UnityEngine;

namespace FiveSQD.WebVerse.Input.Desktop
{
    /// <summary>
    /// Class for a Desktop rig that manages desktop locomotion.
    /// </summary>
    public class DesktopRig : MonoBehaviour
    {
        /// <summary>
        /// The avatar entity for movement and character control.
        /// </summary>
        [Tooltip("The avatar entity for movement and character control.")]
        public WorldEngine.Entity.CharacterEntity avatarEntity;

        /// <summary>
        /// The camera transform for mouse look.
        /// </summary>
        [Tooltip("The camera transform for mouse look.")]
        public Transform cameraTransform;

        /// <summary>
        /// The rig origin/root transform.
        /// </summary>
        [Tooltip("The rig origin/root transform.")]
        public GameObject rigOrigin;

        /// <summary>
        /// Entities following the rig.
        /// </summary>
        [Tooltip("Entities following the rig.")]
        public List<BaseEntity> rigFollowers;

        /// <summary>
        /// Cycles to wait in between rig follower updates.
        /// </summary>
        [Tooltip("Cycles to wait in between rig follower updates.")]
        public int cyclesPerRigFollowerUpdate;

        /// <summary>
        /// Movement speed for WASD locomotion.
        /// </summary>
        [Tooltip("Movement speed for WASD locomotion.")]
        public float movementSpeed = 5.0f;

        /// <summary>
        /// Mouse sensitivity for look control.
        /// </summary>
        [Tooltip("Mouse sensitivity for look control.")]
        public float mouseSensitivity = 2.0f;

        /// <summary>
        /// Jump strength when jumping is enabled.
        /// </summary>
        [Tooltip("Jump strength when jumping is enabled.")]
        public float jumpStrength = 8.0f;

        /// <summary>
        /// Gravity strength when gravity is enabled.
        /// </summary>
        [Tooltip("Gravity strength when gravity is enabled.")]
        public float gravityStrength = -9.81f;

        /// <summary>
        /// The rig follower update counter.
        /// </summary>
        private int followersUpdateCount = 0;

        /// <summary>
        /// Current vertical velocity for gravity.
        /// </summary>
        private float verticalVelocity = 0f;

        /// <summary>
        /// Current rotation around X axis for mouse look.
        /// </summary>
        private float xRotation = 0f;

        /// <summary>
        /// Current movement input for continuous movement.
        /// </summary>
        private Vector2 currentMovementInput = Vector2.zero;

        /// <summary>
        /// Whether gravity is enabled for desktop locomotion.
        /// </summary>
        public bool gravityEnabled
        {
            get { return _gravityEnabled; }
            set 
            { 
                _gravityEnabled = value;
                if (!value)
                {
                    verticalVelocity = 0f; // Reset vertical velocity when gravity is disabled
                }
            }
        }
        private bool _gravityEnabled = true;

        /// <summary>
        /// Whether WASD motion is enabled for desktop locomotion.
        /// </summary>
        public bool wasdMotionEnabled { get; set; } = true;

        /// <summary>
        /// Whether mouse look is enabled for desktop locomotion.
        /// </summary>
        public bool mouseLookEnabled { get; set; } = true;

        /// <summary>
        /// Whether jump is enabled for desktop locomotion.
        /// </summary>
        public bool jumpEnabled { get; set; } = true;

        /// <summary>
        /// Initialize the Desktop rig.
        /// </summary>
        public void Initialize()
        {
            gravityEnabled = true;
            wasdMotionEnabled = true;
            mouseLookEnabled = true;
            jumpEnabled = true;
            rigFollowers = new List<BaseEntity>();
            
            // Try to find the avatar entity if not assigned
            if (avatarEntity == null && WorldEngine.WorldEngine.ActiveWorld != null)
            {
                // Look for the avatar entity in the world
                foreach (var entity in WorldEngine.WorldEngine.ActiveWorld.entityManager.GetAllEntities())
                {
                    if (entity is WorldEngine.Entity.CharacterEntity characterEntity && 
                        entity.entityTag == "avatar") // Assuming avatar entities are tagged as "avatar"
                    {
                        avatarEntity = characterEntity;
                        break;
                    }
                }
            }
            
            if (cameraTransform == null && Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }
            
            if (rigOrigin == null)
            {
                rigOrigin = gameObject;
            }
        }

        /// <summary>
        /// Terminate the Desktop rig.
        /// </summary>
        public void Terminate()
        {
            rigFollowers = null;
        }

        /// <summary>
        /// Set the avatar entity by tag.
        /// </summary>
        /// <param name="entityTag">Tag of the entity to use as avatar</param>
        public void SetAvatarEntityByTag(string entityTag)
        {
            if (string.IsNullOrEmpty(entityTag) || WorldEngine.WorldEngine.ActiveWorld == null)
            {
                return;
            }

            // Look for the entity with the specified tag
            foreach (var entity in WorldEngine.WorldEngine.ActiveWorld.entityManager.GetAllEntities())
            {
                if (entity is WorldEngine.Entity.CharacterEntity characterEntity && 
                    entity.entityTag == entityTag)
                {
                    avatarEntity = characterEntity;
                    break;
                }
            }
            
            if (avatarEntity == null)
            {
                Logging.LogWarning($"[DesktopRig->SetAvatarEntityByTag] Could not find character entity with tag: {entityTag}");
            }
        }

        /// <summary>
        /// Apply WASD movement based on input.
        /// </summary>
        /// <param name="moveInput">Movement input vector (x = horizontal, y = vertical)</param>
        public void ApplyMovement(Vector2 moveInput)
        {
            // Store the current movement input for continuous application
            currentMovementInput = moveInput;
        }

        /// <summary>
        /// Apply jump input.
        /// </summary>
        public void ApplyJump()
        {
            if (!jumpEnabled || avatarEntity == null)
            {
                return;
            }

            // Apply jump using the character entity's built-in jump system
            avatarEntity.Jump(jumpStrength);
        }

        /// <summary>
        /// Apply the stored movement input. Called from Update() for continuous movement.
        /// </summary>
        private void ProcessMovement()
        {
            if (!wasdMotionEnabled || avatarEntity == null || currentMovementInput == Vector2.zero)
            {
                return;
            }

            // Calculate movement direction relative to camera
            Vector3 forward = cameraTransform != null ? cameraTransform.forward : transform.forward;
            Vector3 right = cameraTransform != null ? cameraTransform.right : transform.right;
            
            // Remove Y component to keep movement horizontal
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            // Calculate movement vector
            Vector3 movement = (forward * currentMovementInput.y + right * currentMovementInput.x) * movementSpeed * Time.deltaTime;

            // Apply gravity if enabled (handled by the character entity itself)
            if (gravityEnabled)
            {
                // Use the character entity's built-in movement system which handles gravity
                avatarEntity.Move(new UnityEngine.Vector3(movement.x, movement.y, movement.z));
            }
            else
            {
                // For no gravity, only apply horizontal movement
                avatarEntity.Move(new UnityEngine.Vector3(movement.x, 0f, movement.z));
            }
        }

        /// <summary>
        /// Apply mouse look based on input.
        /// </summary>
        /// <param name="lookInput">Look input vector (x = horizontal, y = vertical)</param>
        public void ApplyLook(Vector2 lookInput)
        {
            if (!mouseLookEnabled || cameraTransform == null)
            {
                return;
            }

            // Apply horizontal rotation to both the rig and avatar entity
            float mouseX = lookInput.x * mouseSensitivity;
            transform.Rotate(Vector3.up * mouseX);
            
            // Also rotate the avatar entity left/right
            if (avatarEntity != null)
            {
                // Get current avatar rotation and add horizontal rotation
                UnityEngine.Quaternion currentRotation = avatarEntity.GetRotation();
                UnityEngine.Quaternion horizontalRotation = UnityEngine.Quaternion.AngleAxis(mouseX, UnityEngine.Vector3.up);
                avatarEntity.SetRotation(horizontalRotation * currentRotation, false, true);
            }

            // Apply vertical rotation to the camera
            float mouseY = lookInput.y * mouseSensitivity;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        void Update()
        {
            // Process continuous movement
            ProcessMovement();

            // Update rig followers similar to VRRig
            if (followersUpdateCount++ >= cyclesPerRigFollowerUpdate)
            {
                if (rigFollowers != null)
                {
                    foreach (BaseEntity follower in rigFollowers)
                    {
                        if (follower != null)
                        {
                            follower.SetPosition(rigOrigin.transform.position, false, true);
                            follower.SetRotation(rigOrigin.transform.rotation, false, true);
                        }
                    }
                }

                followersUpdateCount = 0;
            }
        }
    }
}
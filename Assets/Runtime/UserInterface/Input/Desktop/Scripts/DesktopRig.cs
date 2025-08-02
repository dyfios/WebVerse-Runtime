// Copyright (c) 2019-2025 Five Squared Interactive. All rights reserved.

using System.Collections.Generic;
using FiveSQD.WebVerse.WorldEngine.Entity;
using UnityEngine;

namespace FiveSQD.WebVerse.Input.Desktop
{
    /// <summary>
    /// Class for a Desktop rig that manages desktop locomotion.
    /// </summary>
    public class DesktopRig : MonoBehaviour
    {
        /// <summary>
        /// The character controller for movement.
        /// </summary>
        [Tooltip("The character controller for movement.")]
        public CharacterController characterController;

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
        /// Initialize the Desktop rig.
        /// </summary>
        public void Initialize()
        {
            gravityEnabled = true;
            wasdMotionEnabled = true;
            mouseLookEnabled = true;
            rigFollowers = new List<BaseEntity>();
            
            // Find required components if not assigned
            if (characterController == null)
            {
                characterController = GetComponent<CharacterController>();
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
        /// Apply WASD movement based on input.
        /// </summary>
        /// <param name="moveInput">Movement input vector (x = horizontal, y = vertical)</param>
        public void ApplyMovement(Vector2 moveInput)
        {
            if (!wasdMotionEnabled || characterController == null)
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
            Vector3 movement = (forward * moveInput.y + right * moveInput.x) * movementSpeed * Time.deltaTime;

            // Apply gravity
            if (gravityEnabled)
            {
                if (characterController.isGrounded)
                {
                    verticalVelocity = 0f;
                }
                else
                {
                    verticalVelocity += gravityStrength * Time.deltaTime;
                }
                movement.y = verticalVelocity * Time.deltaTime;
            }
            else
            {
                movement.y = 0f;
            }

            // Move the character
            characterController.Move(movement);
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

            // Apply horizontal rotation to the rig
            float mouseX = lookInput.x * mouseSensitivity;
            transform.Rotate(Vector3.up * mouseX);

            // Apply vertical rotation to the camera
            float mouseY = lookInput.y * mouseSensitivity;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        void Update()
        {
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
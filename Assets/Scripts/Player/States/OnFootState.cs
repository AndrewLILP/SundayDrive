// OnFootState.cs
using UnityEngine;
using RelaxingDrive.World;

namespace RelaxingDrive.Player
{
    /// <summary>
    /// Player state when walking on foot.
    /// Handles WASD movement using CharacterController.
    /// Handles interaction with objects (E key).
    /// Detects nearby interactables using trigger colliders.
    /// </summary>
    public class OnFootState : PlayerState
    {
        private GameObject playerCharacter;
        private CharacterController characterController;
        private FollowCamera camera;
        private GameObject carGameObject;
        
        // Movement settings
        private float moveSpeed = 5f;
        private float turnSpeed = 10f;
        private float gravity = -9.81f;
        private Vector3 verticalVelocity;
        
        // Interaction
        private IInteractable nearbyInteractable;
        private float interactionRange = 3f;
        
        public OnFootState(PlayerStateManager manager) : base(manager)
        {
            playerCharacter = manager.PlayerCharacter;
            camera = manager.FollowCamera;
            carGameObject = manager.CarGameObject;
            
            // Get or add CharacterController
            if (playerCharacter != null)
            {
                characterController = playerCharacter.GetComponent<CharacterController>();
                if (characterController == null)
                {
                    characterController = playerCharacter.AddComponent<CharacterController>();
                    // Setup default character controller values
                    characterController.height = 2f;
                    characterController.radius = 0.5f;
                    characterController.center = new Vector3(0, 1f, 0);
                }
            }
        }
        
        public override void Enter()
        {
            Debug.Log("Entered OnFoot State");
            
            // Enable player character
            if (playerCharacter != null)
            {
                playerCharacter.SetActive(true);
            }
            
            // Setup camera for walking
            if (camera != null)
            {
                camera.SetTarget(playerCharacter.transform);
                camera.SetOffset(stateManager.WalkingCameraOffset);
            }
            
            // Reset vertical velocity
            verticalVelocity = Vector3.zero;
        }
        
        public override void Update()
        {
            HandleMovement();
            CheckForInteractables();
            HandleInteractionInput();
        }
        
        public override void Exit()
        {
            Debug.Log("Exited OnFoot State");
            
            // Clear nearby interactable
            nearbyInteractable = null;
        }
        
        /// <summary>
        /// Handles WASD movement
        /// </summary>
        private void HandleMovement()
        {
            if (characterController == null)
                return;
            
            // Get input
            float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right arrows
            float vertical = Input.GetAxis("Vertical");     // W/S or Up/Down arrows
            
            // Create movement direction
            Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
            
            if (moveDirection.magnitude > 0.1f)
            {
                // Calculate movement relative to camera (or world)
                // For now, using world-relative movement (simpler)
                Vector3 move = moveDirection * moveSpeed * Time.deltaTime;
                
                // Rotate player to face movement direction
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                playerCharacter.transform.rotation = Quaternion.Slerp(
                    playerCharacter.transform.rotation,
                    targetRotation,
                    turnSpeed * Time.deltaTime
                );
                
                // Apply horizontal movement
                characterController.Move(move);
            }
            
            // Apply gravity
            if (characterController.isGrounded)
            {
                verticalVelocity.y = -2f; // Small downward force to stay grounded
            }
            else
            {
                verticalVelocity.y += gravity * Time.deltaTime;
            }
            
            characterController.Move(verticalVelocity * Time.deltaTime);
        }
        
        /// <summary>
        /// Checks for nearby interactable objects
        /// </summary>
        private void CheckForInteractables()
        {
            nearbyInteractable = null;
            
            // Check distance to car (to re-enter)
            float distanceToCar = Vector3.Distance(
                playerCharacter.transform.position,
                carGameObject.transform.position
            );
            
            if (distanceToCar <= interactionRange)
            {
                // Car is nearby and interactable
                // We'll implement IInteractable on car later
                Debug.Log("Near car - Press E to enter");
                // nearbyInteractable = carGameObject.GetComponent<IInteractable>();
            }
            
            // TODO: Check for other interactables (buildings, NPCs)
            // We'll use trigger colliders or overlap spheres
        }
        
        /// <summary>
        /// Handles E key for interaction
        /// </summary>
        private void HandleInteractionInput()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Check if near car
                float distanceToCar = Vector3.Distance(
                    playerCharacter.transform.position,
                    carGameObject.transform.position
                );
                
                if (distanceToCar <= interactionRange)
                {
                    EnterCar();
                    return;
                }
                
                // Otherwise, interact with nearby object
                if (nearbyInteractable != null)
                {
                    nearbyInteractable.Interact();
                }
            }
        }
        
        /// <summary>
        /// Handles entering the car
        /// </summary>
        private void EnterCar()
        {
            Debug.Log("Entering car...");
            stateManager.TransitionToDriving();
        }
    }
}
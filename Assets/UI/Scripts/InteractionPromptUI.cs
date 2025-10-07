// InteractionPromptUI.cs
using UnityEngine;
using UnityEngine.UIElements;
using RelaxingDrive.Player;
using RelaxingDrive.World;

namespace RelaxingDrive.UI
{
    /// <summary>
    /// Displays interaction prompts using UI Toolkit.
    /// Shows "Press E to..." when near interactable objects.
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    public class InteractionPromptUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerStateManager playerStateManager;
        [SerializeField] private UIDocument uiDocument;
        
        private Label promptLabel;
        private VisualElement promptContainer;
        private PlayerInteractionDetector interactionDetector;
        
        private void Awake()
        {
            if (uiDocument == null)
                uiDocument = GetComponent<UIDocument>();
        }
        
        private void OnEnable()
        {
            SetupUIElements();
        }
        
        private void Start()
        {
            // Get interaction detector from player character
            if (playerStateManager != null && playerStateManager.PlayerCharacter != null)
            {
                interactionDetector = playerStateManager.PlayerCharacter.GetComponent<PlayerInteractionDetector>();
                
                if (interactionDetector == null)
                {
                    // Add it if not present
                    interactionDetector = playerStateManager.PlayerCharacter.AddComponent<PlayerInteractionDetector>();
                }
            }
        }
        
        private void SetupUIElements()
        {
            var root = uiDocument.rootVisualElement;
            
            // Query UI elements
            promptContainer = root.Q<VisualElement>("InteractionPrompt");
            promptLabel = root.Q<Label>("PromptText");
            
            // Hide by default
            if (promptContainer != null)
            {
                promptContainer.style.display = DisplayStyle.None;
            }
        }
        
        private void Update()
        {
            UpdatePrompt();
        }
        
        /// <summary>
        /// Updates the interaction prompt based on nearby interactables
        /// </summary>
        private void UpdatePrompt()
        {
            // Only show prompt when on foot
            if (playerStateManager == null || !playerStateManager.IsOnFoot)
            {
                HidePrompt();
                return;
            }
            
            // Priority 1: Check for building/NPC interactables
            if (interactionDetector != null && interactionDetector.HasInteractable)
            {
                string promptText = interactionDetector.ClosestInteractable.GetInteractionPrompt();
                ShowPrompt(promptText);
                return;
            }
            
            // Priority 2: Check if near car
            if (playerStateManager.PlayerCharacter != null && playerStateManager.CarGameObject != null)
            {
                float distanceToCar = Vector3.Distance(
                    playerStateManager.PlayerCharacter.transform.position,
                    playerStateManager.CarGameObject.transform.position
                );
                
                if (distanceToCar <= 3f) // Same range as OnFootState
                {
                    ShowPrompt("Press E to enter vehicle");
                    return;
                }
            }
            
            // Nothing nearby
            HidePrompt();
        }
        
        /// <summary>
        /// Shows the prompt with given text
        /// </summary>
        private void ShowPrompt(string text)
        {
            if (promptContainer != null && promptLabel != null)
            {
                promptLabel.text = text;
                promptContainer.style.display = DisplayStyle.Flex;
            }
        }
        
        /// <summary>
        /// Hides the prompt
        /// </summary>
        private void HidePrompt()
        {
            if (promptContainer != null)
            {
                promptContainer.style.display = DisplayStyle.None;
            }
        }
    }
}
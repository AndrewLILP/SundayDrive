// DialogueUIController.cs
using UnityEngine;
using UnityEngine.UIElements;
using RelaxingDrive.Core;
using RelaxingDrive.Data;

namespace RelaxingDrive.UI
{
    /// <summary>
    /// Controls the dialogue UI using UI Toolkit.
    /// Observes DialogueManager and updates visual elements.
    /// Follows Single Responsibility - only handles UI updates.
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    public class DialogueUIController : MonoBehaviour
    {
        [Header("UI Document")]
        [SerializeField] private UIDocument uiDocument;
        
        // UI Toolkit elements (queried from UXML)
        private VisualElement dialoguePanel;
        private Label speakerNameLabel;
        private Label dialogueTextLabel;
        private Button continueButton;
        private VisualElement portraitImage;
        
        private void Awake()
        {
            // Get UIDocument component
            if (uiDocument == null)
                uiDocument = GetComponent<UIDocument>();
        }
        
        private void OnEnable()
        {
            // Subscribe to DialogueManager events
            DialogueManager.Instance.OnDialogueStarted += ShowDialogue;
            DialogueManager.Instance.OnDialogueLineChanged += UpdateDialogueLine;
            DialogueManager.Instance.OnDialogueEnded += HideDialogue;
            
            // Query UI elements from UXML
            SetupUIElements();
        }
        
        private void OnDisable()
        {
            // Unsubscribe from events
            if (DialogueManager.Instance != null)
            {
                DialogueManager.Instance.OnDialogueStarted -= ShowDialogue;
                DialogueManager.Instance.OnDialogueLineChanged -= UpdateDialogueLine;
                DialogueManager.Instance.OnDialogueEnded -= HideDialogue;
            }
        }
        
        /// <summary>
        /// Query and cache UI elements from UXML
        /// </summary>
        private void SetupUIElements()
        {
            var root = uiDocument.rootVisualElement;
            
            // Query elements by name (must match UXML)
            dialoguePanel = root.Q<VisualElement>("DialoguePanel");
            speakerNameLabel = root.Q<Label>("SpeakerName");
            dialogueTextLabel = root.Q<Label>("DialogueText");
            continueButton = root.Q<Button>("ContinueButton");
            portraitImage = root.Q<VisualElement>("PortraitImage");
            
            // Setup button click event
            if (continueButton != null)
            {
                continueButton.clicked += OnContinueClicked;
            }
            
            // Hide dialogue panel initially
            if (dialoguePanel != null)
            {
                dialoguePanel.style.display = DisplayStyle.None;
            }
        }
        
        /// <summary>
        /// Shows the dialogue panel
        /// </summary>
        private void ShowDialogue(DialogueData dialogueData)
        {
            if (dialoguePanel != null)
            {
                dialoguePanel.style.display = DisplayStyle.Flex;
            }
            
            // Set portrait if available
            if (portraitImage != null && dialogueData.speakerPortrait != null)
            {
                portraitImage.style.backgroundImage = new StyleBackground(dialogueData.speakerPortrait);
            }
        }
        
        /// <summary>
        /// Updates the dialogue text and speaker name
        /// </summary>
        private void UpdateDialogueLine(string speakerName, string lineText)
        {
            if (speakerNameLabel != null)
            {
                speakerNameLabel.text = speakerName;
            }
            
            if (dialogueTextLabel != null)
            {
                dialogueTextLabel.text = lineText;
            }
        }
        
        /// <summary>
        /// Hides the dialogue panel
        /// </summary>
        private void HideDialogue()
        {
            if (dialoguePanel != null)
            {
                dialoguePanel.style.display = DisplayStyle.None;
            }
        }
        
        /// <summary>
        /// Called when continue button is clicked
        /// </summary>
        private void OnContinueClicked()
        {
            DialogueManager.Instance.AdvanceDialogue();
        }
        
        /// <summary>
        /// Alternative: Advance dialogue with keyboard (Space or E)
        /// Call this from Update() if you want keyboard support
        /// </summary>
        private void Update()
        {
            if (DialogueManager.Instance.IsDialogueActive)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E))
                {
                    DialogueManager.Instance.AdvanceDialogue();
                }
            }
        }
    }
}
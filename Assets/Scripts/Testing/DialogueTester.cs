// DialogueTester.cs
using UnityEngine;
using RelaxingDrive.Core;
using RelaxingDrive.Data;

/// <summary>
/// Simple tester to verify dialogue system works.
/// Press T key to trigger test dialogue.
/// </summary>
public class DialogueTester : MonoBehaviour
{
    [Header("Test Dialogue")]
    [SerializeField] private DialogueData testDialogue;

    [Header("Test Key")]
    [SerializeField] private KeyCode testKey = KeyCode.T;

    private void Update()
    {
        if (Input.GetKeyDown(testKey))
        {
            TestDialogue();
        }
    }

    private void TestDialogue()
    {
        Debug.Log("===== DIALOGUE TEST: T Key Pressed =====");

        if (testDialogue == null)
        {
            Debug.LogError("DialogueTester: No test dialogue assigned!");
            Debug.LogError("Assign a DialogueData asset in Inspector!");
            return;
        }

        Debug.Log($"DialogueTester: Test dialogue assigned: {testDialogue.name}");

        if (DialogueManager.Instance == null)
        {
            Debug.LogError("DialogueTester: DialogueManager.Instance is NULL!");
            Debug.LogError("Make sure DialogueManager GameObject exists in scene!");
            return;
        }

        Debug.Log("DialogueTester: DialogueManager found ✓");
        Debug.Log("DialogueTester: Starting dialogue...");

        DialogueManager.Instance.StartDialogue(testDialogue);

        Debug.Log("DialogueTester: Dialogue started ✓");
    }
}
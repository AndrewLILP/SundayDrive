// TestUIScript.cs
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// MINIMAL TEST - Just proves UI Toolkit is working
/// </summary>
[RequireComponent(typeof(UIDocument))]
public class TestUIScript : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("===== TestUIScript: Awake called! =====");
    }

    private void Start()
    {
        Debug.Log("===== TestUIScript: Start called! =====");

        UIDocument uiDoc = GetComponent<UIDocument>();

        if (uiDoc == null)
        {
            Debug.LogError("TestUIScript: NO UIDocument component found!");
            return;
        }

        Debug.Log("TestUIScript: UIDocument component found ✓");

        var root = uiDoc.rootVisualElement;

        if (root == null)
        {
            Debug.LogError("TestUIScript: Root visual element is NULL!");
            return;
        }

        Debug.Log("TestUIScript: Root element found ✓");

        // Create a simple test label
        Label testLabel = new Label("TEST UI WORKING!");
        testLabel.style.position = Position.Absolute;
        testLabel.style.top = 50;
        testLabel.style.left = 50;
        testLabel.style.fontSize = 30;
        testLabel.style.color = Color.green;
        testLabel.style.backgroundColor = Color.black;

        // Padding - different syntax for Unity 6
        testLabel.style.paddingTop = 20;
        testLabel.style.paddingBottom = 20;
        testLabel.style.paddingLeft = 20;
        testLabel.style.paddingRight = 20;

        root.Add(testLabel);

        Debug.Log("===== TestUIScript: Test label added to screen! =====");
    }

    private void Update()
    {
        // This will spam console but proves Update is running
        if (Time.frameCount % 120 == 0) // Every 2 seconds at 60fps
        {
            Debug.Log("TestUIScript: Update is running...");
        }
    }
}
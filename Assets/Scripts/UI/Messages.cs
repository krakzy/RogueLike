using UnityEngine;
using UnityEngine.UIElements;

public class Messages : MonoBehaviour
{
    private Label[] labels = new Label[5];
    private VisualElement root;

    void Start()
    {
        // Assuming you have a UIDocument component attached to the same GameObject
        var uiDocument = GetComponent<UIDocument>();

        // Assign the root VisualElement
        root = uiDocument.rootVisualElement;

        // Initialize labels array with elements from the UI
        for (int i = 0; i < labels.Length; i++)
        {
            labels[i] = root.Q<Label>($"label{i + 1}");
        }

        // Clear all labels
        Clear();

        // Add initial message
        AddMessage("Welcome to the dungeon, Adventurer!", Color.green);
    }

    public void Clear()
    {
        // Set the text of all labels to an empty string
        foreach (var label in labels)
        {
            label.text = string.Empty;
        }
    }

    public void MoveUp()
    {
        // Shift text and color from each label to the next
        for (int i = labels.Length - 1; i > 0; i--)
        {
            labels[i].text = labels[i - 1].text;
            labels[i].style.color = labels[i - 1].style.color;
        }

        // Clear the first label
        labels[0].text = string.Empty;
    }

    public void AddMessage(string content, Color color)
    {
        // Move existing messages up
        MoveUp();

        // Set the first label with the new content and color
        labels[0].text = content;
        labels[0].style.color = color;
    }
}


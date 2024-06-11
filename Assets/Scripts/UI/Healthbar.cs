using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Healthbar : MonoBehaviour
{
    private VisualElement root;

    private VisualElement healthBar;
    private Label healthLabel;
    private Label levelLabel;  // New label for level
    private Label xpLabel;     // New label for XP

    void Start()
    {
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        healthBar = root.Q<VisualElement>("health-bar");
        healthLabel = root.Q<Label>("health-label");

        // Initialize the new labels for level and XP
        levelLabel = root.Q<Label>("level-label");
        xpLabel = root.Q<Label>("xp-label");
    }

    public void SetValues(int currentHitPoints, int maxHitPoints)
    {
        float percent = (float)currentHitPoints / maxHitPoints * 100;
        healthBar.style.width = Length.Percent(percent);
        healthLabel.text = $"Health: {currentHitPoints}/{maxHitPoints}";
    }

    public void SetLevel(int level)
    {
        if (levelLabel != null)
        {
            levelLabel.text = $"Level: {level}";
        }
    }

    public void SetXP(int xp)
    {
        if (xpLabel != null)
        {
            xpLabel.text = $"XP: {xp}";
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

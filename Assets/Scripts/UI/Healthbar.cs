using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Healthbar : MonoBehaviour
{
    private VisualElement root;

    private VisualElement healthBar;

    private Label healthLabel;
    void Start()
    {
        var uiDocument = GetComponent<UIDocument>();

        root = uiDocument.rootVisualElement;

        healthBar = root.Q<VisualElement>("health-bar");
        healthLabel = root.Q<Label>("health-label");

    }

    public void SetValues(int currentHitPoints, int maxHitPoints) 
    {
        float percent = (float)currentHitPoints / maxHitPoints * 100;

        healthBar.style.width = Length.Percent(percent);

        healthLabel.text = $"Health: {currentHitPoints}/{maxHitPoints}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

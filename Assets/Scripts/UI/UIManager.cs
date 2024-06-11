using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Singleton instance
    public static UIManager Instance { get; private set; }

    [Header("Documents")]
    public GameObject HealthBar; // Ensure this is assigned in the inspector
    public GameObject Messages;
    public GameObject Inventory; // New GameObject for Inventory

    private Healthbar healthBar;
    private Messages messagesController;
    private InventoryUI inventoryUI; // Variable to hold the InventoryUI component

    private void Awake()
    {
        // Implement singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Get the script components from the GameObjects
        if (HealthBar != null)
        {
            healthBar = HealthBar.GetComponent<Healthbar>();
            if (healthBar == null)
            {
                Debug.LogError("HealthBar component is not found on the assigned HealthBarObject!");
            }
        }
        else
        {
            Debug.LogError("HealthBar is not assigned in the UIManager!");
        }

        if (Messages != null)
        {
            messagesController = Messages.GetComponent<Messages>();
        }

        if (Inventory != null)
        {
            inventoryUI = Inventory.GetComponent<InventoryUI>();
            if (inventoryUI == null)
            {
                Debug.LogError("InventoryUI component is not found on the assigned Inventory GameObject!");
            }
        }
        else
        {
            Debug.LogError("Inventory is not assigned in the UIManager!");
        }

        // Initial clear and welcome message
        if (messagesController != null)
        {
            messagesController.Clear();
            messagesController.AddMessage("Welcome to the dungeon, Adventurer!", Color.yellow);
        }
    }

    public void UpdateHealth(int current, int max)
    {
        if (healthBar != null)
        {
            healthBar.SetValues(current, max);
        }
        else
        {
            Debug.LogError("HealthBar component is not assigned!");
        }
    }

    public void UpdateLevel(int level)
    {
        if (healthBar != null)
        {
            healthBar.SetLevel(level);
        }
        else
        {
            Debug.LogError("HealthBar component is not assigned!");
        }
    }

    public void UpdateXP(int xp)
    {
        if (healthBar != null)
        {
            healthBar.SetXP(xp);
        }
        else
        {
            Debug.LogError("HealthBar component is not assigned!");
        }
    }

    public void AddMessage(string message, Color color)
    {
        if (messagesController != null)
        {
            messagesController.AddMessage(message, color);
        }
    }

    // Public getter for the InventoryUI component
    public InventoryUI GetInventoryUI()
    {
        return inventoryUI;
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Items;

[RequireComponent(typeof(Actor))]
public class Player : MonoBehaviour, Controls.IPlayerActions
{
    private Controls controls;

    private bool inventoryIsOpen = false;
    private bool droppingItem = false;
    private bool usingItem = false;
    public List<Consumable> Inventory { get; private set; } // Inventory aangepast naar Consumable

    private void Awake()
    {
        controls = new Controls();
        Inventory = new List<Consumable>(); // Initialize the inventory
    }

    private void Start()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -5);
        GameManager.Get.Player = GetComponent<Actor>(); // Assuming GameManager has a Player property
    }

    private void OnEnable()
    {
        controls.Player.SetCallbacks(this);
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Player.SetCallbacks(null);
        controls.Disable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (inventoryIsOpen)
            {
                Vector2 direction = controls.Player.Movement.ReadValue<Vector2>();
                if (direction.y > 0)
                {
                    UIManager.Instance.GetInventoryUI().SelectPreviousItem();
                }
                else if (direction.y < 0)
                {
                    UIManager.Instance.GetInventoryUI().SelectNextItem();
                }
            }
            else
            {
                Move();
            }
        }
    }

    private void Move()
    {
        Vector2 direction = controls.Player.Movement.ReadValue<Vector2>();
        Vector2 roundedDirection = new Vector2(Mathf.Round(direction.x), Mathf.Round(direction.y));
        Debug.Log("roundedDirection");
        Action.MoveOrHit(GetComponent<Actor>(), roundedDirection);
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -5);
    }

    public void OnGrab(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 playerPosition = transform.position;
            Consumable item = GameManager.Get.GetItemAtLocation(playerPosition);

            if (item == null)
            {
                Debug.Log("Er is geen item op deze locatie.");
            }
            else if (Inventory.Count >= 10) // Assuming a maximum inventory size of 10
            {
                Debug.Log("Je inventory is vol.");
            }
            else
            {
                Inventory.Add(item);
                item.gameObject.SetActive(false);
                GameManager.Get.RemoveItem(item);
                Debug.Log("Item toegevoegd aan je inventory.");
            }
        }
    }

    public void OnDrop(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!inventoryIsOpen)
            {
                UIManager.Instance.GetInventoryUI().Show(Inventory);
                inventoryIsOpen = true;
                droppingItem = true;
            }
        }
    }

    public void OnUse(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!inventoryIsOpen)
            {
                UIManager.Instance.GetInventoryUI().Show(Inventory);
                inventoryIsOpen = true;
                usingItem = true;
            }
        }
    }

    public void OnExit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (inventoryIsOpen)
            {
                UIManager.Instance.GetInventoryUI().Hide();
                inventoryIsOpen = false;
                droppingItem = false;
                usingItem = false;
            }
        }
    }

    private void UseItem(Consumable item)
    {
        switch (item.Type)
        {
            case Consumable.ItemType.HealthPotion:
                // Controleer of de actor een speler is
                if (GetComponent<Player>() != null)
                {
                    // Roep de Heal-functie aan van de actor (speler)
                    GetComponent<Actor>().Heal(10); // Stel hier de hoeveelheid genezing in
                    Debug.Log("Health Potion gebruikt. Je hebt 10 HP hersteld.");
                }
                else
                {
                    Debug.LogError("Health Potion kan alleen door de speler worden gebruikt.");
                }
                break;
            case Consumable.ItemType.Fireball:
                // Gebruik de GetNearbyEnemies-functie van GameManager om alle vijanden in de buurt op te vragen
                List<Actor> nearbyEnemies = GameManager.Get.GetNearbyEnemies(transform.position);
                int damageAmount = 20; // Stel hier de schade van de vuurbal in
                foreach (Actor enemy in nearbyEnemies)
                {
                    enemy.DoDamage(damageAmount);
                }
                Debug.Log("Fireball gebruikt. " + damageAmount + " schade toegebracht aan alle vijanden in de buurt.");
                break;
            case Consumable.ItemType.ScrollOfConfusion:
                // Voer de Confuse-functie uit op alle vijanden in de buurt
                List<Actor> nearbyEnemiesConfuse = GameManager.Get.GetNearbyEnemies(transform.position);
                foreach (Actor enemy in nearbyEnemiesConfuse)
                {
                    Enemy enemyComponent = enemy.GetComponent<Enemy>();
                    if (enemyComponent != null)
                    {
                        enemyComponent.Confuse();
                    }
                }
                Debug.Log("Scroll of Confusion gebruikt. Alle vijanden in de buurt zijn in de war.");
                break;
            default:
                Debug.LogError("Onbekend itemtype: " + item.Type);
                break;
        }
    }






    public void OnSelect(InputAction.CallbackContext context)
    {
        if (context.performed && inventoryIsOpen)
        {
            int selectedItemIndex = UIManager.Instance.GetInventoryUI().Selected;
            if (selectedItemIndex < Inventory.Count)
            {
                Consumable selectedItem = Inventory[selectedItemIndex];

                if (droppingItem)
                {
                    selectedItem.transform.position = transform.position;
                    GameManager.Get.AddItem(selectedItem);
                    selectedItem.gameObject.SetActive(true);
                    Inventory.Remove(selectedItem);
                }
                else if (usingItem)
                {
                    UseItem(selectedItem);
                    Inventory.Remove(selectedItem);
                    Destroy(selectedItem.gameObject);
                }

                UIManager.Instance.GetInventoryUI().Hide();
                inventoryIsOpen = false;
                droppingItem = false;
                usingItem = false;
            }
        }
    }
}

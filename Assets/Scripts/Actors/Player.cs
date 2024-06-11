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

    private Actor actor;

    private void Awake()
    {
        controls = new Controls();
        Inventory = new List<Consumable>(); // Initialize the inventory
    }

    private void Start()
    {
        actor = GetComponent<Actor>();
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -5);
        GameManager.Get.Player = actor; // Assuming GameManager has a Player property
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
        Action.MoveOrHit(actor, roundedDirection);
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
        // Implementation of UseItem function
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

    public void OnMoveUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Check for ladder at current position
            Ladder ladder = GameManager.Get.GetLadderAtLocation(transform.position);
            if (ladder != null)
            {
                if (ladder.Up)
                {
                    GameManager.Get.MoveUp();
                }
                else
                {
                    GameManager.Get.MoveDown();
                }
            }
        }
    }

    public void OnMoveDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Check for ladder at current position
            Ladder ladder = GameManager.Get.GetLadderAtLocation(transform.position);
            if (ladder != null)
            {
                if (!ladder.Up)
                {
                    GameManager.Get.MoveDown();
                }
                else
                {
                    GameManager.Get.MoveUp();
                }
            }
        }
    }
}



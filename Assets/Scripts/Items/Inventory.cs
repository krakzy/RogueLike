using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class Inventory : MonoBehaviour
    {
        public List<Consumable> Items = new List<Consumable>();
        public int MaxItems;

        public bool AddItem(Consumable item)
        {
            if (Items.Count < MaxItems)
            {
                Items.Add(item);
                return true;
            }
            return false;
        }

        public void DropItem(Consumable item)
        {
            if (Items.Contains(item))
            {
                Items.Remove(item);
                Debug.Log($"{item.name} has been dropped.");
            }
            else
            {
                Debug.Log("Item not found in the inventory.");
            }
        }
    }
}

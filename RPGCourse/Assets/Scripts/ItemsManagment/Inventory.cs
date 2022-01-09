using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    [SerializeField] List<ItemManager> itemsList;


    private void Start()
    {
        instance = this;
        itemsList = new List<ItemManager>();
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            foreach (ItemManager itemInInventory in itemsList)
            {
                Debug.Log(itemInInventory);
                if (itemInInventory.GetComponentInChildren<StackableItems>())
                    Debug.Log(itemInInventory.GetComponentInChildren<StackableItems>().itemAmount);
            }
        }
    }

    public void AddItems(ItemManager item)
    {
        bool isItemStacks = item.GetComponentInChildren<StackableItems>();


        if (isItemStacks)
        {
            bool itemAlreadyInInventory = false;

            foreach (ItemManager itemInInventory in itemsList)
            {
                if (itemInInventory.itemName == item.itemName)
                {
                    itemInInventory.GetComponentInChildren<StackableItems>().itemAmount ++;
                    itemAlreadyInInventory = true;
                }
            }

            if (!itemAlreadyInInventory)
            {
                itemsList.Add(item);
            }
        }
        else
        {
            itemsList.Add(item);
        }
    }

    public void RemoveItem(ItemManager item)
    {
        bool isItemStacks = item.GetComponentInChildren<StackableItems>();


        if (isItemStacks)
        {
        
            ItemManager inventoryItem = null;
            foreach (ItemManager itemInInventory in itemsList)
            {
                if (itemInInventory.itemName == item.itemName)
                {
                    itemInInventory.GetComponentInChildren<StackableItems>().itemAmount--;
                    inventoryItem = itemInInventory;
                }
            }

            if (inventoryItem != null && inventoryItem.GetComponentInChildren<StackableItems>().itemAmount <= 0)
            {
                itemsList.Remove(inventoryItem);
            }
        }
        else
        {
            itemsList.Remove(item);
        }
    }

    public List<ItemManager> GetItemsList()
    {
        return itemsList;
    }

}

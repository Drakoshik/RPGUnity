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


    public void AddItems(ItemManager item)
    {
        if (item.isStakable)
        {
            bool itemAlreadyInInventory = false;

            foreach(ItemManager itemInInventory in itemsList)
            {
                if(itemInInventory.itemName == item.itemName)
                {
                    itemInInventory.amount += item.amount;
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
        if (item.isStakable)
        {
            ItemManager inventoryItem = null;
            foreach(ItemManager itemInInventory in itemsList)
            {
                if(itemInInventory.itemName == item.itemName)
                {
                    itemInInventory.amount--;
                    inventoryItem = itemInInventory;
                }
            }

            if(inventoryItem != null && inventoryItem.amount <= 0)
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

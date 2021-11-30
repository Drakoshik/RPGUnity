using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventar : MonoBehaviour
{
    public static inventar instance;

    [SerializeField] List<Item> itemsList;

    private void Start()
    {
        instance = this;
        itemsList = new List<Item>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            foreach (Item itemInInventory in itemsList)
            {
                Debug.Log(itemInInventory);
                if(itemInInventory.GetComponentInChildren<StackableItems>())
                    Debug.Log(itemInInventory.GetComponentInChildren<StackableItems>().itemAmount);
            }
        }
    }

    public void AddItem(Item item)
    {
        bool isItemStacks = item.GetComponentInChildren<StackableItems>();


        if (isItemStacks)
        {
            bool itemAlreadyInInventory = false;

            foreach (Item itemInInventory in itemsList)
            {
                if (itemInInventory.itemName == item.itemName)
                {
                    itemInInventory.GetComponentInChildren<StackableItems>().itemAmount += item.GetComponentInChildren<StackableItems>().itemAmount;
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
}

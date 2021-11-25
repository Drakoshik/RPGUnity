using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsAssets : MonoBehaviour
{
    public static ItemsAssets instance;
    [SerializeField] ItemManager[] itemsAvailable;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public ItemManager GetItemAsset(string itemToGetName)
    {
        foreach(ItemManager item in itemsAvailable)
        {
            if(item.itemName == itemToGetName)
            {
                return item;
            }
        }

        return null;
    }

}

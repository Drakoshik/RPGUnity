using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    public string itemName, itemDescription;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UseItem();
            inventar.instance.AddItem(this);
            
        }
    }

    public virtual void UseItem()
    {
        //DestroyObject(gameObject);
        gameObject.SetActive(false);
    }
}

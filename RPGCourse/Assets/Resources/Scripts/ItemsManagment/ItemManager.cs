using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{


    public string itemName, itemDescription;
    public int valueInCoins;
    public Sprite itemImage;


    public virtual void UseItem(int characterToUseOn)
    {
        BattleCharacters selectedCharacter = BattleManager.instance.GetActiveBattleCharacters()[characterToUseOn];
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Inventory.instance.AddItems(this);
            SelfDestroy();
            AudioManager.instance.PlaySFX(5);
        }
    }


    public void SelfDestroy()
    {
        gameObject.SetActive(false);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public enum ItemType { Item, Weapon, Armor}
    public ItemType itemType;

    public string itemName, itemDescription;
    public int valueInCoins;
    public Sprite itemImage;


    public enum AffectType { HP, Mana, MultiPotion}
    public int AmountOfAffect;
    public AffectType affectType;

    public int weaponDexterity;
    public int armorDefence;

    public bool isStakable;
    public int amount;


    public void UseItem(int characterToUseOn)
    {
        PlayerStats selectedCharacter = GameManager.instance.GetPlayerStats()[characterToUseOn];

        if(itemType == ItemType.Item)
        {
            if (affectType == AffectType.HP)
            {
                selectedCharacter.AddHP(AmountOfAffect);
            }
            else if (affectType == AffectType.Mana)
            {
                selectedCharacter.AddMana(AmountOfAffect);
            }
            else if (affectType == AffectType.MultiPotion)
            {
                selectedCharacter.AddMana(AmountOfAffect);
                selectedCharacter.AddHP(AmountOfAffect);
            }
        }
        else if(itemType == ItemType.Weapon)
        {
            if(selectedCharacter.equipedWeaponName != "")
            {
                Inventory.instance.AddItems(selectedCharacter.equipedWeapon);
            }

            selectedCharacter.EquipedWeapon(this);
        }
        else if (itemType == ItemType.Armor)
        {
            if (selectedCharacter.equipedArmorName != "")
            {
                Inventory.instance.AddItems(selectedCharacter.equipedArmor);
            }

            selectedCharacter.EquipedArmor(this);
        }
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

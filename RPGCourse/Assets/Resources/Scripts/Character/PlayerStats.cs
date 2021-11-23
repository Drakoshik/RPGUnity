using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    public string playerName;

    public Sprite CharacterImage;


    [SerializeField] int maxLevel = 50;
    public int playerLevel = 1;
    public int currentXp;
    public int[] xpForNextLevel;
    [SerializeField] int baseLevelXP = 100;

    public int maxHP = 100;
    public int currentHp;

    public int maxMana = 30;
    public int currentMana;


    public int dexterity;
    public int defence;

    public string equipedWeaponName, equipedArmorName;
    public int weaponPower, armorDefence;

    public ItemManager equipedWeapon, equipedArmor;


    private void Start()
    {
        instance = this;

        xpForNextLevel = new int[maxLevel];
        xpForNextLevel[1] = baseLevelXP;
        for(int i = 2; i < xpForNextLevel.Length; i++)
        {
            xpForNextLevel[i] = (int)(0.02f * i * i * i + 3.06f * i * i * i + 105.6f * i);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            AddXP(100);
        }
    }


    public void AddXP(int amountOfXP)
    {
        currentXp += amountOfXP;
        if(currentXp > xpForNextLevel[playerLevel])
        {
            currentXp -= xpForNextLevel[playerLevel];
            playerLevel++;

            if(playerLevel % 2 == 0)
            {
                dexterity++;
            }
            else
            {
                defence++;
            }

            maxHP = Mathf.FloorToInt(maxHP * 2f);
            currentHp = maxHP;

            maxMana = Mathf.FloorToInt(maxHP * 2f);
            currentMana = maxMana;

        }
    }

    public void AddHP(int amountHpToAdd)
    {
        currentHp += amountHpToAdd;
        if(currentHp > maxHP)
        {
            currentHp = maxHP;
        }
    }
    
    public void AddMana(int amountManaToAdd)
    {
        currentMana += amountManaToAdd;
        if(currentMana > maxMana)
        {
            currentMana = maxMana;
        }
    }


    public void EquipedWeapon(ItemManager weaponToEquiped)
    {
        equipedWeapon = weaponToEquiped;
        equipedWeaponName = equipedWeapon.itemName;
        weaponPower = equipedWeapon.weaponDexterity;
    }

    public void EquipedArmor(ItemManager armorToEquiped)
    {
        equipedArmor = armorToEquiped;
        equipedArmorName = equipedArmor.itemName;
        armorDefence = equipedArmor.armorDefence;
    }

}

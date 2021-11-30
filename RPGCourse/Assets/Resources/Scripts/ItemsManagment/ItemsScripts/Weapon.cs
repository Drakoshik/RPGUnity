using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : ItemManager
{
    public int weaponDexterity;
    public override void UseItem(int characterToUseOn)
    {
        PlayerStats selectedCharacter = GameManager.instance.GetPlayerStats()[characterToUseOn];
        selectedCharacter.EquipedWeapon(this);
        base.UseItem(characterToUseOn);
    }
}

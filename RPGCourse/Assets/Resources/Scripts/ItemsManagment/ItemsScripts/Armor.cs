using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : ItemManager
{
    public int armorDefence;
    public override void UseItem(int characterToUseOn)
    {
        PlayerStats selectedCharacter = GameManager.instance.GetPlayerStats()[characterToUseOn];
        selectedCharacter.EquipedArmor(this);
        base.UseItem(characterToUseOn);
    }
}

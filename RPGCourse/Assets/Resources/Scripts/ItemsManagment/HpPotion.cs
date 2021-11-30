using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpPotion : StackableItems
{
    public override void UseItem(int characterToUseOn)
    {
        PlayerStats selectedCharacter = GameManager.instance.GetPlayerStats()[characterToUseOn];
        selectedCharacter.currentHp += amountOfEffect;
        base.UseItem(characterToUseOn);
    }
}

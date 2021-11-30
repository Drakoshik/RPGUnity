using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPotion : StackableItems
{
    public override void UseItem(int characterToUseOn)
    {
        PlayerStats selectedCharacter = GameManager.instance.GetPlayerStats()[characterToUseOn];
        selectedCharacter.currentHp += amountOfEffect;
        selectedCharacter.currentMana += amountOfEffect;
        base.UseItem(characterToUseOn);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPotion : StackableItems
{
    public override void UseItem(int characterToUseOn)
    {
        PlayerStats selectedCharacter = GameManager.instance.GetPlayerStats()[characterToUseOn];
        selectedCharacter.currentMana += amountOfEffect;
        base.UseItem(characterToUseOn);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPotion : StackableItems
{
    public override void UseItem(int characterToUseOn)
    {
        if (GameManager.instance.isBattleStart)
        {
            BattleCharacters selectedCharacter = BattleManager.instance.GetActiveBattleCharacters()[characterToUseOn];
            selectedCharacter.currentHp += amountOfEffect;
            selectedCharacter.currentMana += amountOfEffect;
        }
        else
        {
            PlayerStats selectedCharacter = GameManager.instance.GetPlayerStats()[characterToUseOn];
            selectedCharacter.currentHp += amountOfEffect;
            selectedCharacter.currentMana += amountOfEffect;
        }        
        base.UseItem(characterToUseOn);
    }
}

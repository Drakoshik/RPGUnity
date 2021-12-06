using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackableItems : ItemManager
{
    public int itemAmount;
    public int amountOfEffect;

    public override void UseItem(int characterToUseOn)
    {
        base.UseItem(characterToUseOn);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpellButton : MonoBehaviour
{
    public string spellName;
    public int spellCost;

    public TextMeshProUGUI spellNameText, spellCostText;

    public void Press()
    {
        if(BattleManager.instance.GetCurrantActiveCharacter().currentMana >= spellCost)
        {
            BattleManager.instance.spellChoisePanel.SetActive(false);

            BattleManager.instance.OpenTargetMenu(spellName);
            BattleManager.instance.GetCurrantActiveCharacter().currentMana -= spellCost;
        }
        else
        {
            BattleManager.instance.battleNotice.SetText("NOT ENOUGH MANA!!!!");
            BattleManager.instance.battleNotice.Activate();
            BattleManager.instance.spellChoisePanel.SetActive(false);
        }
    }


}

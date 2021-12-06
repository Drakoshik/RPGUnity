using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleTargetButtons : MonoBehaviour
{
    public string moveName;
    public int activeBattletarget;
    public TextMeshProUGUI targetName;


    private void Start()
    {
        targetName = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Press()
    {
        BattleManager.instance.PlayerAttack(moveName, activeBattletarget);
    }

}

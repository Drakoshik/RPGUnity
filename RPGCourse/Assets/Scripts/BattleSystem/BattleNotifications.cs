using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleNotifications : MonoBehaviour
{
    [SerializeField] float timeAlive;
    [SerializeField] TextMeshProUGUI textNotice;

    public void SetText(string text)
    {
        textNotice.text = text;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        StartCoroutine(DissapearNotice());
    }

    IEnumerator DissapearNotice()
    {
        yield return new WaitForSeconds(timeAlive);
        gameObject.SetActive(false);
    }
 
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QouestZone : MonoBehaviour
{

    [SerializeField] string questToMark;
    [SerializeField] bool markAsComplete;

    [SerializeField] bool markOnEnter;

    private bool canMark;

    public bool deactivateOnMarking;


    private void OnMouseUpAsButton()
    {
        if(canMark && Input.GetMouseButtonDown(0))
        {
            canMark = false;
            MarkTheQuest();
        }
    }


    public void MarkTheQuest()
    {
        if (markAsComplete)
        {
            QuestManager.instance.MarkQuestComplete(questToMark);
        }
        else
        {
            QuestManager.instance.MarkQuestInComplete(questToMark);
        }

        gameObject.SetActive(!deactivateOnMarking);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (markOnEnter)
            {
                MarkTheQuest();
            }
            else
            {
                canMark = true;
            }
        }
    }

}

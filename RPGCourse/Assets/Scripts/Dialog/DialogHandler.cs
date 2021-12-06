using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogHandler : MonoBehaviour
{
    public string[] sentences;

    private bool canActivateBox;
    public bool ifNeedPreviousQuest;

    [SerializeField] string previousQuest;
    private int questID;

    [SerializeField] bool shouldActivateTheQuest;
    [SerializeField] string questToMark;
    [SerializeField] bool markAsComplete;

    private void Start()
    {
        questID = QuestManager.instance.GetQuestNumber(questToMark);
        if(questID >= 1)
            previousQuest = QuestManager.instance.GetQuestNames()[questID - 1];
    }


    private void Update()
    {
        if (canActivateBox && PlayerController.instance.isInteractionAvaliable && !DialogController.instance.IsDialogBoxActive())
        {
            DialogController.instance.ActivateDialog(sentences);
            if (shouldActivateTheQuest)
            {
                if (ifNeedPreviousQuest && questID >= 1)
                {
                    
                    if (QuestManager.instance.CheckIfComplete(previousQuest))
                    {
                        DialogController.instance.ActivateQuestAtEnd(questToMark, markAsComplete);
                    }
                }
                else
                    DialogController.instance.ActivateQuestAtEnd(questToMark, markAsComplete);

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canActivateBox = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canActivateBox = false;
        }
    }
}

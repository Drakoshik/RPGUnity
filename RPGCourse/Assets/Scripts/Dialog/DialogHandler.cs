using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogHandler : MonoBehaviour, IOpenButton
{
    public string[] sentences;

    private bool canActivateBox;
    public bool ifNeedPreviousQuest;

    [SerializeField] string previousQuest;
    private int questID;

    [SerializeField] bool shouldActivateTheQuest;
    [SerializeField] string questToMark;
    [SerializeField] bool markAsComplete;

    [SerializeField] string _name;

    public string Name { get; set; }


    private void Start()
    {
        questID = QuestManager.instance.GetQuestNumber(questToMark);
        if(questID >= 1)
            previousQuest = QuestManager.instance.GetQuestNames()[questID - 1];
        _name = "Dialog";
        Name = _name;
    }


    public void OpenWindow()
    {
        

        if (!DialogController.instance.IsDialogBoxActive())
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
}

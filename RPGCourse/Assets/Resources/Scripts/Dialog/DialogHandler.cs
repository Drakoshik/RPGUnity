using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogHandler : MonoBehaviour
{
    public string[] sentences;
    private bool canActivateBox;


    [SerializeField] bool shouldActivateTheQuest;
    [SerializeField] string questToMark;
    [SerializeField] bool markAsComplete;


    private void Update()
    {
        if(canActivateBox && Input.GetMouseButtonDown(0) && !DialogController.instance.IsDialogBoxActive())
        {
            DialogController.instance.ActivateDialog(sentences);
            if (shouldActivateTheQuest)
            {
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

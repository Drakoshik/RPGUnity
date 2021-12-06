using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    [SerializeField] string[] questNames;
    [SerializeField] bool[] questMarkersComplete;

    [SerializeField] TextMeshProUGUI questText;

    [SerializeField] GameObject questTextPanel;

    public static QuestManager instance;

    private void Start()
    {
        instance = this;


        questMarkersComplete = new bool[questNames.Length];
        UpdateQuestVisualisation();
    }


    private void Update()
    {

        if (GameManager.instance.isBattleStart == true || MenuManager.instance.menu.activeInHierarchy || GameManager.instance.shopOpened)
        {
            questTextPanel.SetActive(false);
        }
        else
            questTextPanel.SetActive(true);
    }


    public int GetQuestNumber(string questToFind)
    {
        for(int i = 0; i < questNames.Length; i++)
        {
            if(questNames[i] == questToFind)
            {
                return i;
            }
        }

        return 0;
    }


    public bool CheckIfComplete(string questToCheck)
    {
        int questNumberToCheck = GetQuestNumber(questToCheck);

        if(questNumberToCheck >= 0)
        {
            return questMarkersComplete[questNumberToCheck];
        }
        return false;
    }


    public void UpdateQuestObjects()
    {
        
        QuestObject[] questObjects = FindObjectsOfType<QuestObject>();

        if (questObjects.Length > 0)
        {
            foreach (QuestObject questObject in questObjects)
            {
                questObject.CheckForComletion();
            }

        }
       
    }

    public void UpdateQuestVisualisation()
    {
        questText.text = "";
        for (int i = 0; i < questNames.Length; i++)
        {
            if(questMarkersComplete[i] == false)
            {
                questText.text += questNames[i] + "\n";
            }
        }
    }

    public void MarkQuestComplete(string questToMark)
    {
        int questNumberToCheck = GetQuestNumber(questToMark);
        questMarkersComplete[questNumberToCheck] = true;

        UpdateQuestObjects();
        UpdateQuestVisualisation();
    }

    public void MarkQuestInComplete(string questToMark)
    {
        int questNumberToCheck = GetQuestNumber(questToMark);
        questMarkersComplete[questNumberToCheck] = false;

        UpdateQuestObjects();
        UpdateQuestVisualisation();
    }


    public void SaveQuestData()
    {
        for(int i = 0; i < questNames.Length; i++)
        {
            if (questMarkersComplete[i])
            {
                PlayerPrefs.SetInt("QuestMarker_" + questNames[i], 1);
            }
            else
            {
                PlayerPrefs.SetInt("QuestMarker_" + questNames[i], 0);
            }
        }
    }


    public void LoadQuestData()
    {
        for (int i = 0; i < questNames.Length; i++)
        {
            int valueToSet = 0;
            string KeyToUse = "QuestMarker_" + questNames[i];

            if (PlayerPrefs.HasKey(KeyToUse))
            {
                valueToSet = PlayerPrefs.GetInt(KeyToUse);
            }

            if (valueToSet == 0) questMarkersComplete[i] = false;
            else questMarkersComplete[i] = true;
        }
        UpdateQuestVisualisation();
    }

    public string[] GetQuestNames()
    {
        return questNames;
    }

}

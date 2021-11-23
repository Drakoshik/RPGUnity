using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] string[] questNames;
    [SerializeField] bool[] questMarkersComplete;

    public static QuestManager instance;

    private void Start()
    {
        instance = this;


        questMarkersComplete = new bool[questNames.Length];
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            //Debug.Log("asd");
            SaveQuestData();
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            //Debug.Log("123");
            LoadQuestData();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            print(CheckIfComplete("Defeat Dragon"));
            MarkQuestComplete("Steal the gem");
            MarkQuestInComplete("Take monster soul!");
        }
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

        if(questNumberToCheck != 0)
        {
            return questMarkersComplete[questNumberToCheck];
        }
        return false;
    }


    public void UpdateQuestObjects()
    {
        QuestObject[] questObjects = FindObjectsOfType<QuestObject>();

        if(questObjects.Length > 0)
        {
            foreach(QuestObject questObject in questObjects)
            {
                questObject.CheckForComletion();
            }
        }
    }

    public void MarkQuestComplete(string questToMark)
    {
        int questNumberToCheck = GetQuestNumber(questToMark);
        questMarkersComplete[questNumberToCheck] = true;

        UpdateQuestObjects();
    }

    public void MarkQuestInComplete(string questToMark)
    {
        int questNumberToCheck = GetQuestNumber(questToMark);
        questMarkersComplete[questNumberToCheck] = false;

        UpdateQuestObjects();
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
    }

}

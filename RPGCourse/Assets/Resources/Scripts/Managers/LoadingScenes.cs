using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScenes : MonoBehaviour
{
    [SerializeField] float timeToLoad;


    private void Start()
    {
        if(timeToLoad > 0)
        {
            StartCoroutine(LoadScene());
        }
    }

    public IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(timeToLoad);

        SceneManager.LoadScene(PlayerPrefs.GetString("Current_Scene"));
        GameManager.instance.LoadData();
        QuestManager.instance.LoadQuestData();
    }

}

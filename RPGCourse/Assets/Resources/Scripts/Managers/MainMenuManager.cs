using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] string newGameScene;
    [SerializeField] Button continueButton;


    private void Start()
    {
        if (PlayerPrefs.HasKey("Player_Pos_x"))
        {
            continueButton.interactable = true;
        }
        else 
            continueButton.interactable = false;
    }


    public void NewGameButton()
    {
        SceneManager.LoadScene(newGameScene);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void ContinueButton()
    {
        SceneManager.LoadScene("LoadingScene");
    }

}

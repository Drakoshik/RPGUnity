using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
	private static GameLoader instance;

	public GameObject gameManager; 
	public GameObject audioManager; 
	public GameObject battleManager; 
	public GameObject menuManager; 


	void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			instance = this;
		}

		DontDestroyOnLoad(gameObject);

		if (GameManager.instance == null)
		{
			Instantiate(gameManager);
		}

		if (AudioManager.instance == null)
		{
			Instantiate(audioManager);
		}

		if (MenuManager.instance == null)
		{
			Instantiate(menuManager);
		}

		if (BattleManager.instance == null)
		{
			Instantiate(battleManager);
		}
	}
}

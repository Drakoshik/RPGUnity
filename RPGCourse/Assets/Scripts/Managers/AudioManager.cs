using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource[] SFX, backgroundMusic;

    public static AudioManager instance;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            PlayBackgroundMusic(5);
        }
    }

    public void PlaySFX(int soundToPlay)
    {
        if(soundToPlay < SFX.Length)
        {
            SFX[soundToPlay].Play();
        }
    }

    public void PlayBackgroundMusic(int musictoPlay)
    {
        StopMusic();

        if (musictoPlay < backgroundMusic.Length)
        {
            backgroundMusic[musictoPlay].Play();
        }
    }

    public void StopMusic()
    {
        foreach(AudioSource song in backgroundMusic)
        {
            song.Stop();
        }
    }

}

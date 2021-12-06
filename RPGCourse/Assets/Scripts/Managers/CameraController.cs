using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private PlayerController playerTarget;
    CinemachineVirtualCamera virtualCamera;

    [SerializeField] int musicToPlay;
    private bool musicAlreadyPlaying;

    private void Start()
    {
        playerTarget = FindObjectOfType<PlayerController>();
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        virtualCamera.Follow = playerTarget.transform;
    }


    private void Update()
    {
        if (!musicAlreadyPlaying)
        {
            musicAlreadyPlaying = true;
            AudioManager.instance.PlayBackgroundMusic(musicToPlay);
        }

        while(playerTarget == null)
        {
            playerTarget = FindObjectOfType<PlayerController>();
            if (virtualCamera)
            {
                virtualCamera.Follow = playerTarget.transform;
            }
        }
    }

}

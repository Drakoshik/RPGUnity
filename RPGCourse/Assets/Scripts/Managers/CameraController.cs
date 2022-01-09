using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private PlayerController playerTarget;
    CinemachineVirtualCamera virtualCamera;

    private float _zoomMin = 3;
    private float _zoomMax = 5;

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

        if (!PlayerController.instance.stopMove)
        {
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroLastPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOneLastPos = touchOne.position - touchOne.deltaPosition;

                float disTouch = (touchZeroLastPos - touchOneLastPos).magnitude;
                float currDistouch = (touchZero.position - touchOne.position).magnitude;

                float difference = currDistouch - disTouch;

                Zoom(difference + 0.001f);
            }

            Zoom(Input.GetAxis("Mouse ScrollWheel"));
        }

        
    }


    private void Zoom(float inc)
    {
        virtualCamera.m_Lens.OrthographicSize = Mathf.Clamp(virtualCamera.m_Lens.OrthographicSize - inc, _zoomMin, _zoomMax);
    }
}

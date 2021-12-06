using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [SerializeField] Joystick joystick;

    [SerializeField] float moveSpeed = 5;

    [SerializeField] Rigidbody2D playerRigidBody;
    [SerializeField] Animator playerAnimator;

    public string transitionName;
    public bool isInteractionAvaliable = false;

    private Vector3 botomLeftEdge;
    private Vector3 topRightEdge;

    public bool stopMove = false;

    private void Start()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);   
    }


    private void FixedUpdate()
    {
        isInteractionAvaliable = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            GetInteraction();

        float horizontalMovement;
        float verticalMovement;

#if UNITY_ANDROID
        horizontalMovement = joystick.Horizontal;
        verticalMovement = joystick.Vertical;
        joystick.gameObject.SetActive(true);

        if (stopMove)
        {
            playerRigidBody.velocity = Vector2.zero;
            joystick.gameObject.SetActive(false);
        }
        else
        {
            playerRigidBody.velocity = new Vector2(horizontalMovement, verticalMovement) * moveSpeed;
            joystick.gameObject.SetActive(true);
        }
#endif


#if UNITY_STANDALONE_WIN
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");
        joystick.gameObject.SetActive(false);

        if (stopMove)
        {
            playerRigidBody.velocity = Vector2.zero;
        }
        else
        {
            playerRigidBody.velocity = new Vector2(horizontalMovement, verticalMovement) * moveSpeed;
        }
#endif

        playerAnimator.SetFloat("movementX", playerRigidBody.velocity.x);
        playerAnimator.SetFloat("movementY", playerRigidBody.velocity.y);

        if ((horizontalMovement == 1 || horizontalMovement == -1 || verticalMovement == 1 || verticalMovement == -1) && !stopMove)
        {
            playerAnimator.SetFloat("lastX", horizontalMovement);
            playerAnimator.SetFloat("lastY", verticalMovement);
        }

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, botomLeftEdge.x, topRightEdge.x),
            Mathf.Clamp(transform.position.y, botomLeftEdge.y, topRightEdge.y),
            Mathf.Clamp(transform.position.z, botomLeftEdge.z, topRightEdge.z)
        );
    }

    public void SetLimit(Vector3 bottomEdgeToSet, Vector3 topEdgeToSet)
    {
        botomLeftEdge = bottomEdgeToSet;
        topRightEdge = topEdgeToSet;
    }

    public void GetInteraction()
    {
        isInteractionAvaliable = true;
        Debug.Log(isInteractionAvaliable);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] GameObject button;

    private IOpenButton openFunction;


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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        openFunction = collision.GetComponent<IOpenButton>();
        if (openFunction != null)
        {
            buttonText.text = openFunction.Name;
            button.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        openFunction = null;
        button.SetActive(false);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            InterstitialAds.instance.ShowAd();
        }


        float horizontalMovement;
        float verticalMovement;


#if UNITY_ANDROID
        horizontalMovement = joystick.Horizontal;
        verticalMovement = joystick.Vertical;
        joystick.gameObject.SetActive(true);

        if (stopMove)
        {
            joystick.GetComponent<Joystick>().handle.localPosition = Vector2.zero;
            joystick.input = Vector2.zero;
            playerRigidBody.velocity = Vector2.zero;
            joystick.gameObject.SetActive(false);
            button.SetActive(false);
        }
        else
        {
            playerRigidBody.velocity = new Vector2(horizontalMovement, verticalMovement) * moveSpeed;
            joystick.gameObject.SetActive(true);
            if (openFunction != null)
            {
                buttonText.text = openFunction.Name;
                button.SetActive(true);
            }
        }
#endif


#if UNITY_STANDALONE_WIN
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");
        joystick.gameObject.SetActive(false);

        if (stopMove)
        {
            playerRigidBody.velocity = Vector2.zero;
        button.SetActive(false);
        }
        else
        {
            playerRigidBody.velocity = new Vector2(horizontalMovement, verticalMovement) * moveSpeed;
        if (openFunction != null)
            {
                buttonText.text = openFunction.Name;
                button.SetActive(true);
            }
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

        if(openFunction != null)
        {
            openFunction.OpenWindow();
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [SerializeField] float moveSpeed = 5;

    [SerializeField] Rigidbody2D playerRigidBody;
    [SerializeField] Animator playerAnimator;

    public string transitionName;

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


    private void Update()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        if (stopMove)
        {
            playerRigidBody.velocity = Vector2.zero;
        }
        else
        {
            playerRigidBody.velocity = new Vector2(horizontalMovement, verticalMovement) * moveSpeed;
        }

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
}

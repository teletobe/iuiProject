using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public GameObject enemy;
    public float enemyCheckDistance = 1f;

    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;
    public float runSpeed = 40f;

    private Transform myTransform;
    private Rigidbody2D myRigidbody;
    private BoxCollider2D myCollider2D;
    private Vector3 lastPosition;
    private bool isMoving;
    private float timer;

    public float movementThreshold = 0.1f;
    public float triggerDuration = 0.05f; 


    private void Start()
    {
        // Get the Transform component when the script starts
        myTransform = GetComponent<Transform>();
        myRigidbody = GetComponent<Rigidbody2D>();
        myCollider2D = GetComponent<BoxCollider2D>();

        // Initialize variables
        lastPosition = transform.position;
        isMoving = false;
        timer = 0f;
    }

    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }

        // Check if the object is moving
        if (Vector3.Distance(transform.position, lastPosition) > movementThreshold)
        {
            isMoving = true;
            timer = 0f;
        }
        else
        {
            isMoving = false;
            timer += Time.deltaTime;
        }

        if (transform.position.y > 0.1f && !isMoving && timer >= 0.1f)
        {
            // Set isTrigger to true
            myCollider2D.isTrigger = true;

            // Delay for a short period (e.g., triggerDuration seconds)
            StartCoroutine(DelayedSetTrigger(false, triggerDuration));
        }

        // Update last position
        lastPosition = transform.position;
    }


    private System.Collections.IEnumerator DelayedSetTrigger(bool value, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Set isTrigger back to the specified value
        myCollider2D.isTrigger = value;
    }


    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

}

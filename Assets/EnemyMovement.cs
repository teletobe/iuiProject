using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public CharacterController2D controller;

    float horizontalMove = 0f;

    public float runSpeed = 40f;
    public Transform player; // Reference to the player's transform

    void Update()
    {
        float direction = (player.position.x > transform.position.x) ? 1f : -1f;

        horizontalMove = direction * runSpeed;

    }

    void FixedUpdate()
    {
        // move character
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, false);
    
    }
}

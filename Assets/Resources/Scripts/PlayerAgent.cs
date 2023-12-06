using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class PlayerAgent : Agent
{
    Rigidbody2D rBody;
    public CharacterController2D controller;
    public float runSpeed = 40f;
    public float enemyCheckDistance = 1f;
    public GameObject enemy;

    public PlayerHealth playerHealth;

    private float horizontalMove = 0f;
    private bool jump = false;
    private bool crouch = false;

    private int previousHealth = 100;

    private float episodeTimer;

    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
    }


    public override void OnEpisodeBegin()
    {
        if (this.transform.localPosition.y < 0)
        {
            this.rBody.angularVelocity = 0f; // Change to 0f for 2D physics
            this.rBody.velocity = Vector2.zero; // Change to Vector2.zero for 2D physics
            this.transform.localPosition = new Vector3(10, 0.2f, 0);
        }

        episodeTimer = 0f;  // Reset the timer at the beginning of each episode


        // Move the target to a new spot
        enemy.transform.localPosition = new Vector3(-4,
                                           0.2f,
                                           0);
        // Reset player and enemy positions
        this.transform.localPosition = new Vector3(-8f, 0.5f, 0f);
        enemy.transform.localPosition = new Vector3(8f, 0.5f, 0f);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(enemy.transform.localPosition);
        sensor.AddObservation(this.transform.localPosition);

        
        sensor.AddObservation(enemy.GetComponent<Enemy>().getHealth());


        // TODO
        // observation space ändern
        //sensor.AddObservation(this.playerHealth);
        // TODO
        // add life of enemy as observaiton
        // add health of own one too
    }




    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions, size = 1 (continuous action space for left and right movement)
        float input = actionBuffers.DiscreteActions[0];
        Debug.Log(input);
        if (input == 0)
        {
            Move(-1);
        } else if (input == 1)
        {
            Move(1);
            
        } else if (input == 2)
        {
            Combat();
        }



        // TODO
        // add jump too
        // add action also to hit
        // -> increase action size 






        // Rewards
        float distanceToEnemy = Vector3.Distance(this.transform.localPosition, enemy.transform.localPosition);


        // Reached target
        //Debug.Log(distanceToEnemy);
        /*
         * if (distanceToEnemy < 3.5f)
        {
            SetReward(1.0f);
            EndEpisode();
        }
        */

        if (enemy.GetComponent<Enemy>().getHealth() < previousHealth)
        {
            SetReward(0.1f);
            previousHealth = enemy.GetComponent<Enemy>().getHealth();
        }

        if (enemy.GetComponent<Enemy>().getHealth() <= 0)
        {
            SetReward(1.0f);
            EndEpisode();
            enemy.GetComponent<Enemy>().setHealth(100);
            previousHealth = 100;
        }


        episodeTimer += Time.fixedDeltaTime;  // Increment the timer

        if (episodeTimer >= 30f)
        {
            EndEpisode();  // End the episode if the timer reaches 30 seconds
            episodeTimer = 0f;  // Reset the timer
        }


        // TODO
        // Some more Rewards and episode end conditions
        // Zuerst nur mal enemy killed reward! 
        // und episode beenden wenn kein health mehr 
        //
        //
        // distance reward function -> smaller reward e.g. 0.05?
        //

        // get hurt = negative reward?

        // hit enemy, larger reward e.g. 0.15?

        // kill enemy = reward 1.0 and end

    }

    private void Move(float moveAction)
    {
        // Move the player horizontally
        horizontalMove = moveAction * runSpeed;


        // TODO jump action method??
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    private void Combat()
    {

        this.gameObject.GetComponent<PlayerCombat>().Attack();
        
    }



    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxisRaw("Horizontal");
    }
}

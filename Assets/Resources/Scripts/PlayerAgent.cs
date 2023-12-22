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

    private int previousHealthEnemy = 100;
    private int previousHealthPlayer = 100;
    

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
        sensor.AddObservation(this.GetComponent<PlayerHealth>().getCurrentHealth());
  
    }




    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions, size = 1 (continuous action space for left and right movement)
        float input = actionBuffers.DiscreteActions[0];
     
        if (input == 0)
        {
            Move(-1);
        } else if (input == 1)
        {
            Move(1);
            
        } else if (input == 2)
        {
            Combat();
        }else if (input == 3)
        {
            Jump();
        }


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

        bool endEpisode = false;

        if (this.GetComponent<PlayerHealth>().getCurrentHealth() < previousHealthPlayer)
        {
            SetReward(-0.1f);
            previousHealthPlayer = this.GetComponent<PlayerHealth>().getCurrentHealth();
        }

        if (enemy.GetComponent<Enemy>().getHealth() < previousHealthEnemy)
        {
            SetReward(0.1f);
            previousHealthEnemy = enemy.GetComponent<Enemy>().getHealth();
        }

        if (enemy.GetComponent<Enemy>().getHealth() <= 0)
        {
            SetReward(1.0f);
            endEpisode = true;
        }

        if (this.GetComponent<PlayerHealth>().getCurrentHealth() <= 0)
        {
            endEpisode = true;
        }


        episodeTimer += Time.fixedDeltaTime;  // Increment the timer

        if (episodeTimer >= 30f)
        {
            endEpisode = true;
        }


        if (endEpisode)
        {
            EndEpisode();
            enemy.GetComponent<Enemy>().setHealth(100);
            previousHealthEnemy = 100;
            previousHealthPlayer = 100;
            this.GetComponent<PlayerHealth>().resetCurrentHealth();

            episodeTimer = 0f; // Reset the timer
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
        jump = false;
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
    }

    private void Combat()
    {

        this.gameObject.GetComponent<PlayerCombat>().Attack();
        
    }

    private void Jump() 
    {
        //int moveAction = 0;
        //horizontalMove = moveAction * runSpeed;
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, true);
      
        //TODO
        //Jump mit schwung?
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxisRaw("Horizontal");
    }
}

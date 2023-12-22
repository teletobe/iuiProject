using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int maxHealth = 100;
    private int currentHealth; 
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    public void TakeDamage(int damage){
        currentHealth -= damage;
        Debug.Log("hit for " + damage);

        if(currentHealth <= 0){
            Die();
        }
    }

    void Die(){
        Debug.Log("Enemy Died!");
    }

    public int getHealth()
    {
        return currentHealth;
    }

    public void setHealth(int health)
    {
        this.currentHealth = health;
    }



    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "PlayerAgent")
        {
            PlayerHealth playerHealth = col.gameObject.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(25);
        }
    }
}

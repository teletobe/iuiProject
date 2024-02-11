using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int maxHealth = 100;
    private int currentHealth;
    public Color color;
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
            Debug.Log("EnemyAgent killed");
        }
        GetComponent<SpriteRenderer>().color = Color.red;
        StartCoroutine(changeColorBack());
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
            repelPlayer(col.gameObject);
        }
    }
    
    void repelPlayer(GameObject player)
    {
        float xPos = 0f;
        //nach rechts
        if (player.transform.position.x > this.transform.position.x)
        {
           xPos= this.transform.position.x + 2.5f;
          
        }
        else//nach links
        {
            xPos = this.transform.position.x - 2.5f;
        }

        player.transform.position = new Vector3(xPos, player.transform.position.y, player.transform.position.z);



    }

    IEnumerator changeColorBack()
    {

        yield return new WaitForSeconds(1f);
        GetComponent<SpriteRenderer>().color = color;
    }
}

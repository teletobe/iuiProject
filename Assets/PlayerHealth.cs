using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
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
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("been hit for " + damage);

        if (currentHealth <= 0)
        {
            Die();
        }
   
        GetComponent<SpriteRenderer>().color = Color.blue;
        StartCoroutine(changeColorBack());
    }

    void Die()
    {
        Debug.Log("Player Died!");
    }

    public int getCurrentHealth()
    {
        return currentHealth;
    }

    public void resetCurrentHealth()
    {
        currentHealth = maxHealth;
    }

    IEnumerator changeColorBack()
    {

        yield return new WaitForSeconds(1f);
        GetComponent<SpriteRenderer>().color = color;
    }
}

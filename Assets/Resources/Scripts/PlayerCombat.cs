using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 40;
    public float attackRate = 2f;

    float nextAttackTime = 0.5f;
    public LayerMask enemyLayers;

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        Vector3 attackPosition = attackPoint.position;


        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPosition, attackRange, enemyLayers);
        Debug.Log(hitEnemies.Length);
    }

    void Attack()
    {
        // Invert the direction for adjusting the attack point position
        //Vector3 facingDirection = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        //Vector3 offset = new Vector3(facingDirection.x * 0.5f, 0f, 0f);
        //Vector3 attackPosition = attackPoint.position + offset;
        Vector3 attackPosition = attackPoint.position;


        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPosition, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit " + enemy.name);
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        // Invert the direction for adjusting the attack point position
        //Vector3 facingDirection = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        //Vector3 offset = new Vector3(facingDirection.x * 0.5f, 0f, 0f);
        //Vector3 attackPosition = attackPoint.position + offset;
        Vector3 attackPosition = attackPoint.position;

        // Draw the wire sphere at the adjusted attack point position
        Gizmos.DrawWireSphere(attackPosition, attackRange);
    }
}

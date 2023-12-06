using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideWithEnemy : MonoBehaviour
{

    //public Transform attackPoint;
    public GameObject enemy;

    private float playerZ;
    private float enemyZ;



    // Start is called before the first frame update
    void Start()
    {
        playerZ = transform.position.z;
        enemyZ = enemy.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(enemyZ - playerZ) < 3.5)
        {
            Debug.Log("TOUCHING!!!!");
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]

public class Player : MonoBehaviour
{
    Health health;
    
    void Start()
    {
        health = GetComponent<Health>();
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            health.takeDamage(10f);
        }
    }
}

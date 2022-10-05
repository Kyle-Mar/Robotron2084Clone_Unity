using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof(Health))]
public class Enemy : MonoBehaviour
{
    Health health;

    void Start()
    {
        health = GetComponent<Health>();
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("HELLO");
        if (collision.gameObject.CompareTag("Bullet"))
        {
            health.takeDamage(10f);
        }
    }
}


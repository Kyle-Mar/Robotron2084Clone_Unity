using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof(Health))]
public class Enemy : MonoBehaviour
{
    Health health;
    void Start()
    {
        LevelManager.Instance.AddToEnemyCount(this.gameObject);
        health = GetComponent<Health>();
    }
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if(collision.gameObject.GetComponent<Bullet>().owner.tag != "Enemy")
            {
                health.takeDamage(10f);
            }
        }
    }
}


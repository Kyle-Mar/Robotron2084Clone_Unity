using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour
{


    Health health;
    void Awake()
    {
    }

    
    void Start()
    {
        health = GetComponent<Health>();
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            float newHealth = health.takeDamage(10f);
            if(newHealth != -1)
            {
                LevelManager.LevelManagerInstance.HUDCanvasObject.GetComponentInChildren<HUDHealthBar>().SetHealthBarValue(newHealth, health.maxHealth);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerInputManager))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerDeath))]

public class Player : Singleton<Player>
{


    Health health;


    protected override void OnAwake()
    {
        health = GetComponent<Health>();
    }

    
    void Start()
    {

    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log(collision.gameObject.name); 
        if (collision.gameObject.CompareTag("Enemy"))
        {
            float newHealth = health.takeDamage(10f);
            if(newHealth != -1)
            {
                LevelManager.Instance.HUDCanvasObject.GetComponentInChildren<HUDHealthBar>().SetHealthBarValue(newHealth, health.maxHealth);
            }
        }
    }
}

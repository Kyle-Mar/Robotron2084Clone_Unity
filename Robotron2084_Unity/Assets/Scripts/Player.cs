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

public class Player : MonoBehaviour
{
    public delegate void PlayerHurtAction();
    public static event PlayerHurtAction OnPlayerHurt;

    Health health;


    void Awake()
    {
        health = GetComponent<Health>();
    }

    
    void Start()
    {

    }

    public void SetHealth(float newHealthValue)
    {
        health.setHealth(newHealthValue);
    }

    public float GetHealth(float newHealthValue)
    {
        return health.getHealth();
    }

    private void OnCollisionStay(Collision collision)
    {
        //Debug.Log(collision.gameObject.name); 
        if (collision.gameObject.CompareTag("Enemy"))
        {
            float currentHealth = health.health;
            float newHealth = health.takeDamage(10f);
            if(currentHealth != newHealth)
            {
                OnPlayerHurt?.Invoke();
            }
            if(newHealth != -1)
            {
                LevelManager.Instance.HUDCanvasObject.GetComponentInChildren<HUDHealthBar>().SetHealthBarValue(newHealth, health.maxHealth);
            }
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (collision.gameObject.GetComponent<Bullet>().owner.tag != "Player")
            {
                float currentHealth = health.health;
                float newHealth = health.takeDamage(3f);
                if (currentHealth != newHealth)
                {
                    OnPlayerHurt?.Invoke();
                }
                if (newHealth != -1)
                {
                    LevelManager.Instance.HUDCanvasObject.GetComponentInChildren<HUDHealthBar>().SetHealthBarValue(newHealth, health.maxHealth);
                }
            }
        }
    }
}

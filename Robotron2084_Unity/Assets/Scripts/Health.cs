using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float health = 100f;
    [SerializeField] bool invincibilityFrames;
    public float maxHealth;
    bool canTakeDamage;
    private Coroutine damageCooldownCoroutine;
    Death death;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
        canTakeDamage = true;
        death = GetComponent<Death>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float takeDamage(float damageValue)
    {
        if(health > 0)
        {
            if (invincibilityFrames && damageCooldownCoroutine == null)
            {
                damageCooldownCoroutine = StartCoroutine(DamageCooldown(1f));
            }
            
            if (canTakeDamage)
            {
                health -= damageValue;
                if (health <= 0)
                {
                    death.death();
                }
            }
            return health;
        }
        return -1;
    }

    public float getHealth()
    {
        return health;
    }

    public void setHealth(float newHealthValue)
    {
        if(newHealthValue > maxHealth)
        {
            health = maxHealth;
            return;
        }
        health = newHealthValue;
    }


    IEnumerator DamageCooldown(float delay)
    {
        while(delay > 0)
        {
            delay -= Time.deltaTime;
            yield return null;
            canTakeDamage = false;
        }
        damageCooldownCoroutine = null;
        canTakeDamage = true;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float health = 100f;
    [SerializeField] bool invincibilityFrames;
    float maxHealth;
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

    public void takeDamage(float damageValue)
    {
        if(health > 0)
        {
            if (invincibilityFrames && damageCooldownCoroutine == null)
            {
                damageCooldownCoroutine = StartCoroutine(DamageCooldown(3f));
            }
            
            if (canTakeDamage)
            {
                Debug.Log(this.gameObject.name);
                Debug.Log(health);
                health -= damageValue;
                if (health <= 0)
                {
                    death.death();
                }
            }
        }
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
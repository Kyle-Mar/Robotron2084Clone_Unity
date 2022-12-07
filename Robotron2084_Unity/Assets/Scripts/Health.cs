using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Health : MonoBehaviour
{
    public Renderer rend;
    // Color[] originalMaterialColors;

    public float health = 100f;
    [SerializeField] bool invincibilityFrames;
    public float maxHealth;
    bool canTakeDamage;
    private Coroutine damageCooldownCoroutine;

    Death death;
    Timer hurtEffectTimer;

    // Start is called before the first frame update
    void Start()
    {
        
        maxHealth = health;
        canTakeDamage = true;
        death = GetComponent<Death>();
        hurtEffectTimer = gameObject.AddComponent<Timer>() as Timer;
        hurtEffectTimer.selfDestructive = false;

        /*originalMaterialColors = new Color[rend.materials.Length];
        for (int i = 0; i < rend.materials.Length; i++) {
            originalMaterialColors[i] = rend.materials[i].GetColor("_Color");
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        decrementDamageTint(rend.materials);
    }

    public float takeDamage(float damageValue)
    {
        if (health > 0)
        {
            if (invincibilityFrames && damageCooldownCoroutine == null)
            {
                damageCooldownCoroutine = StartCoroutine(DamageCooldown(1f));
            }

            if (canTakeDamage)
            {
                setDamageTint(rend.materials, .9f);
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

    private void setDamageTint(Material[] materials, float alphaValue)
    {
        foreach (Material mat in rend.materials)
        {
            if (!mat.HasProperty("_Tint"))
            {
                continue;
            }
            Color tintColor = mat.GetColor("_Tint");
            mat.SetColor("_Tint", new Color(tintColor.r, tintColor.g, tintColor.b, alphaValue));
        }
    }

    private void decrementDamageTint(Material[] materials)
    {
        foreach (Material mat in rend.materials)
        {
            if (!mat.HasProperty("_Tint"))
            {
                continue;
            }
            Color tintColor = mat.GetColor("_Tint");
            if(tintColor.a < 0)
            {
                continue;
            }
            mat.SetColor("_Tint", new Color(tintColor.r, tintColor.g, tintColor.b, tintColor.a - Time.deltaTime));
        }
    }

    public void setHealth(float newHealthValue)
    {
        if (newHealthValue > maxHealth)
        {
            health = maxHealth;
            return;
        }
        health = newHealthValue;
    }


    IEnumerator DamageCooldown(float delay)
    {
        while (delay > 0)
        {
            delay -= Time.deltaTime;
            yield return null;
            canTakeDamage = false;
        }
    damageCooldownCoroutine = null;
        canTakeDamage = true;
    }
}

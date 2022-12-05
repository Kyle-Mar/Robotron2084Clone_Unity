using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDHealthBar : MonoBehaviour
{
    public Slider healthBar;
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Health playerHealthComponent = player.gameObject.GetComponent<Health>();
        healthBar.value = playerHealthComponent.getHealth() / playerHealthComponent.maxHealth;
    }

    public void SetHealthBarValue(float health, float maxHealth)
    {
        healthBar.value = health / maxHealth;
    }
}

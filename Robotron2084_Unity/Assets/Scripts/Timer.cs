using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Inspired in part by https://www.youtube.com/watch?v=3ZfwqWl-YI0
public class Timer : MonoBehaviour
{

    private System.Action timerCallback;
    public float maxTimer { get; private set; }
    public float timer { get; private set; }
    private bool looping;
    public bool active { get; private set; }
    public bool selfDestructive {get; set;}


    public void SetTimer(float timer, System.Action timerCallback, bool looping)
    {
        this.maxTimer = timer;
        this.timer = timer;
        this.timerCallback = timerCallback;
        this.looping = looping;
        this.active = true;
    }

    public void SetActive(bool activeNotActive)
    {
        active = activeNotActive;
    }
    

    private void Update()
    {
        if (!active)
        {
            return;
        }
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0f)
        {
            if (timerCallback != null)
            {
                timerCallback();
            }
            if (looping)
            {
                timer = maxTimer;
            }
            else
            {
                if (selfDestructive)
                {
                    Destroy(this);
                }
                else
                {
                    active = false;
                }
            }
        }
    }
}

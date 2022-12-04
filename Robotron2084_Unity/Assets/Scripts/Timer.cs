using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Inspired in part by https://www.youtube.com/watch?v=3ZfwqWl-YI0
public class Timer : MonoBehaviour
{

    private System.Action timerCallback;
    private float maxTimer;
    private float timer;
    private bool looping;


    public void SetTimer(float timer, System.Action timerCallback, bool looping)
    {
        this.maxTimer = timer;
        this.timer = timer;
        this.timerCallback = timerCallback;
        this.looping = looping;
    }

    private void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }
        if(timer <= 0f)
        {
            timerCallback();
            if (looping)
            {
                timer = maxTimer;
            }
            else
            {
                Destroy(this);
            }
        }
    }
}

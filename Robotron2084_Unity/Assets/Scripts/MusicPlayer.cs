using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicPlayer : Singleton<MusicPlayer>
{
    public AudioMixer mixer;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPitch(float pitch)
    {
        GetComponent<AudioSource>().pitch = pitch;
    }
}

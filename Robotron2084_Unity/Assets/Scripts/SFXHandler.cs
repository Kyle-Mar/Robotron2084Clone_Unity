using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXHandler : Singleton<SFXHandler>
{
    public AudioMixer mixer;
    public AudioMixerGroup mixerGroup;

    void Start()
    {
        mixer = Resources.Load<AudioMixer>("Mixer");
        mixerGroup = mixer.FindMatchingGroups("SoundEffects")[0];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySFX(AudioClip clip)
    {
        GameObject SFXObject = new GameObject("SoundEffect");
        Play(SFXObject, clip);
    }

    //spatial override
    public void PlaySFX(AudioClip clip, Vector3 position)
    {
        GameObject SFXObject = new GameObject("SoundEffect");
        SFXObject.transform.position = position;
        Play(SFXObject, clip);

    }

    private void Play(GameObject SFXObject, AudioClip clip)
    {
        AudioSource source = SFXObject.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = mixerGroup;
        source.clip = clip;
        source.volume = PlayerPrefs.GetFloat("EffectsVolume");
        source.pitch = Random.Range(0.75f, 1.0f);
        source.Play();
        //StartCoroutine(DestroyAfterClipLength(clip.length, SFXObject));
        Timer clipTimer = SFXObject.AddComponent<Timer>() as Timer;
        clipTimer.SetTimer(clip.length, () => { Destroy(SFXObject); }, false);
    }
}
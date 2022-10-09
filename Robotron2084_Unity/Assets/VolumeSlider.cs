using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("MusicVolume");
    }
    public void OnVolumeChanged()
    {
        LevelManager.LevelManagerInstance.SetMusicVolume(slider.value);
        PlayerPrefs.SetFloat("MusicVolume", slider.value);
    }
}

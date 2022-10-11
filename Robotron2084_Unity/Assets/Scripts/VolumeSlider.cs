using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider slider;
    public string targetMixerParamString;

    void Start()
    {
        slider.value = PlayerPrefs.GetFloat(targetMixerParamString);
    }
    public void OnVolumeChanged()
    {
        LevelManager.LevelManagerInstance.SetMixerGroupVolume(targetMixerParamString, slider.value);
        PlayerPrefs.SetFloat(targetMixerParamString, slider.value);
    }
}

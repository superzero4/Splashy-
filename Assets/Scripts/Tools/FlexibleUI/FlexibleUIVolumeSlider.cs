using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FlexibleUIVolumeSlider : FlexibleUISlider
{
    [SerializeField]protected AudioMixerGroup _audioGroupToUpdate;
    // Start is called before the first frame update
    protected override void Reset()
    {
        base.Reset();
        _slider.onValueChanged.AddListener((f) => updateVolume(f));
        _slider.minValue = -80;
        _slider.maxValue = 0;
    }

    public void updateVolume(float f)
    {
        _audioGroupToUpdate.audioMixer.SetFloat(_audioGroupToUpdate.name+"Vol", f);        
    }
}

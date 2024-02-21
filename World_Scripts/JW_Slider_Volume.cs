using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JW_Slider_Volume : MonoBehaviour
{

    public Slider volumeSlider;


    public void OnVolumeChanged()
    {
        AudioListener.volume = volumeSlider.value;
    }

}

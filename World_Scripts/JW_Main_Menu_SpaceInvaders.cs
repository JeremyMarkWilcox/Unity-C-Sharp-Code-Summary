using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JW_Main_Menu_SpaceInvaders : MonoBehaviour
{
    private JW_Slider_Volume volumeSlider;

    void Start()
    {
        AudioListener.volume = 0.5f;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("JW-Level 1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
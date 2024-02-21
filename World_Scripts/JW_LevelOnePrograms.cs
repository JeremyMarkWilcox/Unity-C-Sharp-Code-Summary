using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JW_LevelOnePrograms : MonoBehaviour
{
    public GameObject JW_Menu_Box;

    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        if (JW_Menu_Box != null)
        {
            bool isActive = JW_Menu_Box.activeSelf;
            JW_Menu_Box.SetActive(!isActive);

            if (!isActive)
            {
                Time.timeScale = 0f;
                Cursor.visible = true;
            }
            else
            {
                Time.timeScale = 1f;
                Cursor.visible = false;
            }
        }
    }

    void LateUpdate()
    {
        if (JW_Menu_Box != null && JW_Menu_Box.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene("JW-Level 1");
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                Time.timeScale = 1f; 
                SceneManager.LoadScene("JW-Title");
            }
        }
    }
}

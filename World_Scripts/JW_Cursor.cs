using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JW_Cursor : MonoBehaviour
{
    public Texture2D cursor_normal;
    public Vector2 normalCursorHotSpot;

    public Texture2D cursor_OnButton;
    public Vector2 OnButtonCursorHotSpot;

    void Start()
    {
       Cursor.visible = false;
    }
      
    void Update()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPos;     
    }   

    public void OnButtonCursorEnter()
    {
        Cursor.visible = true;
        Cursor.SetCursor(cursor_OnButton, OnButtonCursorHotSpot, CursorMode.Auto);
    }

    public void OnButtonCursorExit() 
    {
        Cursor.SetCursor(cursor_normal, normalCursorHotSpot, CursorMode.Auto);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class changeCursor
{
    private Texture2D targetCursor, menuCursor;

    public changeCursor(Texture2D target, Texture2D menu)
    {
        targetCursor=target;
        menuCursor=menu;
    }

    // По желанию, вы можете восстановить обычный курсор в другом методе
    void OnDestroy()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }


    public void changeCoursorTarget()
    {
        //Debug.Log("прицел");
        Cursor.SetCursor(targetCursor, Vector2.zero, CursorMode.ForceSoftware);
    }


    public void changeCoursorMenu()
    {
        //Debug.Log("Почему?");
        Cursor.SetCursor(menuCursor, Vector2.zero, CursorMode.ForceSoftware);
    }
}



public class mousePicture : MonoBehaviour
{
    public Texture2D targetCursor, menuCursor;
    static Texture2D targetCursor2, menuCursor2;
    private int windowWidth;
    private Resolution curPos;
    public Text magazineText, bulletText, firstAidText;

    void Start()
    {
        // Загрузка и установка кастомного курсора
        changeCursor ch = new changeCursor(targetCursor, menuCursor);
        ch.changeCoursorTarget();
        targetCursor2=targetCursor;
        menuCursor2=menuCursor;

        Cursor.SetCursor(targetCursor, Vector2.zero, CursorMode.ForceSoftware);

        curPos = Screen.currentResolution;
        windowWidth = Screen.width;

        float fontSize = ((float)curPos.width/42.666f) * ((float)windowWidth/(float)curPos.width);
        magazineText.fontSize = Mathf.RoundToInt(fontSize);
        bulletText.fontSize = Mathf.RoundToInt(fontSize);
        firstAidText.fontSize = Mathf.RoundToInt(fontSize);
    }

    private void FixedUpdate()
    {
        windowWidth = Screen.width;
        float fontSize = ((float)curPos.width/42.666f) * ((float)windowWidth/(float)curPos.width);
        magazineText.fontSize = Mathf.RoundToInt(fontSize);
        bulletText.fontSize = Mathf.RoundToInt(fontSize);
        firstAidText.fontSize = Mathf.RoundToInt(fontSize);
    }

    static public void change_coursor_target()
    {
        changeCursor ch = new changeCursor(targetCursor2, menuCursor2);
        ch.changeCoursorTarget();
    }


    static public void change_coursor_menu()
    {
        changeCursor ch = new changeCursor(targetCursor2, menuCursor2);
        ch.changeCoursorMenu();
    }
}

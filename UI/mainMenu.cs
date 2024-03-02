using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public Texture2D menuCursor;

    private void Start() 
    {
        Cursor.SetCursor(menuCursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void startGame()
    {
        SceneManager.LoadScene("main");
    }

    public void exitGame()
    {
        Debug.Log("Игра закрыта");
        Application.Quit();
    }
}

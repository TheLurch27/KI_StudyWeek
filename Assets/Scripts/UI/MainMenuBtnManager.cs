using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBtnManager : MonoBehaviour
{
    // Exits the game or playmode
    public void QuitButton()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }

    public void StartGameButton() => SceneManager.LoadSceneAsync(1);
}

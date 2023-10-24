using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void OnStartPressed()
    {
        SceneManager.LoadScene("Lobby_scene");
    }

    public void OnQuitPressed()
    {
        Application.Quit();
    }
}

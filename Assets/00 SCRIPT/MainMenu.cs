using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene("GameScene");

    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MenuScene");

    }

    public void LoadIns()
    {
        SceneManager.LoadScene("InstructionScene");

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

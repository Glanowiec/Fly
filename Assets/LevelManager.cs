using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    //ex5
    public void StartGame()
    {
        SceneManager.LoadScene("fly");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

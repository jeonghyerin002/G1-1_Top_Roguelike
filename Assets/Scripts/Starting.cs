using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Starting : MonoBehaviour
{
    public string gameStart;

    public void StartButton()
    {
        SceneManager.LoadScene(gameStart);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void DyingMessage()
    {
        SceneManager.LoadScene(gameStart);
    }

}

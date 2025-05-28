using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelObject : MonoBehaviour
{
    public string dyingMessage;

    public void MoveToDyingMessage()
    {
        SceneManager.LoadScene(dyingMessage);

        Debug.Log("½ÇÆÐ!");
    }
}

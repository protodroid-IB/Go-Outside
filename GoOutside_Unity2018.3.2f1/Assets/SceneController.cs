using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    #region singleton instance
    public static SceneController instance;

    private void MakeSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(this);
        }
        else instance = this;
    }
    #endregion

    private void Awake()
    {
        MakeSingleton();
    }

    public void ChangeScene(string inSceneName)
    {
        SceneManager.LoadScene(inSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

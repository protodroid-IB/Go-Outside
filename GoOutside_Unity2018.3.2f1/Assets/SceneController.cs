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

    private string sceneTochangeTo = "";
    private SceneFader sceneFader;

    private void Awake()
    {
        MakeSingleton();
    }

    private void Start()
    {
        sceneFader = GetComponentInChildren<SceneFader>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void ChangeScene(string inSceneName)
    {
        sceneTochangeTo = inSceneName;
        sceneFader.FadeToBlack();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        sceneFader.FadeFromBlack();
    }

    public void ChangeSceneBackend()
    {
        if(sceneTochangeTo != "")
        {
            Debug.Log(sceneTochangeTo);
            SceneManager.LoadScene(sceneTochangeTo);
        }
        
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    public void QuitGame(string inString)
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

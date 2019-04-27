using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    private Animator fadeAnimator;
    private SceneController sceneController;

    private void Start()
    {
        fadeAnimator = GetComponent<Animator>();
        sceneController = GetComponentInParent<SceneController>();
    }

    public void FadeToBlack()
    {
        fadeAnimator.SetBool("Black", true);
    }

    public void FadeFromBlack()
    {
        fadeAnimator.SetBool("Black", false);
    }

    public void ChangeScene()
    {
        sceneController.ChangeSceneBackend();
    }

    public Animator GetAnimator()
    {
        return fadeAnimator;
    }
}

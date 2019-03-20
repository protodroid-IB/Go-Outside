using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{

    [SerializeField]
    private GameObject pauseMenuGO;

    private bool pauseActive = false;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenuGO.SetActive(false);
        GlobalReferences.instance.playerInteract.pauseInteract += Pause;
    }

    public bool IsPauseActive()
    {
        return pauseActive;
    }

    private void Pause()
    {
        pauseMenuGO.SetActive(true);
        pauseActive = true;
        GlobalReferences.instance.playerInteract.pauseInteract -= Pause;
        GlobalReferences.instance.playerInteract.pauseInteract += UnPause;
    }

    private void UnPause()
    {
        pauseMenuGO.SetActive(false);
        pauseActive = false;
        GlobalReferences.instance.playerInteract.pauseInteract += Pause;
        GlobalReferences.instance.playerInteract.pauseInteract -= UnPause;
    }
}

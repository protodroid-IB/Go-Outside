using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    private DynamicMusicManager dynamicMusicManager;

    private AudioSource peacefulMusicSource;

    private void Start()
    {
        peacefulMusicSource = GetComponent<AudioSource>();
        dynamicMusicManager = GetComponent<DynamicMusicManager>();
    }

    public void StartGame()
    {
        peacefulMusicSource.Stop();
        peacefulMusicSource.enabled = false;

        if(dynamicMusicManager.enabled == false)
            dynamicMusicManager.enabled = true;

        if(dynamicMusicManager.GetMusicGameObject() != null)
            dynamicMusicManager.GetMusicGameObject().SetActive(true);
    }

    public void StopGame()
    {
        peacefulMusicSource.Stop();
        peacefulMusicSource.enabled = true;
        dynamicMusicManager.GetMusicGameObject().SetActive(false);
    }

}

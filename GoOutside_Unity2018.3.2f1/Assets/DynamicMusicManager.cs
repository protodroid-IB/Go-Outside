using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicMusicManager : MonoBehaviour
{
    private GameObject musicGO;

    [System.Serializable]
    public class Track
    {
        private AudioSource audioSource;

        [SerializeField]
        private AudioClip clip;

        [Range(0f,1f)]
        public float startPoint, minVolume, maxVolume;

        private float volumeDistance;

        [HideInInspector]
        private float currentVolume = 0f;

        public void SetUpTrack(GameObject inParent)
        {
            audioSource = inParent.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.Play();
        }

        public void UpdateVolume(float inCutoff)
        {
            if((1f - inCutoff) >= startPoint)
            {
                audioSource.volume = maxVolume; 
            }
        }

        public void SetVolume(float inVolume)
        {
            currentVolume = inVolume;
        }

        public float GetVolume()
        {
            return currentVolume;
        }

        public AudioSource GetAudioSource()
        {
            return audioSource;
        }
    }

    [SerializeField]
    private Track[] tracks;


    // Start is called before the first frame update
    void Start()
    {
        FindMusicGameobject();
        
        foreach(Track track in tracks)
        {
            track.SetUpTrack(musicGO);
        }
    }

    private void FindMusicGameobject()
    {
        int i = 0;
        while (musicGO == null)
        {
            if (transform.GetChild(i).tag == "Music") musicGO = transform.GetChild(i).gameObject;
            else i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(GlobalReferences.instance != null)
        {
            for (int i = 0; i < tracks.Length; i++)
            {
                tracks[i].UpdateVolume(GlobalReferences.instance.resourceManager.GetMentalState());
            }
        }
        
    }
}

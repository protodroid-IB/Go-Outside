using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfVisionLogic : MonoBehaviour
{
    public delegate void Detected();
    public Detected detected;
    public Detected notDetected;

    private Material material;

    [SerializeField]
    private float flashAmplitude = 1f;

    [SerializeField]
    private float flashFrequency = 2f;

    private float intensity = 0f;

    private bool flash = false;

    [HideInInspector]
    public bool isDetected = false;

    private void Start()
    {
        material = GetComponent<MeshRenderer>().material;

        detected += ToggleFlash;
        notDetected += NoFlash;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PlayerModel"))
        {
            if(detected != null)
            {
                detected.Invoke();
                isDetected = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerModel"))
        {
            if (notDetected != null)
            {
                notDetected.Invoke();
                isDetected = false;
            }
        }
    }

    private void Update()
    {
        if(flash)
        {
            Flash();
        }
    }

    private void ToggleFlash()
    {
        flash = !flash;
    }

    private void Flash()
    {
        intensity = 0.5f * flashAmplitude * Mathf.Sin(flashFrequency * Time.time - Mathf.PI * 0.5f) + flashAmplitude * 0.5f;
        material.SetFloat("_FlashIntensity", intensity);
    }

    private void NoFlash()
    {
        ToggleFlash();
        intensity = 0f;
        material.SetFloat("_FlashIntensity", intensity);
    }

    private void OnDestroy()
    {
        detected -= ToggleFlash;
        notDetected -= NoFlash;
    }

}

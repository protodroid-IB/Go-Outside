using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RedImageEffect : MonoBehaviour
{
    [Range(0f, 1f)]
    public float intensity;

    [Range(0.001f, 1f)]
    public float intensityScale = 0.01f;

    public Material material;


    private void Start()
    {
        intensity = 0f;
    }

    public void TriggerEffect(float inDuration)
    {
        StartCoroutine(Countdown(inDuration));
    }

    private IEnumerator Countdown(float inDuration)
    {
        float normalizedTime = 0f;
        
        while (normalizedTime <= 1f)
        {
            intensity = 1f - normalizedTime;
            Mathf.Clamp01(intensity);
            normalizedTime += Time.deltaTime / inDuration;
            yield return null;
        }
    }


    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(material != null)
        {
           

            material.SetFloat("_Amount", intensity * intensityScale);
            Graphics.Blit(source, destination, material);
        }
        
    }

    private void OnDestroy()
    {
        intensity = 0f;
    }
}

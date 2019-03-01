using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveRoof : MonoBehaviour
{

    private bool dissolve = false;

    private bool fadeIn = false;
    private bool fadeOut = false;

    [SerializeField]
    private AnimationCurve curve;

    [SerializeField]
    private float dissolveSpeed = 1f;

    private float dissolveProgress = 0f;

    [SerializeField]
    private MeshRenderer meshRenderer;


    // Update is called once per frame
    void Update()
    {
        if(dissolve)
        {
            if (fadeIn)
            {
                dissolveProgress = curve.Evaluate(Time.time * dissolveSpeed * Time.deltaTime);

                meshRenderer.sharedMaterial.SetFloat("_DissolveAmount", dissolveProgress);

                if(dissolveProgress >= 1f)
                {
                    fadeIn = false;
                }
            }
        }
        else
        {
            if(fadeOut)
            {
                
                dissolveProgress = -curve.Evaluate(Time.time * dissolveSpeed * Time.deltaTime);

                meshRenderer.sharedMaterial.SetFloat("_DissolveAmount", dissolveProgress);

                if (dissolveProgress <= 0f)
                {
                    fadeOut = false;
                    dissolve = false;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("PlayerModel") && dissolve == false)
        {
            dissolve = true;
            fadeIn = true;

            Debug.Log("COLLISION!!!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerModel"))
        {
            fadeOut = true;
        }
    }
}

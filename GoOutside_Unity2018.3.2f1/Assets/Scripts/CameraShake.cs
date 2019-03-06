using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    [SerializeField]
    private bool active = true;

    [SerializeField]
    private Camera camera = null;

    [Space(5)]
    [Header("Shake Intensity")]
    [SerializeField]
    private float traumaMultiplier = 10f;

    [Space(5)]
    [Header("Shake Distance Range")]
    [SerializeField]
    private float traumaMagnitude = 2f;

    [Space(5)]
    [Header("Trauma Drop Off")]
    [SerializeField]
    private float traumaDropOff = 4f;

    public float trauma;

    private float time = 0f;

    private float cameraDepth;





    private void Start()
    {
        cameraDepth = camera != null ? cameraDepth = camera.transform.localPosition.z : cameraDepth = 0;
    }


    public float GetTrauma()
    {
        return trauma;
    }

    public void SetTrauma(float inTrauma)
    {
        trauma = Mathf.Clamp01(inTrauma);
    }

    public void AddToTrauma(float inAdd)
    {
        trauma += inAdd;
        Mathf.Clamp01(trauma);
    }

    public void SubtractFromTrauma(float inSubtract)
    {
        trauma -= inSubtract;
        Mathf.Clamp01(trauma);
    }


    public void SetCamera(Camera inCamera)
    {
        camera = inCamera;
        cameraDepth = camera.transform.localPosition.z;
    }





    // returns a float between -1 and 1 based on time and perlin noise
    private float GetPerlinNoiseNum(float inSeed)
    {
        return (Mathf.PerlinNoise(inSeed, time) - 0.5f) * 2f;
    }

    // generate vector3 of perline noise values between -1 and 1 from seed
    private Vector3 GenerateVector3()
    {
        return new Vector3(GetPerlinNoiseNum(Random.Range(0f, 100f)), GetPerlinNoiseNum(Random.Range(0f, 100f)));
    }

    private void LateUpdate()
    {
        if (camera != null)
        {
            if (active == true && trauma > 0.00025)
            {
                time += Mathf.Pow(trauma, 0.3f) * traumaMultiplier * Time.deltaTime;

                Vector3 newPosition = GenerateVector3() * traumaMagnitude * trauma;
                newPosition = new Vector3(newPosition.x, newPosition.y, 0f);

                camera.transform.localPosition += newPosition;

                SubtractFromTrauma(trauma * traumaDropOff * Time.deltaTime);
            }
            else
            {
                trauma = 0f;
                //Vector3 newPosition = Vector3.Lerp(camera.transform.localPosition, Vector3.zero, Time.deltaTime);
                //newPosition = new Vector3(newPosition.x, newPosition.y, cameraDepth);
                //camera.transform.localPosition = newPosition;
            }
        }

    }

    public void SetTraumaMagnitude(float inMag)
    {
        traumaMagnitude = inMag;
    }

    public void SetTraumaMultiplier(float inMult)
    {
        traumaMultiplier = inMult;
    }

    public void SetTraumaDropOff(float inDropOff)
    {
        traumaDropOff = inDropOff;
    }





}
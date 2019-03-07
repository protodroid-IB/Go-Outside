using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SphereMask : MonoBehaviour
{
    public float minRadius = 5f, maxRadius = 75f;
    private float radius, radiusDistance;

    public bool debugMode = false;

    private bool updateRadius = false;


    private void Start()
    {
        radius = maxRadius;
        radiusDistance = maxRadius - minRadius;
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    float radius;

    //    if (ossiclateEffect)
    //        radius = (maxRadius / 2) * Mathf.Sin(Time.time * speed / 100f) + (maxRadius / 2f);
    //    else
    //        radius = maxRadius;


    //    Shader.SetGlobalFloat("GLOBALMASK_Radius", radius);
    //    Shader.SetGlobalVector("GLOBALMASK_Position", new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z));
    //}

    private void Update()
    {

    }



    public void UpdateMask(float inMentalState)
    {
        if (!debugMode)
            radius = minRadius + (radiusDistance * inMentalState);
        else
            radius = maxRadius;

        if (radius <= minRadius) radius = minRadius;

        

        Shader.SetGlobalFloat("GLOBALMASK_Radius", radius);
        Shader.SetGlobalVector("GLOBALMASK_Position", new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z));
    }

    private void OnValidate()
    {
        UpdateMask(1f);
    }
}

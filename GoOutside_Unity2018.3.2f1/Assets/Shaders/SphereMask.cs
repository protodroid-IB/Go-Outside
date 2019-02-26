using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SphereMask : MonoBehaviour
{
    public float maxRadius = 2f;
    public float speed = 10f;

    public bool ossiclateEffect = false;


    // Update is called once per frame
    void Update()
    {
        float radius;

        if (ossiclateEffect)
            radius = (maxRadius / 2) * Mathf.Sin(Time.time * speed / 100f) + (maxRadius / 2f);
        else
            radius = maxRadius;


        Shader.SetGlobalFloat("GLOBALMASK_Radius", radius);
        Shader.SetGlobalVector("GLOBALMASK_Position", new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z));
    }
}

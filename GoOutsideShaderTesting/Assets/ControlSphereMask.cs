using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ControlSphereMask : MonoBehaviour
{
    [SerializeField]
    private float radius = 1f;


    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalFloat("_Radius", radius);
        Shader.SetGlobalVector("_Position", transform.position);
    }
}

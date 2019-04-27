using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildingController : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer roofMesh;

    private Material[] roofMats;

    [SerializeField]
    private bool startRevealed = false;

    private bool activateDissolveLerp = false;


    private float beginDissolve = 0.0f;
    private float targetDissolve = 1.0f;
    private float dissolveLerp = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        roofMats = roofMesh.materials;

        if(startRevealed == false)
        {
            for (int i = 0; i < roofMats.Length; i++)
            {
                roofMats[i].SetFloat("_DissolveRoofBool", 0.0f);
            }
        }
        else
        {
            for (int i = 0; i < roofMats.Length; i++)
            {
                roofMats[i].SetFloat("_DissolveRoofBool", 1.0f);
            }

            ActivateDissolveLerp();
        }
        
        
    }


    private void Update()
    {
        if (activateDissolveLerp)
        {
            if (Mathf.Abs(targetDissolve - 0.0f) <= 0.001f)
            {
                dissolveLerp = Mathf.Lerp(dissolveLerp, targetDissolve, 3.0f * Time.deltaTime);
            }
            else
            {
                dissolveLerp = Mathf.Lerp(dissolveLerp, targetDissolve, Time.deltaTime);
            }

            for(int i=0; i < roofMats.Length; i++)
            {
                roofMats[i].SetFloat("_RoofDissolveAmount", dissolveLerp);
            }

            if (Mathf.Abs(dissolveLerp - 1.0f) <= 0.001f)
            {
                activateDissolveLerp = false;

                if(Mathf.Abs(targetDissolve - 0.0f) <= 0.001f)
                {
                    SetDissolveRoofBool(false);
                }
            }
        }
    }



    public void SetDissolveRoofBool(bool inBool)
    {
        if(inBool)
        {
            for(int i=0; i < roofMats.Length; i++)
            {
                roofMats[i].SetFloat("_DissolveRoofBool", 1.0f);
            }
           
        }
        else
        {
            for (int i = 0; i < roofMats.Length; i++)
            {
                roofMats[i].SetFloat("_DissolveRoofBool", 0.0f);
            }
        }
    }


    public void SetTargetDissolve(float inTargetDissolve)
    {
        targetDissolve = inTargetDissolve;
    }


    public float GetCurrentDissolve()
    {
        return dissolveLerp;
    }

    public void ActivateDissolveLerp()
    {
        activateDissolveLerp = true;
    }

    public void DissolveRoof()
    {
        for (int i = 0; i < roofMats.Length; i++)
        {
            roofMats[i].SetFloat("_DissolveRoofBool", 1.0f);
        }
        SetTargetDissolve(1f);
        ActivateDissolveLerp();
    }
}

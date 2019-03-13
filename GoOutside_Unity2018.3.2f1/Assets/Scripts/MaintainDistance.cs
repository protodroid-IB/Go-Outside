using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MaintainDistance : MonoBehaviour
{

    [SerializeField]
    private GameObject object1, object2;

    [SerializeField]
    private bool isRectTransform = false;

    [SerializeField]
    private float distanceToMaintain = 10f;

    private RectTransform[] rectTrans = new RectTransform[2];

    private void Start()
    {
        if(isRectTransform)
        {
            rectTrans[0] = object1.GetComponent<RectTransform>();
            rectTrans[1] = object2.GetComponent<RectTransform>();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
    }
}

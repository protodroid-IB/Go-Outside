using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class FieldOfVision : MonoBehaviour
{

    // ANGLES AND DATA
    [SerializeField]
    [Header("Angle in degrees: ")]
    [Range(10f, 360f)]
    private float angleDegrees = 25f;

    [SerializeField]
    private float range = 20f;

    private float angleRadians;

    private float angleZeroOne = 0f;

    private float halfAngleZeroOne;

    private bool spottedPlayer = false;



    // VISUALS
    private Image[] visionCircles;

    [SerializeField]
    private Color spotted;

    [SerializeField]
    private Color notSpotted;




    // COLLIDER STUFF
    private SphereCollider visionCollider;












    // Start is called before the first frame update
    void Start()
    {
        angleZeroOne = angleDegrees / 360f;
        halfAngleZeroOne = angleZeroOne / 2f;

        visionCircles = transform.GetChild(0).GetComponentsInChildren<Image>();

        visionCollider = GetComponent<SphereCollider>();

        UpdateVisuals();
        UpdateCollider();
    }

    




    private void UpdateVisuals()
    {
        if(visionCircles != null)
        {
            for (int i = 0; i < visionCircles.Length; i++)
            {
                visionCircles[i].fillAmount = halfAngleZeroOne;
                visionCircles[i].rectTransform.sizeDelta = new Vector2(range, range);
            }
        }
    }

    private void UpdateCollider()
    {
        if(visionCollider != null)
        {
            visionCollider.radius = range/2f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {

        }
    }


    public void SetAngleDegrees(float inDegrees)
    {
        angleDegrees = inDegrees;
        angleRadians = angleDegrees * Mathf.Deg2Rad;

        angleZeroOne = angleDegrees / 360f;
        halfAngleZeroOne = angleZeroOne / 2f;

        UpdateVisuals();
        UpdateCollider();
    }

    public void SetAngleRadians(float inRadians)
    {
        angleRadians = inRadians;
        angleDegrees = angleRadians * Mathf.Rad2Deg;

        angleZeroOne = angleRadians / (2f * Mathf.PI);
        halfAngleZeroOne = angleZeroOne / 2f;

        UpdateVisuals();
        UpdateCollider();
    }

    private void OnValidate()
    {
        SetAngleDegrees(angleDegrees);   
    }
}

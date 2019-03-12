using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
public class FieldOfVisionMesh : MonoBehaviour
{
    [SerializeField]
    [Range(10f, 360f)]
    private float angleDegrees = 25f;

    [SerializeField]
    private float radius = 20f;

    [SerializeField]
    private int meshResolution = 20;

    private float angleRadians;

    private bool spottedPlayer = false;

    private Mesh mesh;
    private MeshFilter meshFilter;

    private Vector3[] vertices;
    private int[] tris;




    // Start is called before the first frame update
    void Start()
    {
        GenerateMesh();
    }

    void GenerateMesh()
    {
        mesh = new Mesh();
        mesh.name = "Field Of View Mesh";
        meshFilter = gameObject.GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        float angleDivisions = angleDegrees / (meshResolution - 1);
        float startingAngle = 0 - (angleDegrees/2f);

        //Debug.Log("Starting angle: " + startingAngle);

        vertices = new Vector3[meshResolution + 1];

        vertices[0] = transform.localPosition ;

        for (int i=1; i < vertices.Length; i++)
        {
            float theta = Mathf.Deg2Rad * (startingAngle + (i - 1) * angleDivisions);
            float xPos = radius * Mathf.Sin(theta);
            float zPos = radius * Mathf.Cos(theta);

            vertices[i] = transform.localPosition + new Vector3(xPos, transform.position.y, zPos);
        }


        int numTris = (meshResolution - 1);
        int triIndex = 0;

        tris = new int[numTris * 3];

        for (int i = 1; i <= numTris; i++)
        {
            tris[triIndex] = i;
            tris[triIndex + 1] = i + 1;
            tris[triIndex + 2] = 0;

            //Debug.Log("TRIANGLE " + i + "\t " + tris[triIndex] + "\t" + tris[triIndex + 1] + "\t" + tris[triIndex + 2]);

            triIndex += 3;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.RecalculateNormals();

        GetComponent<MeshCollider>().sharedMesh = mesh;
        GetComponent<MeshCollider>().isTrigger = true;
    }


    private void OnValidate()
    {
        GenerateMesh();
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject letterPrefab;

    private Transform markerParent;

    private Dictionary<int, GameObject> houseMarkers = new Dictionary<int, GameObject>();

    [SerializeField]
    private GameObject playerMarkerPrefab;

    private GameObject playerMarker;


    private void Start()
    {
        markerParent = transform.GetChild(0);

        playerMarker = Instantiate(playerMarkerPrefab, FindGlobalPosition(GlobalReferences.instance.playerMovement.transform.position), Quaternion.identity, markerParent);
        playerMarker.transform.localRotation = ResetLocalRotation();
    }

    private void Update()
    {
        playerMarker.transform.position = FindGlobalPosition(GlobalReferences.instance.playerMovement.transform.position);
    }


    public void InitialiseHouseMarkers(Dictionary<int, LetterBox> inChosenLetterBoxes)
    {
        foreach(KeyValuePair<int, LetterBox> letterbox in inChosenLetterBoxes)
        {
            Vector3 newPosition;

            if (letterbox.Value.GetHouse().transform.GetComponent<MeshRenderer>())
            {
                newPosition = FindGlobalPosition(letterbox.Value.GetHouse().transform.GetComponent<MeshRenderer>().bounds.center);
            }
            else if(letterbox.Value.GetHouse().transform.GetChild(0).GetComponent<MeshRenderer>())
            {
                newPosition = FindGlobalPosition(letterbox.Value.GetHouse().transform.GetChild(0).GetComponent<MeshRenderer>().bounds.center);
            }
            else
            {
                newPosition = FindGlobalPosition(Vector3.zero);
            }

            GameObject marker = Instantiate(letterPrefab, newPosition, Quaternion.identity, markerParent);

            marker.transform.localRotation = ResetLocalRotation();

            houseMarkers.Add(letterbox.Key, marker);
        }
    }

    public void RemoveLetterMarker(int key)
    {
        GameObject houseMarker;
        houseMarkers.TryGetValue(key, out houseMarker);
        houseMarkers.Remove(key);
        Destroy(houseMarker);
    }



    private Vector3 FindGlobalPosition(Vector3 inStartPosition)
    {
        Vector3 position = new Vector3(inStartPosition.x, transform.position.y, inStartPosition.z);

        return position;
    }

    private Quaternion ResetLocalRotation()
    {
        Quaternion newRot = Quaternion.Euler(0f, 0f, 0f);

        return newRot;
    }
}

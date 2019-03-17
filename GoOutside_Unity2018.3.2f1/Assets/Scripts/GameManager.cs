using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool activateGameLoop = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GlobalReferences.instance.resourceManager.DayEnded && activateGameLoop)
        {
            SceneController.instance.ChangeScene("MainMenu");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalReferences : MonoBehaviour
{
    #region singleton instance
    public static GlobalReferences instance;

    private void MakeSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else instance = this;
    }
    #endregion

    public PlayerInteract playerInteract;

    private void Awake()
    {
        MakeSingleton();
        playerInteract = GameObject.FindWithTag("Player").GetComponent<PlayerInteract>();
    }

}

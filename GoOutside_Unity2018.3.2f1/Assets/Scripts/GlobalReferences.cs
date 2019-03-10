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

    [HideInInspector]
    public PlayerMovementController playerMovement;

    [HideInInspector]
    public SphereMask sphereMask;

    [HideInInspector]
    public PlayerInteract playerInteract;

    [HideInInspector]
    public InputController inputController;

    [HideInInspector]
    public ErrandManager errandManager;

    [HideInInspector]
    public UIManager uiManager;

    [HideInInspector]
    public ResourceManager resourceManager;

    [HideInInspector]
    public CameraShake cameraShake;

    [HideInInspector]
    public UsefulFunctions usefulFunctions;

    public Collider parkZoneCollider;

    [HideInInspector]
    public ChromaticAbberationEffect chromaticAbberationEffect;

    private void Awake()
    {
        MakeSingleton();
        playerInteract = GameObject.FindWithTag("Player").GetComponent<PlayerInteract>();
        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovementController>();
        sphereMask = GameObject.FindWithTag("Player").GetComponent<SphereMask>();
        inputController = GetComponent<InputController>();
        errandManager = GetComponent<ErrandManager>();
        uiManager = GetComponent<UIManager>();
        resourceManager = GetComponent<ResourceManager>();
        cameraShake = GetComponent<CameraShake>();
        usefulFunctions = GetComponent<UsefulFunctions>();
        chromaticAbberationEffect = Camera.main.GetComponent<ChromaticAbberationEffect>();
    }

    private void Start()
    {
        if(playerInteract == null)
            playerInteract = GameObject.FindWithTag("Player").GetComponent<PlayerInteract>();

        if (playerMovement == null)
            playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovementController>();

        if (sphereMask == null)
            sphereMask = GameObject.FindWithTag("Player").GetComponent<SphereMask>();

        if (inputController == null)
            inputController = GetComponent<InputController>();

        if (errandManager == null)
            errandManager = GetComponent<ErrandManager>();

        if (uiManager == null)
            uiManager = GetComponent<UIManager>();

        if (resourceManager == null)
            resourceManager = GetComponent<ResourceManager>();

        if (cameraShake == null)
            cameraShake = GetComponent<CameraShake>();

        if (usefulFunctions == null)
            usefulFunctions = GetComponent<UsefulFunctions>();

        if(chromaticAbberationEffect == null)
            chromaticAbberationEffect = Camera.main.GetComponent<ChromaticAbberationEffect>();

    }

}

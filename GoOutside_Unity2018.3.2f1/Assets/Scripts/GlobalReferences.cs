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

    [HideInInspector]
    public GameManager gameManager;

    [HideInInspector]
    public DialogueManager dialogueManager;

    [HideInInspector]
    public ChoiceManager choiceManager;

    [HideInInspector]
    public MobilePhoneManager mobilePhoneManager;

    

    //public PlayerMovementController PlayerMovement { get => playerMovement;}

    private void Awake()
    {
        MakeSingleton();
        playerInteract = GameObject.FindWithTag("Player").GetComponent<PlayerInteract>();
        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovementController>();
        sphereMask = GameObject.FindWithTag("Player").GetComponent<SphereMask>();
        inputController = GameObject.FindWithTag("Preload").GetComponent<InputController>();
        errandManager = GetComponent<ErrandManager>();
        uiManager = GetComponent<UIManager>();
        resourceManager = GetComponent<ResourceManager>();
        cameraShake = GetComponent<CameraShake>();
        usefulFunctions = GetComponent<UsefulFunctions>();
        chromaticAbberationEffect = Camera.main.GetComponent<ChromaticAbberationEffect>();
        gameManager = GetComponent<GameManager>();
        dialogueManager = GetComponent<DialogueManager>();
        choiceManager = GetComponent<ChoiceManager>();
        mobilePhoneManager = GetComponent<MobilePhoneManager>();

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
            inputController = GameObject.FindWithTag("Preload").GetComponent<InputController>();

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

        if (gameManager == null)
            gameManager = GetComponent<GameManager>();

        if (dialogueManager == null)
            dialogueManager = GetComponent<DialogueManager>();

        if(choiceManager == null)
            choiceManager = GetComponent<ChoiceManager>();

        if(mobilePhoneManager == null)
            mobilePhoneManager = GetComponent<MobilePhoneManager>();
    }

}

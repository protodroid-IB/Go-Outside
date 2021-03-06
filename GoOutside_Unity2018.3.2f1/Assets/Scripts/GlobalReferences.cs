﻿using System.Collections;
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

    //[HideInInspector]
    //public PauseManager pauseManager;

    [HideInInspector]
    public MapCameraMovement mapCameraMovement;

    [HideInInspector]
    public MapUIManager mapUIManager;

    public ExerciseApplication exerciseApplication;

    public MumDialogue mumDialogue;

    [HideInInspector]
    public MusicManager musicManager;

    [HideInInspector]
    public SceneFader sceneFader;

    [HideInInspector]
    public SisterMovement sisterMovement;

    public EndStateUIManager endStateUIManager;



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
        //pauseManager = GetComponent<PauseManager>();
        mapCameraMovement = GameObject.FindWithTag("MapCamera").GetComponent<MapCameraMovement>();
        mapUIManager = GameObject.FindWithTag("MapUI").GetComponent<MapUIManager>();
        musicManager = GameObject.FindWithTag("Preload").GetComponent<MusicManager>();
        sceneFader = GameObject.FindWithTag("Preload").GetComponentInChildren<SceneFader>();
        sisterMovement = GameObject.FindWithTag("Sister").GetComponent<SisterMovement>();

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

        //if(pauseManager == null)
        //    pauseManager = GetComponent<PauseManager>();

        if(mapCameraMovement == null)
            mapCameraMovement = GameObject.FindWithTag("MapCamera").GetComponent<MapCameraMovement>();

        if(mapUIManager == null)
            mapUIManager = GameObject.FindWithTag("MapUI").GetComponent<MapUIManager>();

        if(musicManager == null)
            musicManager = GameObject.FindWithTag("Preload").GetComponent<MusicManager>();

        if(sceneFader == null)
            sceneFader = GameObject.FindWithTag("Preload").GetComponentInChildren<SceneFader>();

        if(sisterMovement == null)
            sisterMovement = GameObject.FindWithTag("Sister").GetComponent<SisterMovement>();
    }

}

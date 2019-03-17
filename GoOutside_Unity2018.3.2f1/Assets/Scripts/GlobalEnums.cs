using System.Collections;
using System.Collections.Generic;

public enum ControllerType { Gamepad, Keyboard };

public enum Errands { DropSister, Workout, DeliverLetters, PatDogs, PickUpSister };

public enum NPCState { Idle, Chasing, Collided, GoingHome, Talking };

public enum DogState { Idle, Walking, Petted };

[System.Serializable]
public struct Choice
{
    public string text;
    public float mentalStateEffect;
}
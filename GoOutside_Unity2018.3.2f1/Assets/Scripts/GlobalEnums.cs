using System.Collections;
using System.Collections.Generic;

public enum ControllerType { Gamepad, Keyboard };

public enum Errands { DropSister, Workout, DeliverLetters, PatDogs, PickUpSister };

public enum NPCState { Idle, Chasing, Collided, GoingHome, Talking };

public enum DogState { Idle, Walking, Petted };

public enum MobilePhoneState { Closed, Alert, Open };

[System.Serializable]
public struct Choice
{
    public string text;
    public float mentalStateEffect;
}

[System.Serializable]
public struct TimeOfDay
{
    [UnityEngine.Range(8,20)]
    public int hour;

    [UnityEngine.Range(0, 59)]
    public int minute;
}
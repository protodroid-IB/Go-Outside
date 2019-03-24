using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ExerciseApplication : MonoBehaviour
{
    [SerializeField]
    private Transform machinesParent;

    [SerializeField]
    private TextMeshProUGUI[] machineNumberUI = new TextMeshProUGUI[6];

    [SerializeField]
    private TextMeshProUGUI[] machineCompleteUI = new TextMeshProUGUI[6];

    [SerializeField]
    private Color completeColor = Color.green;

    private Dictionary<int, GymMachine> chosenMachines = new Dictionary<int, GymMachine>();

    private Dictionary<int, int> uiIndexReference = new Dictionary<int, int>();

    // Start is called before the first frame update
    void Start()
    {
        FindGymMachinesToExerciseOn();
        UpdateNumberUI();
    }



    private void FindGymMachinesToExerciseOn()
    {
        uiIndexReference.Clear();

        for (int i = 0; i < machineNumberUI.Length; i++)
        {
            int rand = UnityEngine.Random.Range(0, machinesParent.childCount);
            GymMachine outMachine = machinesParent.GetChild(rand).GetComponent<GymMachine>();

            while (chosenMachines.ContainsKey(outMachine.GetMachineNumber()))
            {
                rand = UnityEngine.Random.Range(0, machinesParent.childCount);
                outMachine = machinesParent.GetChild(rand).GetComponent<GymMachine>();
            }

            uiIndexReference.Add(outMachine.GetMachineNumber(), i);
            chosenMachines.Add(outMachine.GetMachineNumber(), outMachine);
        }
    }



    private void UpdateNumberUI()
    {
        int i = 0;

        foreach(KeyValuePair<int, GymMachine> machine in chosenMachines)
        {
            machineNumberUI[i].text = machine.Key.ToString();
            i++;
        }
    }


    public bool CheckExerciseComplete(int inMachineNum)
    {
        bool complete = false;

        foreach(KeyValuePair<int, GymMachine> machine in chosenMachines)
        {
            if(machine.Value.GetMachineNumber() == inMachineNum)
            {
                complete = true;
                UpdateMachineCompleteUI(inMachineNum);
            }
        }

        return complete;
    }

    private void UpdateMachineCompleteUI(int inMachineNum)
    {
        int uiIndex = 0;
        uiIndexReference.TryGetValue(inMachineNum, out uiIndex);

        machineCompleteUI[uiIndex].text = "COMPLETE!";
        machineCompleteUI[uiIndex].color = completeColor;
    }
}

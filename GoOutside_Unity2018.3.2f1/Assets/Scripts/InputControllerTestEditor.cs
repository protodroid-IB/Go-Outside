//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//[CustomEditor(typeof(InputControllerTest))]
//public class InputControllerTestEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        InputControllerTest script = (InputControllerTest)target;

//        InsertSpaces(2);
//        EditorGUILayout.LabelField("General Controller Settings", EditorStyles.boldLabel);
//        script.enableControllerChecking = (bool)EditorGUILayout.Toggle("Enable Gamepad Detection", script.enableControllerChecking);
//        script.controllerType = (InputControllerTest.ControllerType)EditorGUILayout.EnumPopup("Controller Type", script.controllerType);


//        InsertSpaces(4);
//        EditorGUILayout.LabelField("Input Types", EditorStyles.boldLabel);
//        EditorGUI.indentLevel++;

//        InputControllerTest.Control _moveControl = (InputControllerTest.Control)script.moveControl;

        
//        if (script.controllerType == InputControllerTest.ControllerType.Gamepad)
//        {
//            _moveControl.gp_Axis = (InputControllerTest.GP_Axis)EditorGUILayout.EnumPopup("Move", _moveControl.gp_Axis);
//        }
//        else
//        {
//            _moveControl.kb_Axis = (InputControllerTest.KB_Axis)EditorGUILayout.EnumPopup("Move", _moveControl.kb_Axis);
//        }

//        EditorGUI.indentLevel--;
//        EditorGUI.indentLevel--;
//    }

//    public void InsertSpaces(int num)
//    {
//        for (int i = 0; i < num; i++)
//        {
//            EditorGUILayout.Space();
//        }
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UberEventManager))]
public class MainEventWriter : Editor
{
    UberEventManager _myScrip;
    string filePath = "Assets/Scripts/Managers/EventEnums/";
    string fileName = "MainNarrativeBeats";
 
    private void OnEnable()
    {
        _myScrip = (UberEventManager)target;
    }
 
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if(GUILayout.Button("Save"))
        {
            EditorMethods.WriteToEnum(filePath, fileName, _myScrip.eventEnums);
        }
    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UberEventManager))]
public class MainEventWriter : Editor
{
    private UberEventManager _myScrip;
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

        if (GUILayout.Button("Update"))
        {
            var narrativeEnums = new List<string>();
            foreach (var beat in Enum.GetNames(typeof(MainNarrativeBeats)))
            {
                narrativeEnums.Add(beat);
            }
            _myScrip.eventEnums = narrativeEnums;
        }
    }
}


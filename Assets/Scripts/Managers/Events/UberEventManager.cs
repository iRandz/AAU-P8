using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class MyEnumEvent : UnityEvent<MainNarrativeBeats>
{
}

public class UberEventManager : NarrativeEvent
{

    [SerializeField] public List<string> eventEnums;

    [System.Serializable] public class MainEventContainer
    {
        public MainNarrativeBeats narrativeBeat;
        public SubEventManager subManager;
        public UnityEvent anEvent;
    }
    
    public List<MainEventContainer> mainEvents;

    [Tooltip("The narrative beat which activates the logging. Standard setting is 'End'. " +
             "Can be used to test logging without playing the game through.")]
    [SerializeField] private MainNarrativeBeats LoggingBeat = MainNarrativeBeats.End;
    
    
    void Awake()
    {
        if (FindObjectsOfType<UberEventManager>().Length > 1)
        {
            Debug.LogError("More than one UberEventManager was found. One was deleted.");
            Destroy(this);
        }

        for (int i = 0; i < mainEvents.Count; i++)
        {
            if (!mainEvents[i].subManager)
            {
                Debug.LogError("Element " + i + " in the UberEventManager's MainEvents was not associated with a SubEventManager.");
            }
        }

    }

    public void ChangeNarrativeBeat(MainNarrativeBeats beatIn)
    {
        UberState = beatIn;
        foreach (var mainEvent in mainEvents)
        {
            if (beatIn == mainEvent.narrativeBeat)
            {
                mainEvent.subManager.OnSelected();
                mainEvent.anEvent.Invoke();
                break;
            }
            mainEvent.subManager.OnDeselected();
        }
        
        UpdateLogs();
        if (beatIn == LoggingBeat)
        {
            SaveLogs();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class tutorialChoiceEvent : NarrativeEvent
{
    private float lookedAtForSec = 0;
    [SerializeField][Tooltip("Time the target should be looked at in seconds.")] private float lookedAtTargetInSec;

    [SerializeField] private UnityEvent terminateEvent;
    private bool eventRun;
    private void Update()
    {
        if (lookedAtForSec > lookedAtTargetInSec && !eventRun)
        {
            //Debug.Log("gase");
            //ChangeMainNarrativeBeat();
            terminateEvent.Invoke();
            eventRun = true;
        }
    }

    public void WereLookedAt()
    {
        lookedAtForSec += Time.deltaTime;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LookedBackScript : NarrativeEvent
{

    [SerializeField] private UnityEvent LookedBackNoneEvent;
    [SerializeField] private UnityEvent LookedBackSomeEvent;
    
    
    private bool active = false;
    private bool HasRun = false;

    private void CheckEventCondition()
    {
        switch (choiceState)
        {
            case storyChoices.None:
                LookedBackNoneEvent.Invoke();
                break;
            default:
                LookedBackSomeEvent.Invoke();
                break;
        }
    }
    
    public void ActivateEvent()
    {
        active = true;
    }

    public void LookedBack()
    {
        if (active && !HasRun)
        {
            CheckEventCondition();
            HasRun = true;
        }
    }
}

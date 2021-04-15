using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainEventArea2 : NarrativeEvent
{
    [SerializeField] private UnityEvent EventCompleted;
    [SerializeField] private UnityEvent TVandObject1Event;
    [SerializeField] private UnityEvent TVandObject2Event;
    [SerializeField] private UnityEvent Object1Event;
    [SerializeField] private UnityEvent Object2Event;
    [SerializeField] private UnityEvent TvEvent;
    [SerializeField] private UnityEvent NoneEvent;
    
    public void LookedAt1()
    {
        EventCompleted.Invoke();
        switch (choiceState)
        {
            case storyChoices.None:
                choiceState = storyChoices.Object1;
                Object1Event.Invoke();
                break;
            case storyChoices.TV:
                choiceState = storyChoices.TVObject1;
                TVandObject1Event.Invoke();
                break;
        }
    }
    public void LookedAt2()
    {
        EventCompleted.Invoke();
        switch (choiceState)
        {
            case storyChoices.None:
                choiceState = storyChoices.Object2;
                Object2Event.Invoke();
                break;
            case storyChoices.TV:
                choiceState = storyChoices.TVObject2;
                TVandObject2Event.Invoke();
                break;
        }
    }
    public void LookedAtNone()
    {
        EventCompleted.Invoke();
        switch (choiceState)
        {
            case storyChoices.None:
                choiceState = storyChoices.None;
                NoneEvent.Invoke();
                break;
            case storyChoices.TV:
                choiceState = storyChoices.TV;
                TvEvent.Invoke();
                break;
        }
    }
}

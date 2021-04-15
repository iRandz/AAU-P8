using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TVscript : NarrativeEvent
{
    [SerializeField] private float duration;
    [SerializeField] private float interval;

    [SerializeField] private MyFloatFloatEvent startTVSequence;
    [SerializeField] private UnityEvent looked;
    [SerializeField] private UnityEvent didntLook;

    public void StartTV()
    {
        startTVSequence.Invoke(duration, interval);
    }

    public void TheyLooked()
    {
        choiceState = storyChoices.TV;
        looked.Invoke();
    }

    public void TheyDidNotLook()
    {
        choiceState = storyChoices.None;
        didntLook.Invoke();
    }
}

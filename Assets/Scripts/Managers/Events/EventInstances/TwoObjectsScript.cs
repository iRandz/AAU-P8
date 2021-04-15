using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TwoObjectsScript : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private float interval;

    [SerializeField] private MyFloatFloatEvent startTwoChoiceEvent;

    [SerializeField] private UnityEvent IgnoredBothEvent;
    [SerializeField] private UnityEvent LookedAt1Event;
    [SerializeField] private UnityEvent LookedAt2Event;

    private int DidNotLookCount = 0;
    
    public void StartEvent()
    {
        startTwoChoiceEvent.Invoke(duration, interval);
    }

    public void PlayerLookedAt1()
    {
        LookedAt1Event.Invoke();
    }
    
    public void PlayerLookedAt2()
    {
        LookedAt2Event.Invoke();
    }

    public void DidNotLook()
    {
        DidNotLookCount += 1;
        if (DidNotLookCount >1)
        {
            IgnoredBothEvent.Invoke();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class NarrativeEvent : MonoBehaviour
{
    [SerializeField] protected bool debugMode;

    [SerializeField] protected bool mustWait;
    [SerializeField] private MainNarrativeBeats nextNarrativeBeat;
    private UberEventManager manager;

    [SerializeField] private float EventDelay;
    [SerializeField] private UnityEvent DelayedEvent;
    
    public enum storyChoices
    {
        None,
        TV,
        TVObject1,
        TVObject2,
        Object1,
        Object2,
    }

    protected static storyChoices choiceState;

    protected static MainNarrativeBeats UberState;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<UberEventManager>();
        if (mustWait)
        {
            enabled = !mustWait;
        }
    }

    private void OnEnable()
    {
        if (mustWait)
        {
            enabled = !mustWait;
        }
    }

    private IEnumerator DelayEvent()
    {
        yield return new WaitForSeconds(EventDelay);
        DelayedEvent.Invoke();
    }

    public void ChangeMainNarrativeBeat()
    {
        manager.ChangeNarrativeBeat(nextNarrativeBeat);
    }

    public void UnWaitAndEnable()
    {
        enabled = true;
        mustWait = false;
        
    }

    public void InvokeDelayedEvent()
    {
        StartCoroutine(DelayEvent());
    }
}

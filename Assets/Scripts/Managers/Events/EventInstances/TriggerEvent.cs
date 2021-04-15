using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TriggerEvent : NarrativeEvent
{
    [SerializeField][TagSelector][Tooltip("The tag that will be used to compare against. Only objects with this tag will activate the collision event.")]
    private string comparisonTag;
    [SerializeField] private UnityEvent OnCollissionWithTag;
    [SerializeField] private bool RunOnlyOnce;
    private bool HasRun = false;

    private void OnTriggerEnter(Collider other)
    {
        if (mustWait || !enabled || (HasRun && RunOnlyOnce))
        {
            return;
        }
        if (other.gameObject.CompareTag(comparisonTag))
        {
            OnCollissionWithTag.Invoke();
            HasRun = true;
        }
    }
}

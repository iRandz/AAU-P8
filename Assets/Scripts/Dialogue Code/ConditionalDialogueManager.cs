using System;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalDialogueManager : MonoBehaviour
{
    
    #region SINGLETON PATTERN
    public static ConditionalDialogueManager _instance;
    public static ConditionalDialogueManager Instance
    {
        get {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<ConditionalDialogueManager>();
             
                if (_instance == null)
                {
                    GameObject container = new GameObject("ConditionalDialogueManager");
                    _instance = container.AddComponent<ConditionalDialogueManager>();
                }
            }
     
            return _instance;
        }
    }
    #endregion
    
    [SerializeField] private ConditionalDialogue[] _conditionalDialogueElements;
    [HideInInspector] public List<DialogueConditions> conditions = new List<DialogueConditions>();

    public enum ConditionNames
    {
        TestCondition1,
        TestCondition2,
        PaidAttentionToSummer,
        PaidAttentionToWinter,
        OnlyPaidAttentionToTV,
        IgnoredAllInArea2AndTV,
        OpenTheDoorsArea2,
        InArea3,
        FirstPlayed,
        HalfWayPlayed
    }

    public void Start()
    {
        foreach (ConditionalDialogue element in _conditionalDialogueElements)
        {
            foreach (DialogueConditions condition in element.dialogueConditionsArray)
            {
                conditions.Add(condition);
            }
        }
    }

    [ContextMenu("Debug Play Conditional Dialogue")]
    public void PlayDialogueFromConditions()
    {
        foreach (ConditionalDialogue element in _conditionalDialogueElements)
        {
            if (!element.HasBeenTriggered)
            {
                element.PlayDialogueIfConditionsAreMet();
            }
        }
    }

    public void ConditionMet(string name)
    {
        foreach (DialogueConditions condition in conditions)
        {
            if (name == condition.GetNameAsString())
            {
                condition.ConditionMet();
                PlayDialogueFromConditions();
            }
        }
    } 
    
}

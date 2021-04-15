using UnityEngine;
using UnityEngine.Events;

public class DialogueEventTrigger : NarrativeEvent
{
    
    [SerializeField] private DialogueElement dialogueElement;

    [SerializeField] private UnityEvent dialogueEvents;

    private bool delayEventRun;

    private bool eventRun;

    private void Awake()
    {
        dialogueElement.SetStarted(false);
        dialogueElement.SetEnded(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueElement.GetStarted() && dialogueElement.triggerEventWithDelayFromStart && !delayEventRun)
        {
            InvokeDialogueEventWithDelay(dialogueElement.triggerDelay);
            delayEventRun = true;
        }

        if (dialogueElement.GetEnded() && dialogueElement.triggerEventAtEnd && !eventRun)
        {
            dialogueEvents.Invoke();
            Debug.Log("END");
            eventRun = true;
        }
    }
    
    [ContextMenu("TriggerDialogueWithEvent")]
    private void TriggerDialogueWithEvent ()
    {
        DialogueManager.Instance.PlayDialogue(dialogueElement);
    }

    private void InvokeDialogueEventWithDelay(float delay)
    {
        Invoke("InvokeDialogueEvent", dialogueElement.triggerDelay);
    }

    private void InvokeDialogueEvent()
    {
        if (dialogueElement.triggerEventAtEnd || dialogueElement.triggerEventWithDelayFromStart)
        {
            dialogueEvents.Invoke();
            Debug.Log("DELAY");
        }
    }
    
    public void TriggerDialogueWoWithEvent()
    {
        DialogueManager.Instance.PlayDialogue(dialogueElement);
    }
}

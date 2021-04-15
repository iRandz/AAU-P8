using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

[System.Serializable]
[CreateAssetMenu(fileName = "New Dialogue Element", menuName = "Dialogue Element")]
public class DialogueElement : ScriptableObject
{
    public bool important = false;
    public bool triggerEventAtEnd = false;
    [FormerlySerializedAs("triggerEventWithDelay")] public bool triggerEventWithDelayFromStart = false;
    public float triggerDelay;
    //public UnityEvent dialogueElementEvent;

    private bool _started = false;
    private bool _ended = false;
    
    private bool _canBeInterrupted = true;
    private bool _canInterrupt = false;

    [FormerlySerializedAs("voFiles")] public Sound voFile;
    
    [TextArea(3,10)]
    public string subtitleText;

    public bool CanInterrupt => _canInterrupt;

    public bool CanBeInterrupted => _canBeInterrupted;

    public void SetImportance()
    {
        if (important)
        {
            _canBeInterrupted = false;
            _canInterrupt = true;
        }

        if (!important)
        {
            _canBeInterrupted = true;
            _canInterrupt = false;
        }
    }

    public bool GetStarted()
    {
        return _started;
    }
    
    public bool GetEnded()
    {
        return _ended;
    }

    public void SetStarted(bool started)
    {
        _started = started;
    }
    
    public void SetEnded(bool ended)
    {
        _ended = ended;
    }
    
    /*public void InvokeDialogueEvent()
    {
        if (triggerEventAtEnd || triggerEventWithDelayFromStart)
        {
            dialogueElementEvent.Invoke();
        }
    }*/
}

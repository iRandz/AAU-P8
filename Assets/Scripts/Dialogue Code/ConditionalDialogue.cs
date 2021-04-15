using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class ConditionalDialogue
{
    public List<DialogueConditions> dialogueConditionsArray = new List<DialogueConditions>();
    private bool _hasBeenTriggered = false;
    public DialogueElement dialogueElement;

    public bool AreAllConditionsMet()
    {
        List<bool> states = new List<bool>();
        foreach (DialogueConditions state in dialogueConditionsArray)
        {
            states.Add(state.state);
        }

        if (!states.Contains(false))
        {
            return true;
        }
        return false;
    }

    public void PlayDialogueIfConditionsAreMet()
    {
        if (AreAllConditionsMet())
        {
            DialogueManager.Instance.PlayDialogue(dialogueElement);
            HasBeenTriggered = true;
        }
    }

    public bool HasBeenTriggered
    {
        get => _hasBeenTriggered;
        set => _hasBeenTriggered = value;
    }
}

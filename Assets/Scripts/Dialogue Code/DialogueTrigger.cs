using UnityEngine;
using UnityEngine.Serialization;

public class DialogueTrigger : MonoBehaviour
{
    [FormerlySerializedAs("dialogueElement")] [SerializeField] private DialogueElement[] dialogueElements;
    
    [ContextMenu("TriggerDialogue")]
    public void TriggerDialogue ()
    {
        DialogueManager.Instance.StartDialogueQueue(dialogueElements);
    }
}

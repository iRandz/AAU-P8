using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleObjectAttentionScript : MonoBehaviour
{

    [System.Serializable] private class DialogueElementContainer
    {
        public bool played = false;
        public float timeCondition;
        public DialogueElement dialogueElement;
    }

    [SerializeField][Tooltip("The dialogue elements activated by looking at this object. " +
                             "Includes the timers for when each element is activated.")]
    private List<DialogueElementContainer> dialogueElements = new List<DialogueElementContainer>();
    
    [SerializeField] private float minLookTime = 2;
    private GazeAwareObject _gazeAwareObject;
    
    // Start is called before the first frame update
    void Start()
    {
        _gazeAwareObject = gameObject.GetComponent<GazeAwareObject>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (DialogueElementContainer element in dialogueElements)
        {
            if (!element.played && _gazeAwareObject.RetrieveCumulativeTimer() > minLookTime  && element.timeCondition < _gazeAwareObject.RetrieveCumulativeTimer())
            {
                DialogueManager.Instance.PlayDialogue(element.dialogueElement);
                element.played = true;
            }
        }
    }
}

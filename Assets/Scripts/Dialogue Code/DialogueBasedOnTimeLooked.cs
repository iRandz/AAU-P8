using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GazeAwareObject))]
public class DialogueBasedOnTimeLooked : NarrativeEvent
{
    [Header("Debug")]
    [SerializeField] private float debugCumulativeTime;

    [Header("Settings")]
    [SerializeField] private DialogueElement standardDialogueElement;
    [SerializeField] private List<float> timeCondition = new List<float>();
    [SerializeField] private List<DialogueElement> dialogueElementsToPlayAfterLookedAt = new List<DialogueElement>();
    [SerializeField] private float minLookTime = 2;
    private GazeAwareObject _gazeAwareObject;
    private bool _hasPlayed = false;

    // Start is called before the first frame update
    void Awake()
    {
        _gazeAwareObject = gameObject.GetComponent<GazeAwareObject>();
    }

    private void Update()
    {
        if (debugMode)
        {
            debugCumulativeTime = _gazeAwareObject.RetrieveCumulativeTimer();
        }
    }

    public void CheckLookTime()
    {
        if(_hasPlayed) return;
        foreach (float timeCondition in timeCondition)
        {
            if (!_hasPlayed && _gazeAwareObject.RetrieveCumulativeTimer() > minLookTime && this.timeCondition.IndexOf(timeCondition) == this.timeCondition.Count-1)
            {
                DialogueManager.Instance.PlayDialogue(dialogueElementsToPlayAfterLookedAt[this.timeCondition.IndexOf(timeCondition)]);
                DialogueManager.Instance.PlayDialogue(standardDialogueElement);
                _hasPlayed = true;
            }
            else if (_gazeAwareObject.RetrieveCumulativeTimer() > minLookTime && timeCondition < _gazeAwareObject.RetrieveCumulativeTimer() && _gazeAwareObject.RetrieveCumulativeTimer() < this.timeCondition[this.timeCondition.IndexOf(timeCondition)+1])
            {
                DialogueManager.Instance.PlayDialogue(dialogueElementsToPlayAfterLookedAt[this.timeCondition.IndexOf(timeCondition)]);
                DialogueManager.Instance.PlayDialogue(standardDialogueElement);
                _hasPlayed = true;
            }

        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(GazeAwareObject))]
public class DialogueBasedOnTimeLooked : NarrativeEvent
{
    [Header("Debug")]
    [SerializeField] private float debugCumulativeTime;

    [Header("Settings")]
    [SerializeField] private DialogueElement standardDialogueElement;
    [SerializeField] private List<float> timeCondition = new List<float>();
    [SerializeField] private List<DialogueElement> dialogueElementsToPlayAfterLookedAt = new List<DialogueElement>();
    [FormerlySerializedAs("minLookTime")] [SerializeField] private float minLookTimeForDialogue = 2;
    [SerializeField] private float midasTimer = 0.5f;
    private bool _lookedAt;
    private GazeAwareObject _gazeAwareObject;
    private bool _hasPlayed = false;
    private float _timer;

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
        if (_hasPlayed) return;
        if (!_lookedAt)
        {
            _timer += Time.deltaTime;
            if (_timer > midasTimer)
            {
                CheckLookTime();
                _timer = 0;
            }
        }
        else
        {
            _timer = 0;
        }
    }

    public void CheckLookTime()
    {
        if(_hasPlayed) return;
        foreach (float timeCondition in timeCondition)
        {
            if (!_hasPlayed && _gazeAwareObject.RetrieveCumulativeTimer() > minLookTimeForDialogue && this.timeCondition.IndexOf(timeCondition) == this.timeCondition.Count-1)
            {
                DialogueManager.Instance.PlayDialogue(dialogueElementsToPlayAfterLookedAt[this.timeCondition.IndexOf(timeCondition)]);
                DialogueManager.Instance.PlayDialogue(standardDialogueElement);
                _hasPlayed = true;
            }
            else if (_gazeAwareObject.RetrieveCumulativeTimer() > minLookTimeForDialogue && timeCondition < _gazeAwareObject.RetrieveCumulativeTimer() && _gazeAwareObject.RetrieveCumulativeTimer() < this.timeCondition[this.timeCondition.IndexOf(timeCondition)+1])
            {
                DialogueManager.Instance.PlayDialogue(dialogueElementsToPlayAfterLookedAt[this.timeCondition.IndexOf(timeCondition)]);
                DialogueManager.Instance.PlayDialogue(standardDialogueElement);
                _hasPlayed = true;
            }
        }
    }


    public bool LookedAt
    {
        get => _lookedAt;
        set => _lookedAt = value;
    }
}

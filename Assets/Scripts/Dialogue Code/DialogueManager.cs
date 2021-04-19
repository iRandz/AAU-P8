using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Subtitles))]
public class DialogueManager : MonoBehaviour
{
    #region SINGLETON PATTERN
    public static DialogueManager _instance;
    public static DialogueManager Instance
    {
        get {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<DialogueManager>();
             
                if (_instance == null)
                {
                    GameObject container = new GameObject("DialogueManager");
                    _instance = container.AddComponent<DialogueManager>();
                }
            }
     
            return _instance;
        }
    }
    #endregion
    
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private Subtitles subtitles;
    [SerializeField] private bool subtitlesActive = false;
    [FormerlySerializedAs("timeBetweenQueuedVO")] [SerializeField] private float timeBetweenQueuedVo = 1f;

    private Queue<DialogueElement> _dialogueElementsQueue;
    public List<DialogueElement> _dialogueElementsList;

    private DialogueElement _currentDialogueElement;
    private bool _queueIsRunning;

    private Coroutine _currentCoroutine = null;

    // Start is called before the first frame update
    void Start()
    {
        _dialogueElementsQueue = new Queue<DialogueElement>();
        _dialogueElementsList = new List<DialogueElement>();
        subtitles = GetComponent<Subtitles>();
    }

    public void StartDialogueQueue(DialogueElement[] dialogue)
    {
        foreach (DialogueElement dialogueElement in dialogue)
        {
            if (dialogueElement == null) continue;
            audioManager.RegisterSound(dialogueElement.voFile);
            PlayDialogue(dialogueElement);
        }
    }
    [ContextMenu("Next Dialogue")]
    public void NextDialogueInQueue()
    {
        DialogueElement dialogueElement = _dialogueElementsQueue.Dequeue();
        PlayDialogue(dialogueElement);
    }

    public void EndDialogueQueue()
    {
        _dialogueElementsQueue.Clear();
    }

    public void PlayDialogue(DialogueElement dialogueElement)
    {
        dialogueElement.SetImportance();
        if (_currentDialogueElement == null)
        {
            //Debug.Log("FIRST DIALOGUE " + dialogueElement + " is playing");
            QueueDialogueElement(dialogueElement);
        }
        else if(_currentDialogueElement.CanBeInterrupted == false && dialogueElement.important)
        {
            //Debug.Log("Important sound is already playing, new important sound: " + dialogueElement + " has been queued");
            QueueDialogueElementTopAndRemoveNonImportant(dialogueElement);
        }
        else if (_currentDialogueElement.CanBeInterrupted == false && dialogueElement.important == false)
        {
            //Debug.Log("Important sound is playing, non important VO " + dialogueElement + " has been queued");
            QueueDialogueElement(dialogueElement);
        }
        else if (_currentDialogueElement.CanBeInterrupted)
        {
            if (dialogueElement.CanInterrupt)
            {
                //Debug.Log("Important VO: " + dialogueElement + " has interrupted non important VO: " + _currentDialogueElement);
                audioManager.StopIfPlaying(_currentDialogueElement.voFile);
                StopCoroutine(_currentCoroutine);
                _queueIsRunning = false;
                QueueDialogueElementTopAndRemoveNonImportant(dialogueElement);
            }
            else if (dialogueElement.CanInterrupt == false)
            {
                //Debug.Log("Non important VO: " + dialogueElement + " has been queued after non important");
                QueueDialogueElement(dialogueElement);
            }
        }
    }

    public void QueueDialogueElement(DialogueElement dialogueElement)
    {
        if (dialogueElement == null)
        {
            //Debug.Log("Dialogue Element: " + dialogueElement + " was not found, and could not be queued");
            return;
        }

        //Debug.Log("Dialogue Element: " + dialogueElement + " was queued");
        _dialogueElementsList.Add(dialogueElement);
        if (_queueIsRunning == false)
        {
            _queueIsRunning = true;
            _currentCoroutine = StartCoroutine(IterateDialogueList());
        }
    }

    public void QueueDialogueElementTopAndRemoveNonImportant(DialogueElement dialogueElement)
    {
        RemoveAllNonImportantFromDialogueQueue();
        _dialogueElementsList.Add(dialogueElement);
        if (_queueIsRunning == false)
        {
            _queueIsRunning = true;
            _currentCoroutine = StartCoroutine(IterateDialogueList());
        }
    }

    public void RemoveAllNonImportantFromDialogueQueue()
    {
        List<DialogueElement> _dialogueElementsForDeletion = new List<DialogueElement>();
        foreach (DialogueElement dialogueElementInList in _dialogueElementsList)
        {
            if (!dialogueElementInList.important)
            {
                _dialogueElementsForDeletion.Add(dialogueElementInList);
            }
        }

        if (_dialogueElementsForDeletion.Count > 0)
        {
            foreach (DialogueElement dialogueElementForDeletion in _dialogueElementsForDeletion)
            {
                _dialogueElementsList.Remove(dialogueElementForDeletion);
            }
        }
    }

    private IEnumerator IterateDialogueList()
    {
        while (_dialogueElementsList.Count != 0)
        {
            DialogueElement dialogueElement = _dialogueElementsList[0];
            //Debug.Log("Now playing: " + dialogueElement.voFile.name+" from queue");
            _currentDialogueElement = dialogueElement;
            if (_currentDialogueElement.triggerEventWithDelayFromStart) _currentDialogueElement.SetStarted(true);
            audioManager.PlaySound(dialogueElement.voFile);
            if(subtitlesActive) StartCoroutine(subtitles.ConvertAndDisplaySubtitles(dialogueElement.subtitleText));
            yield return new WaitForSeconds(dialogueElement.voFile.source.clip.length + timeBetweenQueuedVo);
            if(_currentDialogueElement.triggerEventAtEnd) _currentDialogueElement.SetEnded(true);
            if (_dialogueElementsList.Count > 0)
            {
                try
                {
                    _dialogueElementsList.RemoveAt(0);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
        _queueIsRunning = false;
        yield return null;
    }

    /*private void dialogueEventInvoker()
    {
        _currentDialogueElement.InvokeDialogueEvent();
    }*/
    
}

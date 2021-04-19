using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public abstract class NarrativeEvent : MonoBehaviour
{
    [SerializeField] protected bool debugMode;

    [SerializeField] protected bool mustWait;
    [SerializeField] private MainNarrativeBeats nextNarrativeBeat;
    private UberEventManager manager;

    [SerializeField] private float EventDelay;
    [SerializeField] private UnityEvent DelayedEvent;
    
    protected enum storyChoices
    {
        None,
        TV,
        TVObject1,
        TVObject2,
        Object1,
        Object2,
    }

    protected static storyChoices choiceState;

    protected static MainNarrativeBeats UberState;
    
    // Logging
    private class LoggingStoryContainer
    {
        public storyChoices StoryChoice;
        public float Timestamp;
        public MainNarrativeBeats NarrativeBeats;
        public LoggingStoryContainer()
        {
            NarrativeBeats = UberState;
            StoryChoice = choiceState;
            Timestamp = Time.time;
        }
    }

    private static List<LoggingStoryContainer> LoggingList = new List<LoggingStoryContainer>();
    
    // CSV
    private static string basePath;
    private static string path;
    private static string directory;
    private static string currentEntry;
    private const string fileName = "StoryChoices.csv";
    private const string sep = ";";
    private const string headers = "NarrativeBeat;StoryChoice;TimeStamp";
    
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<UberEventManager>();
        if (mustWait)
        {
            enabled = !mustWait;
        }

        // Logging
        basePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        path = basePath + "/LoggingData";
        directory = path;
    }

    private void OnEnable()
    {
        if (mustWait)
        {
            enabled = !mustWait;
        }
    }

    private IEnumerator DelayEvent()
    {
        yield return new WaitForSeconds(EventDelay);
        DelayedEvent.Invoke();
    }

    protected void UpdateLogs()
    {
        LoggingList.Add(new LoggingStoryContainer());
    }
    protected static void SaveLogs()
    {
        GazeAwareObject.SaveLogs();
        if (!Directory.Exists (directory)) 
        {
            Directory.CreateDirectory (directory);
        }

        directory += "/";
        
        string fileLocation = directory + fileName;

        // Create new file / delete preexisting one.
        if (File.Exists(fileLocation))
        {
            File.Delete(fileLocation);
        }

        // Write headers
        using (StreamWriter writer = File.AppendText(fileLocation))
        {
            writer.WriteLine(headers);
        }

        // Write each entry to the file
        foreach (var gazeObject in LoggingList)
        {
            currentEntry = string.Concat(gazeObject.NarrativeBeats + sep + gazeObject.StoryChoice + sep + gazeObject.Timestamp);
            
            // Write line to file
            using StreamWriter writer = File.AppendText(fileLocation);
            writer.WriteLine(currentEntry);
        }
        
        print("done logging timestamps");
    }

    public void ChangeMainNarrativeBeat()
    {
        manager.ChangeNarrativeBeat(nextNarrativeBeat);
    }

    public void UnWaitAndEnable()
    {
        enabled = true;
        mustWait = false;
        
    }

    public void InvokeDelayedEvent()
    {
        StartCoroutine(DelayEvent());
    }
}

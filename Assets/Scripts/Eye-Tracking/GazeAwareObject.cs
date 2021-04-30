using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using Tobii.Gaming;

[System.Serializable] public class MyFloatFloatEvent : UnityEvent<float, float>
{
}

[RequireComponent(typeof(GazeAware))]
public class GazeAwareObject : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool debugMode;
    [SerializeField] private float debugCumulativeTime;

    [Header("Settings")]
    // Serialized fields
    [SerializeField] private float AwarenessDistance = 50;
    [SerializeField] private GameObject playerCamera;
    
    // Events
    [SerializeField] private UnityEvent lookedAtEvent;
    [SerializeField] private UnityEvent notLookedAtEvent;
    [SerializeField] private UnityEvent stillLookedAtEvent;
    [SerializeField] private UnityEvent lookedAtEnoughEvent;
    [SerializeField] private UnityEvent notLookedAtEnoughEvent;
        
    // Components
    private Transform playerTrans;
    private Camera playerCam;
    private GazeAware _gazeAware;
    private Collider _collider;
    
    // Booleans
    private bool hasFocus;
    private bool _stillLookedAt = false;
    
    // Logging
    private float _cumulativeTimer = 0;

    private class LoggingObjectContainer
    {
        public string Name;
        public float TotalGazeDuration;

        public LoggingObjectContainer(string nameIn, float durationIn)
        {
            Name = nameIn;
            TotalGazeDuration = durationIn;
        }
    }

    private static List<LoggingObjectContainer> LoggingList = new List<LoggingObjectContainer>();
    
    // CSV
    private static string basePath;
    private static string path;
    private static string directory;
    private static string currentEntry;
    private const string fileName = "GazeObjects.csv";
    private const string sep = ";";
    private const string headers = "Names;GazeDuration";

    // Start is called before the first frame update
    void Start()
    {
        if (playerCamera == null)
        {
            Debug.LogError("GazeAwareObject needs the main camera's Transform to calculate distance.");
            this.enabled = false;
        }

        playerTrans = playerCamera.GetComponent<Transform>();
        playerCam = playerCamera.GetComponent<Camera>();
        _collider = GetComponent<Collider>();
        _gazeAware = gameObject.GetComponent<GazeAware>();
        
        // Logging
        basePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        path = basePath + "/LoggingData";
        directory = path;
        LoggingList.Add(new LoggingObjectContainer(this.name, 0));
    }

    // Update is called once per frame
    void Update()
    {

        if (TobiiAPI.IsConnected)
        {
            hasFocus = _gazeAware.HasGazeFocus;
        }
        else
        {
            RaycastHit hit = PlayerGaze.FindPlayerGaze(playerCam, false);
            hasFocus = hit.collider == _collider;
        }


        var distBool = false;
        // This object is gazed upon
        if (hasFocus)
        {
            distBool = Vector3.Distance(transform.position, playerTrans.position) < AwarenessDistance;
            if (distBool)
            {
                _cumulativeTimer += Time.deltaTime;
                stillLookedAtEvent.Invoke();
            }
        }
        
        // This object has just started being gazed upon
        if (hasFocus && !_stillLookedAt)
        {
            if (distBool)
            {
                _stillLookedAt = true;
                lookedAtEvent.Invoke();
            }
        }

        // This object is no longer being gazed upon
        if (_stillLookedAt && !hasFocus)
        {
            _stillLookedAt = false;
            UpdateLoggingValues();
            notLookedAtEvent.Invoke();
        }

        if (debugMode)
        {
            debugCumulativeTime = RetrieveCumulativeTimer();
        }
    }

    // Private functions
    private void UpdateLoggingValues()
    {
        foreach (var gazeObject in LoggingList)
        {
            if (gazeObject.Name == this.name)
            {
                gazeObject.TotalGazeDuration = _cumulativeTimer;
            }
        }
    }
    
    private IEnumerator CheckLookedAtDuration(float durationIn, float intervalIn)
    {
        float startTime = Time.time;
        bool failed = true;
        float gazedAtDuration = 0;
        while (intervalIn + startTime > Time.time)
        {
            if (_stillLookedAt)
            {
                gazedAtDuration += Time.deltaTime;
            }

            if (gazedAtDuration > durationIn)
            {
                lookedAtEnoughEvent.Invoke();
                failed = false;
                break;
            }
            yield return null;
        }

        if (failed)
        {
            notLookedAtEnoughEvent.Invoke();
        }
    }
    
    // Public functions
    public void GazedAtForDurationWithinInterval(float durationIn, float intervalIn)
    {
        StartCoroutine(CheckLookedAtDuration(durationIn, intervalIn));
    }

    public float RetrieveCumulativeTimer()
    {
        return _cumulativeTimer;
    }
    
    public static void SaveLogs()
    {
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
            currentEntry = string.Concat(gazeObject.Name + sep + gazeObject.TotalGazeDuration);
            
            // Write line to file
            using StreamWriter writer = File.AppendText(fileLocation);
            writer.WriteLine(currentEntry);
        }
        
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, AwarenessDistance);
    }
    
}

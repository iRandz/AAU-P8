using System.Collections;
using System.Collections.Generic;
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
    
    // Other
    private float _cumulativeTimer = 0;
    
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
            notLookedAtEvent.Invoke();
        }

        if (debugMode)
        {
            debugCumulativeTime = RetrieveCumulativeTimer();
        }
    }

    // Private functions
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
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, AwarenessDistance);
    }
    
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SubEventManager : MonoBehaviour
{
    [SerializeField] private bool startAsActivated = false;
    public List<NarrativeEvent> narrativeEvents = new List<NarrativeEvent>();

    public UnityEvent SelectedEvent;
    private bool selectedEventRun;
    public UnityEvent DeselectedEvent;
    private bool deSelectedEventRun;

    private bool selected = false;
    // Start is called before the first frame update
    void Start()
    {

        selected = startAsActivated;
        foreach (var anEvent in narrativeEvents)
        {
            anEvent.enabled = startAsActivated;
        }
        
    }

    // Private functions
    private void WakeEvents()
    {
        foreach (var anEvent in narrativeEvents)
        {
            anEvent.enabled = true;
        }
    }

    private void HibernateEvents()
    {
        foreach (var anEvent in narrativeEvents)
        {
            anEvent.enabled = false;
        }
    }
    
    // Public functions
    public void OnSelected()
    {
        if (!selected)
        {
            if (!selectedEventRun)
            {
                SelectedEvent.Invoke();
                selectedEventRun = true;
            }
            WakeEvents();
            selected = !selected;
        }
    }

    public void OnDeselected()
    {
        if (selected)
        {
            if (!deSelectedEventRun)
            {
                DeselectedEvent.Invoke();
                deSelectedEventRun = true;
            }
            HibernateEvents();
            selected = !selected;
        }
    }
}

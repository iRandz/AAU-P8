using UnityEngine;
using UnityEngine.Events;

public class SetUpEvent : NarrativeEvent
{
    [SerializeField] private UnityEvent setUpEvent;
    private bool setupIsRun = false;
    // Start is called before the first frame update
    private void LateUpdate()
    {
        if (!setupIsRun)
        {
            setUpEvent.Invoke();
            setupIsRun = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            ChangeMainNarrativeBeat();
        }
    }
}

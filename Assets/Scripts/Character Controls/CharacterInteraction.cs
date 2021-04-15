using UnityEngine;

public class CharacterInteraction : MonoBehaviour
{
    #region SINGLETON PATTERN
    public static CharacterInteraction _instance;
    public static CharacterInteraction Instance
    {
        get {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<CharacterInteraction>();
             
                if (_instance == null)
                {
                    GameObject container = new GameObject("DialogueManager");
                    _instance = container.AddComponent<CharacterInteraction>();
                }
            }
     
            return _instance;
        }
    }
    #endregion

    private InteractableObject _focus;

    private Transform _mainCameraTransform;

    private InteractableObject _interactableObjectHit;

    [SerializeField] private GameObject interactionUI;

    private void Start()
    {
        if (Camera.main is { }) _mainCameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.SphereCast(_mainCameraTransform.position, 0.1f, _mainCameraTransform.forward, out hit, 100))
        {
            _interactableObjectHit = hit.collider.gameObject.GetComponent<InteractableObject>();

            if (_interactableObjectHit && _interactableObjectHit.HasInteracted) return;
            //Checks if the current focus object is interactable
            if (_interactableObjectHit && _interactableObjectHit != _focus)
            {
                SetFocus(_interactableObjectHit);
            }
            else if(!_interactableObjectHit && _focus)
            {
                RemoveFocus();
            }
        }
    }
    
    private void SetFocus(InteractableObject newFocus)
    {
        //This does not need to run if the object is the same
        if (newFocus != _focus)
        {
            if (_focus)
            {
                _focus.OnDefocus();
            }

            _focus = newFocus; 
            _focus.OnFocus();
        }
    }

    private void RemoveFocus()
    {
        if(_focus)
            _focus.OnDefocus();
        _focus = null;
    }

    public GameObject InteractionUI => interactionUI;
}

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private float interactionRadius;
    [SerializeField] private string interactKey = "e";
    [SerializeField] private bool multipleInteractions;
    private GameObject _interactionUI;
    private CharacterInteraction _characterInteraction;
    private Transform _playerTransform;
    private bool _isFocus;
    private bool _hasInteracted = false;
    private bool _showingUI = false;
    
    
    [FormerlySerializedAs("OnInteraction")] public UnityEvent onInteraction;
    
    // Start is called before the first frame update
    void Start()
    {
        _characterInteraction = CharacterInteraction.Instance;
        _playerTransform = _characterInteraction.gameObject.transform;
        _interactionUI = _characterInteraction.InteractionUI;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isFocus && !_hasInteracted)
        {
            InteractWithObject();
        }

        if (!_isFocus && _showingUI)
        {
            _interactionUI.SetActive(false);
            _showingUI = false;
        }
    }

    private void OnDisable()
    {
        if (enabled)
        {
            _interactionUI.SetActive(false);
        }
    }

    public void OnFocus()
    {
        if(!_hasInteracted) _isFocus = true;
    }

    public void OnDefocus()
    {
        _isFocus = false;
    }

    private void InteractWithObject()
    {
        float distance = Vector3.Distance(_playerTransform.position, this.transform.position);
        if (distance <= interactionRadius)
        {
            if (!_hasInteracted)
            {
                _interactionUI.SetActive(true);
                _showingUI = true;
            }
            if (Input.GetKeyDown(interactKey) && !_hasInteracted)
            {
                onInteraction.Invoke();
                if (!multipleInteractions)
                {
                    _hasInteracted = true;
                    _interactionUI.SetActive(false);
                }
            }
        }
        if (distance >= interactionRadius)
        {
            _interactionUI.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(this.transform.position, interactionRadius);
    }

    public bool HasInteracted => _hasInteracted;
}

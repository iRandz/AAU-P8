using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LockObject : MonoBehaviour
{
    [SerializeField] private KeyObject key;
    private bool _locked = true;
    public UnityEvent _OnUnlock;

    public void unlock()
    {
        if (key.InHand)
        {
            key.Use();
            _locked = false;
            _OnUnlock.Invoke();
        }
    }
}

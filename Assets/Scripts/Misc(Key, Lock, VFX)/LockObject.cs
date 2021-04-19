using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LockObject : MonoBehaviour
{
    [SerializeField] private KeyObject key;
    public UnityEvent _OnUnlock;

    public void unlock()
    {
        if (key.InHand)
        {
            key.Use();
            _OnUnlock.Invoke();
        }
    }
}

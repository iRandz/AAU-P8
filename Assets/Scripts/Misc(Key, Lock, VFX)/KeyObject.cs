using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyObject : MonoBehaviour
{
    [SerializeField] private GameObject inHandVersion;
    [SerializeField] private GameObject afterUnlockVersion;
    private bool inHand = false;
    
    private Renderer _renderer;
    private Collider _collider;

    private void Start()
    {
        _renderer = gameObject.GetComponent<Renderer>();
        _collider = gameObject.GetComponent<Collider>();
        inHandVersion.SetActive(false);
        afterUnlockVersion.SetActive(false);
    }

    public void PickUp()
    {
        _renderer.enabled = false;
        _collider.enabled = false;
        inHandVersion.SetActive(true);
        inHand = true;
    }

    public void Use()
    {
        if (inHand)
        {
            inHandVersion.SetActive(false);
            afterUnlockVersion.SetActive(true);
            inHand = false;
        }
    }

    public void Used()
    {
        inHandVersion.SetActive(false);
        gameObject.SetActive(false);
    }

    public bool InHand => inHand;
        
}

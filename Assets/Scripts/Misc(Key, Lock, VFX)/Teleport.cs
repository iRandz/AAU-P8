using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Teleport : NarrativeEvent
{
    private GameObject Player;
    private Transform PlayerTr;
    private CharacterController PlayerCC;
    private Transform Destination;
    
    
    [SerializeField] private Transform Object1Destination;
    [SerializeField] private Transform Object2Destination;
    [SerializeField] private Transform SplitDestination;
    [SerializeField] private Transform FlatDestination;
    
    [SerializeField] private float TeleportDelay;

    [SerializeField] private UnityEvent PrepareTeleportEvent;
    [SerializeField] private UnityEvent TeleportEvent;

    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
        PlayerTr = Player.GetComponent<Transform>();
        PlayerCC = Player.GetComponent<CharacterController>();
    }

    private IEnumerator DelayedTeleport()
    {
        yield return new WaitForSeconds(TeleportDelay);
        TeleportEvent.Invoke();
        Player.SetActive(false);
        PlayerTr.position = Destination.position;
        PlayerTr.rotation = Destination.rotation;
        Player.SetActive(true);
        PlayerCC.enabled = true;
    }
    
    public void TeleportStart()
    {
        switch (choiceState)
        {
            case storyChoices.None:
                Destination = FlatDestination;
                break;
            case storyChoices.Object1:
            case storyChoices.TVObject1:
                Destination = Object1Destination;
                break;
            case storyChoices.Object2:
            case storyChoices.TVObject2:
                Destination = Object2Destination;
                break;
            case storyChoices.TV:
                Destination = SplitDestination;
                break;
        }
        
        PrepareTeleportEvent.Invoke();
        PlayerCC.enabled = false;
        StartCoroutine(DelayedTeleport());
    }
}

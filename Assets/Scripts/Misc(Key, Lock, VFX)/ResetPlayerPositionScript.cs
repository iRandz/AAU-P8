using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(Collider))]
public class ResetPlayerPositionScript : NarrativeEvent
{
    private GameObject player;
    private Transform playerTR;
    [SerializeField] [TagSelector] private String compareTag;
    
    private Transform Destination;

    [SerializeField] private Transform TutorialDestination;
    [SerializeField] private Transform Level1Destination;
    [SerializeField] private Transform Level2Destination;
    [SerializeField] private Transform Object1Destination;
    [SerializeField] private Transform Object2Destination;
    [SerializeField] private Transform SplitDestination;
    [SerializeField] private Transform FlatDestination;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerTR = player.GetComponent<Transform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(compareTag))
        {
            ResetPayerPosition();
        }
    }

    public void ResetPayerPosition() // Teleports the player based on where they are in the narrative
    {
        switch (UberState)
        {
            case MainNarrativeBeats.Setup:
            case MainNarrativeBeats.Tutorial:
                Destination = TutorialDestination;
                break;
            case MainNarrativeBeats.Level1:
                Destination = Level1Destination;
                break;
            case MainNarrativeBeats.Level2:
                Destination = Level2Destination;
                break;
            case MainNarrativeBeats.Level3:
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
                    default:
                        break;
                }
                break;
            default:
                break;
        }
        player.SetActive(false);
        playerTR.position = Destination.position;
        playerTR.rotation = Destination.rotation;
        player.SetActive(true);
    }
}

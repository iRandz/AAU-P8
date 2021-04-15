using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioSource : MonoBehaviour
{

    [SerializeField] private AudioSource audioSource;
    
    // Start is called before the first frame update
    void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
        //playAudio();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playAudio()
    {
        if (audioSource != null)
        {
            audioSource.Play();
            Debug.Log("Audiosource on " + this.gameObject.name + " has been played.");
        }
    }
}

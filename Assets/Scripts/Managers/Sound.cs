using UnityEngine;

[System.Serializable]
public class Sound {
    
    [Header("File")]
    public string name;
    public string tag;
    public AudioClip clip;
    //public Subtitles.Section voSection;

    [Header("Audio Settings")]
    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(0.1f, 3f)]
    public float pitch = 1f;
    public bool loop = false;
    public bool playOnAwake = false;

    [Header("Spatial Settings")]
    [Range(0f, 1f)]
    public float spatialBlend;
    public float spatialMinDist = 5f;
    public float spatialMaxDist = 25f;
    [Tooltip("Use this to select which object the AudioSource should be applied to, especially in case of spatial sounds.")]
    public GameObject alternativeGameObject; // Add to other gameobject than AudioManager

    [HideInInspector]
    public AudioSource source;
}

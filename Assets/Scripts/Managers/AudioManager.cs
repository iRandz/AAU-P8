using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    
    //public Subtitles _SubtitlesInstance;
    public Sound[] sounds;

    public static AudioManager instance;

    private List<Sound> voiceOverQueue = new List<Sound>();

    private bool _queueIsRunning = false;
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            RegisterSounds(sounds);
        }
    }
    
    public void PlaySound(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            //Debug.Log("No sound with name: " + s.name + " sound is now being registered using RegisterSound()");
            RegisterSound(s);
        }
        //Debug.Log("Playing Sound: " + s.name + ".");
        s.source.Play();
    }

    public void PlaySound(Sound s)
    {
        if (s.source == null)
        {
            //Debug.Log("No sound with name: " + s.name + " sound is now being registered using RegisterSound()");
            RegisterSound(s);
        }
        s.source.Play();
        //Debug.Log("Sound: " + s.name + " has been played");
    }

    public void StopIfPlaying(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            //Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }

        if (s.source.isPlaying)
        {
            s.source.Stop();
        }
    }
    
    public void StopIfPlaying(Sound s)
    {
        if (s == null)
        {
            //Debug.LogWarning("Sound: " + s.name + " not found!");
            return;
        }

        if (s.source.isPlaying)
        {
            //Debug.Log("Sound: " + s.name + " has been stopped");
            s.source.Stop();
        }
    }

    public void PlayVoiceOverInQueue(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            //Debug.LogWarning("Sound: " + s.name + " not found!");
            return;
        }
        //Debug.Log("Sound Queued: " + s.name + ".");
        voiceOverQueue.Add(s);
        if (_queueIsRunning == false)
        {
            StartCoroutine(IterateVOQueue());
            _queueIsRunning = true;
        }
    }
    public void PlayVoiceOverInQueue(Sound s)
    {
        if (s == null)
        {
            //Debug.LogWarning("Sound: " + s.name + " not found!");
            return;
        }
        //Debug.Log("Sound Queued: " + s.name + ".");
        voiceOverQueue.Add(s);
        if (_queueIsRunning == false)
        {
            _queueIsRunning = true;
            StartCoroutine(IterateVOQueue());
        }
    }

    public void PlayRandomFromTag(string soundTag)
    {
        Sound[] s = Array.FindAll(sounds, sound => sound.tag == soundTag);
        if (s.Length == 0)
        {
            //Debug.LogWarning("Sounds with tag: " + soundTag + " not found!");
            return;
        }
        s[UnityEngine.Random.Range(0, s.Length)].source.Play();
    }

    public void FadeSoundIn(string soundName, float fadeInDuration = 2)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            //Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }
        
        StartCoroutine(AnimateSoundFade(s, fadeInDuration, "In"));
        s.source.Play();
    }

    public void FadeSoundIn(string soundName)
    {
        float fadeInDuration = 2f;
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            //Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }

        StartCoroutine(AnimateSoundFade(s, fadeInDuration, "In"));
        s.source.Play();
    }

    public void FadeSoundOut(string soundName, float fadeOutDuration = 2)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            //Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }
        
        StartCoroutine(AnimateSoundFade(s, fadeOutDuration, "Out"));
    }

    public void FadeSoundOut(string soundName)
    {
        float fadeOutDuration = 0.5f;
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            //Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }

        StartCoroutine(AnimateSoundFade(s, fadeOutDuration, "Out"));
    }

    IEnumerator AnimateSoundFade(Sound s, float duration, string inOut)
    {
        float percent = 0;
        
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;
            if (inOut == "In")
            {
                s.source.volume = Mathf.Lerp(0, s.volume, percent);
                yield return null;

            } else if (inOut == "Out")
            {
                s.source.volume = Mathf.Lerp(s.volume, 0, percent);
                if (s.source.volume <= 0f)
                {
                    s.source.Stop();
                }
                yield return null;
            }
        }
    }

    IEnumerator IterateVOQueue()
    {
        while (voiceOverQueue.Count != 0)
        {
            voiceOverQueue[0].source.Play();
            yield return new WaitForSeconds(voiceOverQueue[0].source.clip.length + 1f);
            voiceOverQueue.RemoveAt(0);
        }

        _queueIsRunning = false;
        yield return null;
    }

    public bool CheckIfPlaying(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            //Debug.LogWarning("Sound: " + soundName + " not found!");
            return false;
        }
        return s.source.isPlaying;
    }

    public void RegisterSounds(Sound[] soundList)
    {
        foreach (Sound s in soundList)
        {
            if (s.alternativeGameObject == null)
            {
                s.source = gameObject.AddComponent<AudioSource>();
            }
            else
            {
                s.source = s.alternativeGameObject.AddComponent<AudioSource>();
            }
            s.source.clip = s.clip;
                
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
            s.source.spatialBlend = s.spatialBlend;
            s.source.minDistance = s.spatialMinDist;
            s.source.maxDistance = s.spatialMaxDist;

            s.source.rolloffMode = s.rollOffIndex switch
            {
                0 => AudioRolloffMode.Logarithmic,
                1 => AudioRolloffMode.Linear,
                2 => AudioRolloffMode.Custom,
                _ => AudioRolloffMode.Custom,
            };
        }
    }
    public void RegisterSound(Sound s){
        if (s.alternativeGameObject == null)
        {
            s.source = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            s.source = s.alternativeGameObject.AddComponent<AudioSource>();
        }
        s.source.clip = s.clip;
                
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;
        s.source.playOnAwake = s.playOnAwake;
        s.source.spatialBlend = s.spatialBlend;
        s.source.minDistance = s.spatialMinDist;
        s.source.maxDistance = s.spatialMaxDist;

        s.source.rolloffMode = s.rollOffIndex switch
        {
            0 => AudioRolloffMode.Logarithmic,
            1 => AudioRolloffMode.Linear,
            2 => AudioRolloffMode.Custom,
            _ => AudioRolloffMode.Custom,
        };
    }
}

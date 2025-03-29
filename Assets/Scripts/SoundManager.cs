using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Audio;
using System;
using Random = UnityEngine.Random;

[System.Serializable]
public class Sound
{
    public AudioClip[] clip;
    public string name;
    [Range(0f, 1f)]
    public float volume;
    public bool loop;
    public bool isPaused = false;

    public AudioSource source;
}

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    public Sound[] sounds;
    public float delay;

    private Sound s;
    private float lastPlayTime = 0f;

    //public AudioClip ButtonPressed;
    // Start is called before the first frame update
    /*
    public AudioClip[] walkingAudioClips;

    GameObject soundGameObject;
    AudioClip currentClip;
    AudioSource currentAudioSource;

    public void PlaySound(AudioClip audioClip, float volume)
    {
        currentClip = audioClip;
        currentAudioSource = soundGameObject.AddComponent<AudioSource>();
        currentAudioSource.PlayOneShot(audioClip);
        SetVolume(volume);
    }

    public void PlayWalkSound()
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();

        currentClip = walkingAudioClips[Random.Range(0, walkingAudioClips.Length - 1)];
        audioSource.clip = currentClip;
        audioSource.Play();
    }

    public void SetVolume(float volume)
    {
        currentAudioSource.volume = volume;
    }

    //why?
    public void StopSound()
    {
        //GameObject soundGameObject = new GameObject("Sound");
        //AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        currentAudioSource = null;
        //audioSource.Stop();
    }
    */
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip[0];

            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name, float delay)
    {
        if (Time.time - lastPlayTime < delay)
        {
            return;
        }

        s = Array.Find(sounds, sound => sound.name == name);
        if (s.clip.Length > 1)
        {
            AudioClip clip = s.clip[Random.Range(0, s.clip.Length)];
            s.source.clip = clip;
        }
        s.source.Play();
        lastPlayTime = Time.time;
    }

    public void PlayOnClick(string name)
    {
        s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public void Stop(string name)
    {
        s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }
    
    public void PauseAll()
    {
        foreach(Sound j in sounds)
        {
            if(j.source.isPlaying && j.name != "Background")
            {
                j.source.Pause();
                j.isPaused = true;
            }
        }
    }

    public void ResumeAll()
    {
        foreach (Sound j in sounds)
        {
            if (j.isPaused)
            {
                j.source.Play();
                j.isPaused = false;
            }
        }
    }

    public void SetVolume(string name, float volume)
    {
        s = Array.Find(sounds, sound => sound.name == name);
        s.source.volume = volume;
    }

    void Start()
    {
        Play("Background", 0f);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        //if pauseMenuUI, pause voices as well
        if (currentAudioSource != null && Time.timeScale == 0)
        {
            currentAudioSource.Stop();
        }
        else if (currentAudioSource != null && !currentAudioSource.isPlaying && Time.timeScale == 1)
        {
            Debug.Log("SOUND HAS TO BE PLAYED AGAIN");
            currentAudioSource.PlayOneShot(currentClip);
        }
        */
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // store a static reference to the single instance of SoundManager
    public static SoundManager Instance = null;
    // refers to audio source added to the soundManager for effects
    private AudioSource soundEffectAudio;

    public AudioClip gunFire;
    public AudioClip upgradedGunFire;
    public AudioClip hurt;
    public AudioClip alienDeath;
    public AudioClip marineDeath;
    public AudioClip victory;
    public AudioClip elevatorArrived;
    public AudioClip powerUpPickup;
    public AudioClip powerUpAppear;

    // Start is called before the first frame update
    void Start()
    {
        // if this is the first SoundManager created, it sets static Instance variable to the current obj.
        if (Instance == null)
        {
            Instance = this;
        }
        // if a second SoundManager is created, it automatically destroys itself to ensure only one SoundManager exists
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        // returns all components of AudioSource type
        // checks for the audio source that doesn't have music set to it
        AudioSource[] sources = GetComponents<AudioSource>();
        foreach (AudioSource source in sources)
        {
            if (source.clip == null)
            {
                soundEffectAudio = source;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayOneShot(AudioClip clip)
    {
        soundEffectAudio.PlayOneShot(clip);
    }

}

using System.Collections;
using System.Collections.Generic;
using Prefabs.PowerUp.Scripts.Utility;
using UnityEngine;
using UnityEngine.Serialization;

public class PlaySoundEffect : Singleton<PlaySoundEffect>
{
    public AudioSource soundEffects;
    public AudioClip rootHitsRockAudioSound;
    public AudioClip spiderBreaksRootSound;
    public AudioClip nutrientPickUpSound;   
    public AudioClip victorySound;
    public AudioClip defeatSound;
    
    public float volume = 0.5f;

    public void PlayRootHitsRockSound()
    {
        soundEffects.PlayOneShot(rootHitsRockAudioSound, volume);
    }

    public void PlaySpiderBreaksRootSound()
    {
        soundEffects.PlayOneShot(spiderBreaksRootSound, volume);
    }
    
    public void PlayNutrientPickUpSound()
    {
        soundEffects.PlayOneShot(nutrientPickUpSound, volume);
    }
    
    public void PlayVictorySound()
    {
        soundEffects.PlayOneShot(victorySound, volume);
    }
    
    public void PlayDefeatSound()
    {
        soundEffects.PlayOneShot(defeatSound, volume);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

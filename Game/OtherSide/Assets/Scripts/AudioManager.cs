using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    void Awake()
    {
        // check if there is another audiomanager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Play theme music when the audio manager is loaded
        Play("Theme");
    }

    /**
     * to use the found first select and name clip in the audiomanager,
     * then call FindObjectOfType<AudioManager>().Play("name") to play the sound.
     */
    public void Play(string name, GameObject obj = null)
    {
        Debug.Log("Play sound:" + name);

        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + "not found!");
            return;
        }

        // if audio source is defined
        if (obj != null)
        {
            Debug.LogWarning("tie to" + obj); 
            obj.AddComponent<AudioSource>(); 

            s.source = obj.GetComponent<AudioSource>(); 

            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

             
            s.source.minDistance = 1; 
            s.source.maxDistance = 100; 
            s.source.spatialBlend = 1; 
            s.source.rolloffMode = AudioRolloffMode.Logarithmic; 
            s.source.spread = 180; 
        }
        
        s.source.Play();
    }
}

using UnityEngine.Audio;
using UnityEngine;

/**
 * The sound class
 */

[System.Serializable]
public class Sound {

    // name of the sound
    public string name;

    // audio clip
    public AudioClip clip; 

    [Range(0f, 1f)]
    public float volume; 
    [Range(.1f, 3f)]
    public float pitch; 

    public bool loop;

    // public float minDistance;
    // public float maxDistance;  

    [HideInInspector]
    public AudioSource source;
}

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
        if (instance == null) {
            instance = this; 
        } else {
            Destroy(gameObject); 
            return; 
        }
    
        foreach (Sound s in sounds) {
            s.source =  gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip; 
            
            s.source.volume = s.volume; 
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Play theme when the audio manager is loaded
        Play("Theme");
    }

    /**
     * to use the found first select and name clip in the audiomanager, 
     * then call FindObjectOfType<AudioManager>().Play("name") to play the sound.
     */ 
    public void Play(string name) {
        Debug.Log("Play sound:" + name); 
        Sound s = Array.Find(sounds, sound => sound.name == name); 
        if (s == null) {
            Debug.LogWarning("Sound:" + name + "not found!"); 
            return;
        }
        s.source.Play(); 
    }
}

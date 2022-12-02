using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class bossTrigger : MonoBehaviour
{
    public GameObject boss; // boss gameobject, for turning on boss
    public GameObject cameraScene; // player camera for zooming out
    public float zoomout = 8.0f; // how much to zoom out player camera
    public GameObject lightingScene; // make the lights turn on brighter
    public float lightIntensity; // how much intensity to turn up the lights
    public GameObject barricade; // barricade after player enters boss arena
    public GameObject chest; // treasure after defeating the boss
    public GameObject particlesInstantiate; // particles for when things get instantiated
    // public AudioClip bossmusic; // boss music

    private float playerOriginalZoom = 4.0f;
    private bool alreadyTriggered = false;
    private bool alreadyTriggered2 = false;
    Coroutine zoomTween;

    /* This script is to trigger the proper settings to fight the boss
     * Such as instantiating the boss when the player approaches and 
     * brigtening up the scene and maybe zooming out the camera.
     */
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !alreadyTriggered)
        {
            // Debug.LogWarning("Testing boss trigger");
            boss.SetActive(true);
            lightingScene.GetComponent<Light2D>().intensity = lightIntensity;
            barricade.SetActive(true);
            // cameraScene.GetComponent<Camera>().orthographicSize = zoomout; // instant zoom out
            ZoomTo(cameraScene.GetComponent<Camera>(), zoomout, 3.0f); // smooth zoomout (camera, targetZoom, duration)
            // TODO: add boss music and hp bar
            alreadyTriggered = true; // don't trigger this action more than once
            // Destroy(gameObject); // destroy this trigger
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && alreadyTriggered && !alreadyTriggered2 && boss.GetComponent<Enemy>().health <= 0)
        {
            // boss dies when it's reaches 0 hp
            // change boss to death animation
            boss.GetComponent<Animator>().SetBool("isDead", true);
            /*if (particlesInstantiate != null)
            {
                // smoke particle effect
                GameObject phit = Instantiate(particlesInstantiate, boss.transform);
                phit.GetComponent<ParticleSystem>().Play();
                Destroy(phit, phit.GetComponent<ParticleSystem>().main.duration);
            }*/

            // activate chest into scene
            chest.SetActive(true);
            if (particlesInstantiate != null)
            {
                // smoke particle effect
                GameObject phit = Instantiate(particlesInstantiate, chest.transform);
                phit.GetComponent<ParticleSystem>().Play();
                Destroy(phit, phit.GetComponent<ParticleSystem>().main.duration);
            }

            // reset camera to normal
            // ZoomTo(cameraScene.GetComponent<Camera>(), playerOriginalZoom, 4.0f); // smooth zoomout (camera, targetZoom, duration)

            alreadyTriggered2 = true; // don't do more than once
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player" && alreadyTriggered && alreadyTriggered2)
        {
            // reset camera when player leaves boss arena
            ZoomTo(cameraScene.GetComponent<Camera>(), playerOriginalZoom, 2.0f); // smooth zoomout (camera, targetZoom, duration)
        }

    }
    IEnumerator ZoomCoroutine(Camera camera, float targetSize, float duration)
    {
        // smoothly zooms out the camera using Math.Lerp, alternatively might want Math SmoothStep or Towards
        float startSize = camera.orthographicSize;

        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            float blend = t / duration;
            // A SmoothStep rather than a Lerp might look good here too.
            camera.orthographicSize = Mathf.Lerp(startSize, targetSize, blend);

            // Wait one frame, then resume.
            yield return null;
        }

        // Finish up our transition, and mark our work done.
        camera.orthographicSize = targetSize;
        zoomTween = null;
    }

    void ZoomTo(Camera camera, float targetSize, float duration)
    {
        // Starts zoom coroutine
        // Check if we're already in a zoom transition, and stop it to ensure we don't double-up.
        if (zoomTween != null)
            StopCoroutine(zoomTween);

        // Kick off a new zoom transition that can keep going on future frames.
        zoomTween = StartCoroutine(ZoomCoroutine(camera, targetSize, duration));
    }
}

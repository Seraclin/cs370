using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class telportbacktoboss : MonoBehaviour
    // teleports the player back into the boss room if they're on wrong side of barricade
{
    public GameObject particlesInstantiate; // particles for when things get instantiated

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
        if(collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().enabled = false;
            StartCoroutine(teleport(collision.gameObject));
        }
    }

    IEnumerator teleport(GameObject player)
    {
        // player.GetComponent<PlayerController>().enabled = false; // don't let player move during this time
        yield return new WaitForSeconds(0.15f); // wait for animation
        player.transform.position = GameObject.FindGameObjectWithTag("Teleport").transform.position; // move the player to Teleport position

        if (particlesInstantiate != null)
        {
            // smoke particle effect
            GameObject phit = Instantiate(particlesInstantiate, player.transform);
            phit.GetComponent<ParticleSystem>().Play();
            Destroy(phit, phit.GetComponent<ParticleSystem>().main.duration);
        }
        player.GetComponent<PlayerController>().enabled = true; // let player move now
    }
}

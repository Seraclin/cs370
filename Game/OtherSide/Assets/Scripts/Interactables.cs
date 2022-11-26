using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactables : MonoBehaviour
{
    [SerializeField] int healing;
    public PlayerHealth pHealth;
    public bool inventory; // check if item can be put into inventory
    public bool openable; // check if item is openable
    public bool locked; // check if item is locked
    public GameObject itemNeeded;//Item needed to interact

    //chest values
    public bool isChest;
    public GameObject itemInChest;
    private bool chestOpened;
    public Transform spawnPoint;
    private Interactables itemInChestScript;

    [SerializeField] Sprite openSprite; // chest or door open sprite
    [SerializeField] GameObject particleInteract; // particle when open sprite happens


    public void DoInteraction()
    {
        //pick up use/store.
        gameObject.SetActive(false);

        if (healing != 0)
        {
            pHealth.ChangeHealth(healing);
        }
    }

    public void openChest()
    {
        if (!chestOpened)
        {
            chestOpened = true;
            /*
            GameObject item = Instantiate(itemInChest, spawnPoint.position, spawnPoint.rotation) as GameObject;
            itemInChestScript = item.GetComponent<Interactables>();
            itemInChestScript.
            */
            itemInChest.SetActive(true);
            itemInChest.transform.position = spawnPoint.position;

            // change chest sprite to open chest sprite
            changeSprite();
        }
    }
    public void changeSprite()
    {
        // changes sprite to 'openSprite'
        // change door to unlocked door (door_unlocked)
        // change chest to open chest (chest_open)
        if(particleInteract != null) // particle effects
        {
            GameObject particle = Instantiate(particleInteract, gameObject.transform);
            particle.GetComponent<ParticleSystem>().Play();
            Destroy(particle, particle.GetComponent<ParticleSystem>().main.duration);
        }
        gameObject.GetComponent<SpriteRenderer>().sprite = openSprite;
    }
}
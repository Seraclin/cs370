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
    [SerializeField] Sprite chestOpenSprite; // chest open sprite


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
            gameObject.GetComponent<SpriteRenderer>().sprite = chestOpenSprite;
        }
    }
    
}
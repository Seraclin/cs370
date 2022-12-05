using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public GameObject currentInteractable = null; //Variable for if we implement an "Interact" key
    public Interactables currentInterObjScript = null;
    public Inventory inventory;

    [SerializeField]
    private bool triggerActive = false;

    [SerializeField]
    public KeyCode key;

    void Start() { }

    void Update()
    {
        if (triggerActive && Input.GetKeyDown(key))
        {
            if (currentInterObjScript.isChest)
            {
                currentInteractable.SendMessage("openChest");
                return;
            }

            //check if item goes into inventory
            if (currentInterObjScript.inventory)
            {
                inventory.addItem(currentInteractable);
                FindObjectOfType<AudioManager>().Play("pickupItem");
            }
            else if (currentInterObjScript.openable)
            {
                if (currentInterObjScript.locked) //is locked?
                {
                    //searching through inventory to find item
                    if (inventory.FindItem(currentInterObjScript.itemNeeded))
                    {
                        //door is unlocked
                        currentInterObjScript.locked = false;
                        currentInterObjScript.changeSprite(); // change to unlocked door sprite
                        Debug.Log("unlocked");
                        FindObjectOfType<AudioManager>().Play("openDoor");
                    }
                    else
                    {
                        Debug.Log("Still locked");
                    }
                }
                //NOT locked

                else
                {
                    Debug.Log("Opened");
                    currentInteractable.SendMessage("DoInteraction");
                    FindObjectOfType<AudioManager>().Play("openDoor");
                }
            }
            else
            {
                currentInteractable.SendMessage("DoInteraction"); //automatic interaction
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Inter") //function for if we use interact key
        {
            //Debug.Log(other.name); //testing
            triggerActive = true;
            currentInteractable = other.gameObject;
            currentInterObjScript = currentInteractable.GetComponent<Interactables>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Inter")
        {
            if (other.gameObject == currentInteractable.gameObject)
            {
                triggerActive = false;
                currentInteractable = null;
            }
        }
    }
}

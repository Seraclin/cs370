using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

    public GameObject currentInteractable = null; //Variable for if we implement an "Interact" key
    //public Interactables currentInterObjScript = null;
    //public Inventory inventory;

    void Start()
    {

    }

    void Update()
    {
        currentInteractable.SendMessage("DoInteraction"); //automatic interaction
    }

    void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.gameObject.tag == "Inter")//function for if we use interact key
        {
            //Debug.Log(other.name); //testing
            currentInteractable = other.gameObject;
            //currentInterObjScript = currentInteractable.GetComponent<Interactables>();
        }
        

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Inter")
        {
            if (other.gameObject == currentInteractable.gameObject)
            {
                currentInteractable = null;
            }
            
        }
    }
    
}

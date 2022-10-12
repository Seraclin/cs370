using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public GameObject[] inventory = new GameObject[5];//inventory size of 5, so far only keys?
    // Start is called before the first frame update

    public void addItem(GameObject item)
    {
        bool itemAdded = false;
        //first open inventory slot
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = item;
                //Debug.Log(item.name + " was added"); //testing
                itemAdded = true;
                item.SendMessage("DoInteraction"); //automatic interaction
                break;
            }
        }
        //if inventory is full

        if (!itemAdded)
        {
            //Debug.Log("inventory full, item not added");
        }
    }

    public bool FindItem(GameObject item)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == item)
            {
                return true; //found correct item
            }
        }//item not found = false
        return false;
    }
    
}

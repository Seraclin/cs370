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
    public GameObject itemNeeded; //Item needed to interact
    
    public void DoInteraction()
    {
        //pick up use/store.
        gameObject.SetActive(false);


        pHealth.ChangeHealth(healing);
    }
    
}
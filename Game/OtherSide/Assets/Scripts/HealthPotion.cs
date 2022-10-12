using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    [SerializeField] int healing;
    public PlayerHealth pHealth;

    public void DoInteraction()
    {
        //pick up use/store.
        gameObject.SetActive(false);


        pHealth.ChangeHealth(healing);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trap : MonoBehaviour
{
    [SerializeField] int damage; //Amount of damage done to player
    int xForce;
    int yForce;
    private bool trigger;
    PlayerHealth pHealth;

    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            pHealth = collision.gameObject.GetComponent<PlayerHealth>();
            trigger = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            trigger = false;
        }
    }

    void Update()
    {
        if (trigger)
        {
            Debug.Log("Trigger");
            pHealth.ChangeHealth(damage);
        }
    }
}

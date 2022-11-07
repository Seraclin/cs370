using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int damage; //this is the amount of damage done by the weapon
    [SerializeField] float duration;
    void FixedUpdate()
    {
        if (duration > 0)
        {
            duration -= Time.deltaTime;
        } else
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.tag != collision.gameObject.tag + "Ability")
        {
            

            if (collision.gameObject.tag == "Enemy")
            {
                
                Enemy eScript = collision.gameObject.GetComponent<Enemy>();
                eScript.ChangeHealth(damage);
                Destroy(gameObject);
               
            }
            else if (collision.gameObject.tag == "Wall")
            {
                Destroy(gameObject);
            }
            else if (collision.gameObject.tag == "Player")
            {

                PlayerHealth eScript = collision.gameObject.GetComponent<PlayerHealth>();
                eScript.ChangeHealth(0 - damage);
                Destroy(gameObject);
            }

        }


    }
}

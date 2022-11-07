using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int damage; //this is the amount of damage done by the weapon
    Rigidbody2D rb;
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.tag != collision.gameObject.tag + "Ability")
        {
            

            if (collision.gameObject.tag == "Enemy")
            {
                
                Enemy eScript = collision.gameObject.GetComponent<Enemy>();
                
                eScript.ChangeHealth(damage);
                Destroy(gameObject);
               
            }
            if (collision.gameObject.tag == "Wall")
            {
                Destroy(gameObject);
               
            }
        }
       
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    [SerializeField] int damage; //Amount of damage done to player
    int xForce;
    int yForce;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            
            //This code is probably bad
            if(this.gameObject.transform.position.x<collision.gameObject.transform.position.x)
            {
            xForce = 1;
            }
            else
            {
            xForce = -1;
            }
            if (this.gameObject.transform.position.y < collision.gameObject.transform.position.y)
            {
                yForce = 1;
            }
            else
            {
                yForce = -1;
            }
            PlayerHealth pHealth = collision.gameObject.GetComponent<PlayerHealth>();
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
            pHealth.ChangeHealth(damage);
            pc.CollisionForce(xForce, yForce);
        }
    }
}

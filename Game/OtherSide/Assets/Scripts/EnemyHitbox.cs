using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    [SerializeField] int damage; //Amount of damage done to player

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            PlayerHealth pHealth = collision.gameObject.GetComponent<PlayerHealth>();
            pHealth.ChangeHealth(-10);
        }
    }
}

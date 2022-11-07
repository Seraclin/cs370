using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Damage logic for the ability hitting enemy or player.
 * TODO: needs a lot of work, needs to inherit attributes from Ability object, have Ability owner
 */
public class AbilityDamage : MonoBehaviour
{
    [SerializeField] int damageA = -20; // TODO: replace with Ability object's damage
    [SerializeField] float durationA = 5; // duration of ability TODO: replace with Ability object's duration 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // On collision of the this abilities collider with enemy/player call their respective change health
        // TODO: check who is owner of attack, so they don't get damage from their own attack
        if(collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Ability hit an enemy! Its health changed by "+ damageA);
            EnemyHealth eHealth = collision.gameObject.GetComponent<EnemyHealth>();
            eHealth.ChangeHealth(damageA); // TODO: fix null ref here when enemies attack other enemies
            // collision.gameObject.GetComponent<EnemyHealth>.ChangeHealth(this.)
        }
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Ability hit the player! Your health changed by "+damageA);
            PlayerHealth pHealth = collision.gameObject.GetComponent<PlayerHealth>();
            pHealth.ChangeHealth(damageA); // TODO: pass in damage value from Ability object

        }

    }
}

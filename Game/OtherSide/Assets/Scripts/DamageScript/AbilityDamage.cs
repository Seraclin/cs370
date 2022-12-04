using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Damage logic for the ability hitting enemy or player.
 * TODO: needs a lot of work, needs to inherit attributes from Ability object, have Ability owner
 */
public class AbilityDamage : MonoBehaviour
{
    internal int damage; // TODO: replace with Ability object's damage
    private float dCoef;
    [SerializeField] GameObject particleTrail; // particle trail
    [SerializeField] GameObject particleImpact; // particle hit
    // [SerializeField] float duration = 5; // duration of ability TODO: replace with Ability object's duration 

    // Start is called before the first frame update
    void Start()
    {
        if (particleTrail != null) // display projectile trail
        {
            GameObject ptrail = Instantiate(particleTrail, gameObject.transform);
            ptrail.GetComponent<ParticleSystem>().Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.tag != collision.gameObject.tag + "Ability")
        {
            if (particleImpact != null && (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Player")) // play projectile impact particles, which self-destroy after playing
            {
                // public static Object Instantiate(Object original, Vector3 position, Quaternion rotation);
                Transform pos = gameObject.transform;
                // GameObject phit = Instantiate(particleImpact, collision.transform.position, Quaternion.identity); // impact at enemy center
                GameObject phit = Instantiate(particleImpact, gameObject.transform.position, Quaternion.identity); // impact at enemy collider
                // phit.GetComponent<ParticleSystem>().Play();
                // Destroy(phit, phit.GetComponent<ParticleSystem>().main.duration);
            }
            else if (particleImpact != null && (collision.gameObject.tag == "Wall")) // wall needs it's own case
            {
                Transform pos = gameObject.transform;
                GameObject phit = Instantiate(particleImpact, gameObject.transform.position, Quaternion.identity);
            }

            if (collision.gameObject.tag == "Enemy")
            {
                Enemy eScript = collision.gameObject.GetComponent<Enemy>();
                dCoef = eScript.damageCoef;
                if (!eScript.isDead && gameObject.transform.parent.gameObject.GetComponent<AbilityArray>().holderArray[1].ability.isPassive)
                {
                    gameObject.transform.parent.gameObject.GetComponent<AbilityArray>().holderArray[1].ability.Activate(gameObject.transform.parent.gameObject);
                }
                eScript.ChangeHealth(0 - Mathf.RoundToInt(damage * dCoef));
                Destroy(gameObject, 0.2f);

            }
            else if (collision.gameObject.tag == "Wall")
            {
                Destroy(gameObject);
            }
            else if (collision.gameObject.tag == "Player")
            {
                // Debug.Log("Player hit, health reduce by " + damage);
                PlayerHealth eScript = collision.gameObject.GetComponent<PlayerHealth>();
                dCoef = eScript.damageCoef;
                eScript.ChangeHealth(0 - Mathf.RoundToInt(damage * dCoef));
                
                if (gameObject.transform.parent.gameObject.GetComponent<AbilityArray>().holderArray[1].ability.isPassive)
                {
                    gameObject.transform.parent.gameObject.GetComponent<AbilityArray>().holderArray[1].ability.Activate(gameObject.transform.parent.gameObject);
                }
   
                Destroy(gameObject, 0.2f);
            }

        }


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DamageOverTime : MonoBehaviour
{
    internal int damage; //this is the amount of damage done by the weapon
    [SerializeField] float duration;

    [SerializeField] GameObject particleTrail; // particle trail
    [SerializeField] GameObject particleImpact; // particle hit

    private Enemy eScript1;
    private PlayerHealth eScript2;
    private bool tookdamage;
    private string tag;
    private Collider2D obj;

    void Start()
    {
        if (particleTrail != null) // display projectile trail
        {
            GameObject ptrail = Instantiate(particleTrail, gameObject.transform);
            ptrail.GetComponent<ParticleSystem>().Play();
        }
    }
    void FixedUpdate()
    {
        if (duration > 0)
        {
            duration -= Time.deltaTime;
        }
        else
        {
            if (particleImpact != null) // play projectile impact particles, which self-destroy after playing
            {
                GameObject phit = Instantiate(particleImpact, gameObject.transform);
                // phit.GetComponent<ParticleSystem>().Play();
                // Destroy(phit, phit.GetComponent<ParticleSystem>().main.duration);
            }
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.tag != collision.gameObject.tag + "Ability")
        {
            /* Doesn't make sense to have this for a poison ability
             * else if (particleImpact != null && (collision.gameObject.tag == "Wall")) // wall needs it's own case
            {
                Transform pos = gameObject.transform;
                GameObject phit = Instantiate(particleImpact, gameObject.transform.position, Quaternion.identity);
            }*/

            obj = collision;
            if (collision.gameObject.tag == "Enemy")
            {
                eScript1 = collision.gameObject.GetComponent<Enemy>();
                tag = collision.gameObject.tag;
                // Debug.Log(damage);
                InvokeRepeating("Damage", 0.1f, 0.5f);

            }
            /* Shouldn't destroy upon wall - Sam
            else if (collision.gameObject.tag == "Wall")
            {
                Destroy(gameObject);
            }*/
            else if (collision.gameObject.tag == "Player")
            {
                eScript2 = collision.gameObject.GetComponent<PlayerHealth>();
                tag = collision.gameObject.tag;
                InvokeRepeating("Damage", 0.1f, 0.5f);
            }

        }


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == tag)
        {
            CancelInvoke("Damage");
        }
    }

    private void Damage()
    {
        if (tag == "Enemy")
        {
            tookdamage = eScript1.ChangeHealth(0 - damage);
            if (!eScript1.isDead && gameObject.transform.parent.gameObject.GetComponent<AbilityArray>().holderArray[1].ability.isPassive)
            {
                gameObject.transform.parent.gameObject.GetComponent<AbilityArray>().holderArray[1].ability.Activate(obj.gameObject);
            }
            // Destroy(gameObject);
            if (particleImpact != null && tookdamage && (tag == "Enemy" || tag == "Player")) // play projectile impact particles, which self-destroy after playing
            {
                // public static Object Instantiate(Object original, Vector3 position, Quaternion rotation, Transform parent);
                Transform pos = gameObject.transform;
                GameObject phit = Instantiate(particleImpact, obj.transform.position, Quaternion.identity, obj.transform);
                // phit.GetComponent<ParticleSystem>().Play();
                // Destroy(phit, phit.GetComponent<ParticleSystem>().main.duration);
            }
        } else if (tag == "Player")
        {
            tookdamage = eScript2.ChangeHealth(0 - damage);
            if (gameObject.transform.parent.gameObject.GetComponent<AbilityArray>().holderArray[1].ability.isPassive)
            {
                gameObject.transform.parent.gameObject.GetComponent<AbilityArray>().holderArray[1].ability.Activate(obj.gameObject);
            }
            // Destroy(gameObject);
            if (particleImpact != null && tookdamage && (tag == "Enemy" || tag == "Player")) // play projectile impact particles, which self-destroy after playing
            {
                // public static Object Instantiate(Object original, Vector3 position, Quaternion rotation, Transform parent);
                Transform pos = gameObject.transform;
                GameObject phit = Instantiate(particleImpact, obj.transform.position, Quaternion.identity, obj.transform);
                // phit.GetComponent<ParticleSystem>().Play();
                // Destroy(phit, phit.GetComponent<ParticleSystem>().main.duration);
            }
        }
    }

    
}

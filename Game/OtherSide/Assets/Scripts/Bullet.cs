using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour
{
    internal int damage; //this is the amount of damage done by the weapon
    [SerializeField] float duration;

    [SerializeField] GameObject particleTrail; // particle trail
    [SerializeField] GameObject particleImpact; // particle hit

    internal GameObject maker;

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
            if (particleImpact != null && (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Player")) // play projectile impact particles, which self-destroy after playing
            {
                // public static Object Instantiate(Object original, Vector3 position, Quaternion rotation);
                Transform pos = gameObject.transform;
                GameObject phit = Instantiate(particleImpact, collision.transform.position, Quaternion.identity);
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
                eScript.ChangeHealth(0 - damage);
                if (maker.GetComponent<AbilityArray>().holderArray[1].ability.isPassive)
                {
                    Debug.Log("Activate passive");
                    maker.GetComponent<AbilityArray>().holderArray[1].ability.Activate(collision.gameObject);
                }
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
                if (maker.GetComponent<AbilityArray>().holderArray[1].ability.isPassive)
                {
                    Debug.Log("Activate passive");
                    maker.GetComponent<AbilityArray>().holderArray[1].ability.Activate(collision.gameObject);
                }
                Destroy(gameObject);
            }

        }


    }
}

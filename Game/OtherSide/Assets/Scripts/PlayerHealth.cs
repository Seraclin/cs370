using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
    
{
    [SerializeField] public int maxHealth = 100; // these should be public - Sam
    [SerializeField] public int health = 100;
    [SerializeField] Slider slider;
    [SerializeField] bool invincibility;
    bool isDeathAnim; //checks if player is dead (so u cant die twice)
    [SerializeField] float invincibilityTime;
    [SerializeField] PlayerController pc;
    [SerializeField] Collider2D col;
    [SerializeField] GameObject gameOverScreen;

    [SerializeField] Animator anim;  // for animations to transition
    [SerializeField] GameObject particleHit; // particle prefab
    private GameObject phit; // particle when you get hit
    private bool tookdamage = false; // returns true if health was decreased, otherwise false


    private void Start()
    {
        anim = GetComponent<Animator>();
        
    }

    public bool ChangeHealth(int h)
    {
        tookdamage = false; // returns true if health was decreased, otherwise false
        if (invincibility && h < 0) // negate incoming damage, when invincible
        {
            h = 0;
            tookdamage = false;
        }

        if (h < 0 && invincibility == false) // take incoming dmg
        {
            health += h;
            tookdamage = true; // we take damage
            slider.value = health;
            invincibility = true;

            if(health < 0 && !isDeathAnim)  // player is dead
            {
                isDeathAnim = true;
                // Death animation - Sam
                anim.SetBool("isDead", true);
                // TODO: Delay death screen

                //RESTART GAME -JC
                pc.enabled = false;
                Invoke("Death", 1.2f);
                //newScreen.SetActive(true);
              
                
            }
            
            // col.enabled = false ; // don't disable the box collider as it leads to undefined behavior for other triggers - Sam
            // instead negate damage to 0 when invincible
            Invoke("RemoveInvincibility", invincibilityTime);
        }
        else // invincible or healing
        {
            health += h;
            tookdamage = false; // we take no damage

            if (health > maxHealth)
            {
                health = maxHealth;
            }
            slider.value = health;
        }
        if(isDeathAnim == false && particleHit != null && h != 0)
        {
            // hit particle effect, only when damage is not zero
            phit = Instantiate(particleHit, gameObject.transform);
            phit.GetComponent<ParticleSystem>().Play();
            Destroy(phit, phit.GetComponent<ParticleSystem>().main.duration);
        }
        return tookdamage;
    }
    void Death()
    {
        gameOverScreen.SetActive(true);
        // Invoke("Respawn", 0.95f);
    }
    void Respawn()
    {
        gameOverScreen.SetActive(false);
        health = 100;
        slider.value = health;
        pc.enabled = true;
        isDeathAnim = false;
        anim.SetBool("isDead", false);
    }
    void RemoveInvincibility()
    {
        col.enabled = true; // use something else for invincibility
        invincibility = false;
    }
}

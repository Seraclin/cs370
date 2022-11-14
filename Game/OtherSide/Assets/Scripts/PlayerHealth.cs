using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
    
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] int health = 100;
    [SerializeField] Slider slider;
    [SerializeField] bool invincibility;
    bool isDeathAnim; //checks if player is dead (so u cant die twice)
    [SerializeField] float invincibilityTime;
    [SerializeField] PlayerController pc;
    [SerializeField] Collider2D col;
    [SerializeField] GameObject gameOverScreen;

    [SerializeField] Animator anim;  // for animations to transition

    private void Start()
    {
        anim = GetComponent<Animator>();
    }


    public void ChangeHealth(int h)
    {

        if (h < 0 && invincibility == false)
        {
            health += h;
            slider.value = health;
            invincibility = true;
            if(health < 0 && !isDeathAnim)  // player is dead
            {
                isDeathAnim = true;
                // Death animation - Sam
              //  anim.SetBool("isDead", true);
                // TODO: Delay death screen

                //RESTART GAME -JC
                pc.enabled = false;
                Invoke("Death", 1f);
                //newScreen.SetActive(true);
              
                
            }
            
            col.enabled = false ;
            Invoke("RemoveInvincibility", invincibilityTime);
        }
        else
        {
            health += h;
            slider.value = health;
        }
    }
    void Death()
    {
        gameOverScreen.SetActive(true);
        Invoke("Respawn", 0.95f);
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

        col.enabled = true;
        invincibility = false;
    }
}

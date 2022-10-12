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
    [SerializeField] float invincibilityTime;
    [SerializeField] PlayerController pc;
    [SerializeField] Collider2D col;
    [SerializeField] GameObject DeathScreen;

    private void Start()
    {
        this.gameObject.GetComponent<PlayerController>().enabled = true;
    }

    public void ChangeHealth(int h)
    {

        if (h < 0 && invincibility == false)
        {
            health += h;
            slider.value = health;
            invincibility = true;
            if(health < 0)
            {
                //FOR TESTING PURPOSES
                YouDied();
                slider.value = health;
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
   
    void RemoveInvincibility()
    {
        col.enabled = true;
        invincibility = false;
    }
    void YouDied()
    {
        this.gameObject.SetActive(false);
        Instantiate(DeathScreen);
    }
}

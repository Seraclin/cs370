using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerHealth : MonoBehaviour
    
{
     public int maxHealth = 100; // these should be public - Sam
     public int health = 100;
    [SerializeField] public float damageCoef = 1f;
    [SerializeField] Slider slider;
    [SerializeField] bool invincibility;
    bool isDeathAnim; //checks if player is dead (so u cant die twice)
    [SerializeField] float invincibilityTime;
    [SerializeField] PlayerController pc;
    [SerializeField] Collider2D col;
    [SerializeField] GameObject gameOverScreen;

    [SerializeField] Animator anim;  // for animations to transition
    [SerializeField] GameObject particleHit; // particle prefab for taking damage
    [SerializeField] GameObject particleHeal; // particle for healing
    private GameObject phit; // particle when you get hit
    [SerializeField] Vector3 respawnPoint;
    PhotonView pv;

    private bool tookdamage = false; // returns true if health was decreased, otherwise false



    private void Start()
    {
        anim = GetComponent<Animator>();
        respawnPoint = this.transform.position;
        pv = GetComponent<PhotonView>();
        
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
            pv.RPC("SyncHealth", RpcTarget.OthersBuffered, health);
            if (health < 0 && !isDeathAnim)  // player is dead
            {
                slider.value = 0;
                isDeathAnim = true;
                // Death animation - Sam
                anim.SetBool("isDead", true);
                // TODO: Delay death screen

                //RESTART GAME -JC
                pc.enabled = false;
                Invoke("Respawn", 1.2f);
                // anim.SetBool("isDead", true);
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
        if(isDeathAnim == false && particleHit != null && h < 0)
        {
            // hit particle effect, only when damage, i.e. h is less than zero/negative
            phit = Instantiate(particleHit, gameObject.transform);
            phit.GetComponent<ParticleSystem>().Play();
            Destroy(phit, phit.GetComponent<ParticleSystem>().main.duration);
        }
        else if (isDeathAnim == false && particleHeal != null && h > 0)
        {
            // heal particle effect, only when healing, i.e. h is greater than zero/positive
            phit = Instantiate(particleHeal, gameObject.transform);
            phit.GetComponent<ParticleSystem>().Play();
            Destroy(phit, phit.GetComponent<ParticleSystem>().main.duration);
        }
        return tookdamage;
    }
   [PunRPC] void SyncHealth(int h)
    {
        slider.value = h;
    }
    void Respawn()
    {
        //gameOverScreen.SetActive(false);
        this.transform.position = respawnPoint;
        health = 100;
        slider.value = health;
        pv.RPC("SyncHealth", RpcTarget.OthersBuffered, health);
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

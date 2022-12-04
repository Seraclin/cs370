using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] public int maxHealth = 100; // made these public so I can grab -Sam
    [SerializeField] public int health = 100; // enemy's current health

    [SerializeField] bool invincibility;
    [SerializeField] float invincibilityTime;
    [SerializeField] Collider2D col;

    private bool tookdamage = false; // returns true if health was decreased, otherwise false
    /*
     * This the script for enemy taking damage.
     * Ideally, the changehealth() is invoked by an Ability object
     * and passed in a damage (h<0) or healing (h>0) value from the Ability object's damage variable.
     * If the enemy health <=0, it will die
     */
    public bool ChangeHealth(int h)
    { // 'h' is the ability damage
        tookdamage = false;

        if (h < 0 && invincibility == false) // enemy isn't invincible so it takes damage
        {
            health += h;
            tookdamage = true; // health was changed
            invincibility = true;
            if (health <= 0) // enemy dies, might have to change equality later
            {
                
                Enemy enemyScript = this.GetComponent<Enemy>();
                enemyScript.isDead = true;
                this.gameObject.SetActive(false);

                // Destroy(this); // deletes this game object and all children & components
                // TODO: create a corpse which takes enemy ability and enemy name/sprite bore deletion.
                // TODO: Probably should change logic to instead make another gameObject (e.g. gravestone)
                // that inherits enemy abilities before deletion. And player then can possess that instead.
                // TODO: ability owner is ambigiuous, so it ability object doesn't disappear with enemy
            }
            col.enabled = false;
            Invoke("RemoveInvincibility", invincibilityTime);
        }
        else if (h > 0)// enemy is healed
        {
            health += h;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
        }
        return tookdamage;
    }
    void RemoveInvincibility()
    {
        col.enabled = true;
        invincibility = false;
    }
}

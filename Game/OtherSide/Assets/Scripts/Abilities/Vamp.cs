using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Vamp is a secondary ability that enables the holder to heal when dealing damage
 * This ability (probably) should only work with melee
 * Passive ability (probably)
 * Right now, passive is just an ability with 0 cooldown
 */

[CreateAssetMenu(fileName = "Vamp", menuName = "Scripts/Secondary/Vamp", order = 1)]
public class Vamp : Ability
{
    
    public override void Activate(GameObject parent)
    {
        // Debug.Log("Vamp Ability activated");
        if (parent.tag == "Enemy")
        {
            Enemy eScript = parent.GetComponent<Enemy>();
            eScript.ChangeHealth(damage);

        }
        else if (parent.tag == "Player")
        {
            // Debug.Log("Player hit, health reduce by " + damage);
            PlayerHealth eScript = parent.GetComponent<PlayerHealth>();
            eScript.ChangeHealth(damage);
        }

    }

    /*
     * Deactivate is called when the skill is cooldown
    */
    public override void Deactivate(GameObject parent) // 
    {
        // Debug.Log("Vamp Ability deactivated");

    }

}

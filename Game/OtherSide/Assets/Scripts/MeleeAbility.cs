using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * MeleeAbility ScriptableObject inherits attributes from the Ability class.
 * Default name is "New_MeleeAbility", and you can fill out its attributes in the editor.
 * You can drag-and-drop this as an asset to a script reference such as
 * an "AbilityHolder" script to handle ability cooldown/behavior.
 * This is the basic melee ability.
 * It uses no resource and have a very low cd.
 */
[CreateAssetMenu(fileName = "New_MeleeAbility", menuName = "Scripts/MeleeAbility", order = 2)]
public class MeleeAbility : Ability
{
    public GameObject meleeInstance; // The prefab of melee (currently it's a placeholder from the existing sprites)
    private Transform spawnPosition; // The position of generating the meleeInstance
    private GameObject cloneSkillPrefab; // Reference to the generated melee object
    public GameObject owner;
    private SpriteRenderer ren;

    /* Activate is called when the skill is active
     * define ability behavior: generate an object with a collider as the hitbox of the melee   
     */
    public override void Activate(GameObject parent) 
    {
        Debug.Log("Melee Ability activated");
        owner = parent;
        spawnPosition = parent.transform;
        ren = parent.GetComponent<SpriteRenderer>();

        if (ren.flipX)
        {
            meleeInstance.GetComponent<SpriteRenderer>().flipX = false;
            cloneSkillPrefab = Instantiate(meleeInstance, spawnPosition.position + new Vector3(1, 0, 0), spawnPosition.rotation,parent.transform);
        }
        else
        {
            meleeInstance.GetComponent<SpriteRenderer>().flipX = true;
            cloneSkillPrefab = Instantiate(meleeInstance, spawnPosition.position - new Vector3(1, 0, 0), spawnPosition.rotation,parent.transform);
        }
        
    }

    /* Deactivate is called when the skill is cooldown
    * destroy the melee object
    */
    public override void Deactivate(GameObject parent) // 
    {
        Debug.Log("Melee Ability deactivated");
        Destroy(cloneSkillPrefab);
    }
}

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

    private Vector3 direction;

    /* Activate is called when the skill is active
     * define ability behavior: generate an object with a collider as the hitbox of the melee   
     */
    public override void Activate(GameObject parent) 
    {
        Debug.Log("Melee Ability activated");

        spawnPosition = parent.transform;
        Vector3 displacement = new Vector3(0, 0, 0);

        if (parent.tag == "Player")
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.transform.position.z * -1;
            displacement = Camera.main.ScreenToWorldPoint(mousePosition) - spawnPosition.position;
        }
        else if (parent.tag == "Enemy")
        {
            Vector3 playerPosition = parent.GetComponent<Enemy>().player.GetComponent<Transform>().position;
            displacement = playerPosition - spawnPosition.position;
        }


        direction = displacement.normalized;

        cloneSkillPrefab = Instantiate(meleeInstance, spawnPosition.position + direction, spawnPosition.rotation, parent.transform);

        cloneSkillPrefab.tag = parent.tag + "Ability";

        float rotZ = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        cloneSkillPrefab.transform.rotation = Quaternion.Euler(0f, 0f, -rotZ - 90);

        cloneSkillPrefab.GetComponent<AbilityDamage>().damage = damage;
        
        FindObjectOfType<AudioManager>().Play("meleeAttack", cloneSkillPrefab); 
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

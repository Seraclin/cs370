using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 
 */
[CreateAssetMenu(fileName = "New_Thrower", menuName = "Scripts/ThrowerAbility", order = 3)]
public class Thrower : Ability
{
    public GameObject meleeInstance; // The prefab
    private Transform spawnPosition; // The position of generating the Instance
    private GameObject cloneSkillPrefab; // Reference to the generated object

    private Vector3 direction;

    
    public override void Activate(GameObject parent)
    {
        // Debug.Log("Melee Ability activated");
        FindObjectOfType<AudioManager>().Play("meleeAttack");

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

        cloneSkillPrefab.GetComponent<DamageOverTime>().damage = damage;
    }

    /* Deactivate is called when the skill is cooldown
    * destroy the melee object
    */
    public override void Deactivate(GameObject parent) // 
    {
        // Debug.Log("Melee Ability deactivated");
        // Destroy(cloneSkillPrefab);
    }
}

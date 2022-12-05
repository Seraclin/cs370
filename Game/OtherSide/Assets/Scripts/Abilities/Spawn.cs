using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Spawn", menuName = "Scripts/Spawn")]
public class Spawn : Ability
{
    public GameObject enemyInstance; // The prefab of melee (currently it's a placeholder from the existing sprites)
    private Transform spawnPosition; // The position of generating the meleeInstance
    private GameObject cloneSkillPrefab; // Reference to the generated melee object

    private Vector3 direction;

    [SerializeField] GameObject spawnParticle; // particle

    /* Activate is called when the skill is active
     * define ability behavior: generate an object with a collider as the hitbox of the melee   
     */
    public override void Activate(GameObject parent)
    {
        // Debug.Log("Melee Ability activated");

        spawnPosition = parent.transform;
        Vector3 displacement = new Vector3(0, 0, 0);

        if (parent.tag == "Enemy")
        {
            Vector3 playerPosition = parent.GetComponent<Enemy>().player.GetComponent<Transform>().position;
            displacement = playerPosition - spawnPosition.position;
            direction = displacement.normalized;
            if (Vector3.Distance(spawnPosition.position + 10 * direction, playerPosition) > 3)
            {
                cloneSkillPrefab = Instantiate(enemyInstance, spawnPosition.position + 10 * direction, spawnPosition.rotation);
                GameObject phit = Instantiate(spawnParticle, cloneSkillPrefab.transform);
                cloneSkillPrefab.GetComponent<Enemy>().player = parent.GetComponent<Enemy>().player;
                
                FindObjectOfType<AudioManager>().Play("meleeAttack", cloneSkillPrefab);
            }
        }

       
    }

    /* Deactivate is called when the skill is cooldown
    * destroy the melee object
    */
    public override void Deactivate(GameObject parent) 
    {
        // Debug.Log("Melee Ability deactivated");
        // Destroy(cloneSkillPrefab);
    }
}

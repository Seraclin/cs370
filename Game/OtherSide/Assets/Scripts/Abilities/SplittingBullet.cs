using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New_SplittingBullet", menuName = "Scripts/SplittingBullet")]
public class SplittingBullet : Ability
{
    public GameObject RangeInstance; // prefab for the projectile
    private Transform spawnPosition; // position of generating the instance
    //public int projectileRange; // how far the projectile can travel in pixels
    private GameObject cloneSkillPrefab; // store generated ability object
    private GameObject cloneSkillPrefab2;
    private GameObject cloneSkillPrefab3;
    private Vector3 direction;
    private Vector3 direction2;
    private Vector3 direction3;
    public float projectileSpeed; // how fast the projectile travels across the range, it's actually the force
    //public float projectileSize; // how big the projectile is
    // public float projectileDuration; // how long the projectile exists, don't need this if we have an 'activeTime' in the base Ability class
    // Two options: calculate predetermined distance by having projectile travel from start to projectileRange at 'speed', or we can have it only last for a 'duration' and it travels at 'speed'

    public override void Activate(GameObject parent)
    {

        // Debug.Log("Range Ability activated");

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
        direction2 = Quaternion.Euler(0, 0, 30) * direction;
        direction3 = Quaternion.Euler(0, 0, -30) * direction;

        cloneSkillPrefab = Instantiate(RangeInstance, spawnPosition.position + direction, spawnPosition.rotation);
        cloneSkillPrefab2 = Instantiate(RangeInstance, spawnPosition.position + direction, spawnPosition.rotation);
        cloneSkillPrefab3 = Instantiate(RangeInstance, spawnPosition.position + direction, spawnPosition.rotation);

        cloneSkillPrefab.tag = parent.tag + "Ability";
        cloneSkillPrefab2.tag = parent.tag + "Ability";
        cloneSkillPrefab3.tag = parent.tag + "Ability";

        float rotZ = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        cloneSkillPrefab.transform.rotation = Quaternion.Euler(0f, 0f, -rotZ - 90);
        cloneSkillPrefab2.transform.rotation = Quaternion.Euler(0f, 0f, -rotZ - 60);
        cloneSkillPrefab3.transform.rotation = Quaternion.Euler(0f, 0f, -rotZ - 120);

        cloneSkillPrefab.GetComponent<Rigidbody2D>().AddForce(direction * projectileSpeed, ForceMode2D.Force);
        cloneSkillPrefab2.GetComponent<Rigidbody2D>().AddForce(direction2 * projectileSpeed, ForceMode2D.Force);
        cloneSkillPrefab3.GetComponent<Rigidbody2D>().AddForce(direction3 * projectileSpeed, ForceMode2D.Force);

        cloneSkillPrefab.GetComponent<Bullet>().damage = damage;
        cloneSkillPrefab.GetComponent<Bullet>().maker = parent;

        cloneSkillPrefab2.GetComponent<Bullet>().damage = damage;
        cloneSkillPrefab2.GetComponent<Bullet>().maker = parent;

        cloneSkillPrefab3.GetComponent<Bullet>().damage = damage;
        cloneSkillPrefab3.GetComponent<Bullet>().maker = parent;

        FindObjectOfType<AudioManager>().Play("fireballAttack", cloneSkillPrefab);
        FindObjectOfType<AudioManager>().Play("fireballAttack", cloneSkillPrefab2);
        FindObjectOfType<AudioManager>().Play("fireballAttack", cloneSkillPrefab3);
    }

    /* Deactivate is called when the skill is cooldown
    * destroy the melee object
    */
    public override void Deactivate(GameObject parent) // 
    {
        Debug.Log("Range Ability deactivated");
        //Destroy(cloneSkillPrefab);
    }



}

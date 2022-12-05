using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/*
 * RangedAbility ScriptableObject inherits attributes from the Ability class.
 * Default name is "New_RangedAbility", and you can fill out its attributes in the editor.
 * You can drag-and-drop this as an asset to a script reference such as
 * an "AbilityHolder" script to handle ability cooldown/behavior.
 */
[CreateAssetMenu(fileName = "New_RangedAbility", menuName = "Scripts/RangedAbility", order = 1)]
public class RangedAbility : Ability
{
    // RangedAbility inherits attributes from Ability object

    // Extra defined variables for RangedAbility
    public GameObject RangeInstance; // prefab for the projectile
    private Transform spawnPosition; // position of generating the instance
    //public int projectileRange; // how far the projectile can travel in pixels
    private GameObject cloneSkillPrefab; // store generated ability object

    private Vector3 direction; 
    public float projectileSpeed;
    [SerializeField] PhotonView pv;
    [SerializeField] PhotonTransformView ptv;
    // how fast the projectile travels across the range, it's actually the force
    //public float projectileSize; // how big the projectile is
    // public float projectileDuration; // how long the projectile exists, don't need this if we have an 'activeTime' in the base Ability class
    // Two options: calculate predetermined distance by having projectile travel from start to projectileRange at 'speed', or we can have it only last for a 'duration' and it travels at 'speed'
    
    public override void Activate(GameObject parent)
    {
        pv = parent.GetComponent<PhotonView>();
        ptv = parent.GetComponent<PhotonTransformView>();
        Debug.Log("Range Ability activated");
        FindObjectOfType<AudioManager>().Play("fireballAttack"); 

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
        if (pv.IsMine)
        {
            cloneSkillPrefab = PhotonNetwork.Instantiate(RangeInstance.name, spawnPosition.position + direction, spawnPosition.rotation);


            //cloneSkillPrefab = Instantiate(RangeInstance, spawnPosition.position + direction, spawnPosition.rotation);

            cloneSkillPrefab.tag = parent.tag + "Ability";

            float rotZ = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            cloneSkillPrefab.transform.rotation = Quaternion.Euler(0f, 0f, -rotZ - 90);

            cloneSkillPrefab.GetComponent<Rigidbody2D>().AddForce(direction * projectileSpeed, ForceMode2D.Force);

            cloneSkillPrefab.GetComponent<Bullet>().damage = damage;
            cloneSkillPrefab.GetComponent<Bullet>().maker = parent;
        }

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

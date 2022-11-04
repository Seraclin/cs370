using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This is the possession skill for the Player only. 
 * You can think of this similar to Cappy in Mario Odyssey, where the player can possess a target,
 * and temporarily gain different abilities depending on the enemy.
 * Currently, this only prints to the console, when the button is pressed, who's the closest possessable entity in range.
 * It also changes the player sprite/animation to that entity.
 * Treat this as a customly defined skill (i.e. doesn't use AbilityHolder.cs), since it's uniquely for the player only
 */
// [CreateAssetMenu(fileName = "New_PossessionAbility", menuName = "Scripts/PossessionAbility", order = 3)]

public class PossessionAbility : MonoBehaviour
{
    [SerializeField] public KeyCode key; // assign button in editor for possession, press again to end early

    [SerializeField] public Sprite abilityIcon; // icon for ability
    [SerializeField] public string abilityName; // name of the ability
    [SerializeField] public string abilityDescription; // description of ability
    [SerializeField] public Collider2D possessCollider; // possession collider, TODO: add
    [SerializeField] public float stamina; // a resource linked to possesion? possession HP?
    [SerializeField] public Sprite indicator; // something to indicate you are possessing something? TODO

    public float radius; // possession circular radius, currently using this for testing
    public float duration; // max duration you can possess something?

    // Player original information to store for when you unpossess
    private Sprite originalSprite;
    private Animation anim;
    // private vars for tracking possession information
    private List<GameObject> possessable; // tracks what possessable objects are in-range
    private GameObject closest = null; // closest thing to possess
    private List<Ability> abilities; // tracks what abilities are granted during possession

    void Start()
    {
        
    }

    void Update()
    {
        
    }


    /* Possess the closest isPossessable object, if possible
     * Returns true if successfully possess, otherwise returns false
     * Max range is the collider possessCollider
     */
    public bool activate(GameObject parent)
    {
        Debug.Log("Possession button pressed");
        // Vector3 playerPosition = parent.transform.position; // player's position
        float minDistance = 0;
        if(possessable.Count == 0) // if list is empty
        {
            Debug.Log("No possessable objects in range");
        }

        Debug.Log("Possesion Activated");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // TODO: remove this later
        Debug.Log("All possessable objects:" + enemies.ToString());
        GameObject closestObj = GetClosestEnemy(new List<GameObject>(enemies), parent.transform);
        closest = closestObj;
        possessable.Add(closest); // TODO: determine possession range/priority, currently possesses closest enemy
        // TODO: remove reliance on "Enemy" tag and add 'posessable' variable for things that you can possess?
        
        
        // Turn player sprite to the game object sprite, might mess up animations
        parent.GetComponent<SpriteRenderer>().sprite = closest.GetComponent<SpriteRenderer>().sprite; // changes player's sprite to the possessed object's sprite
        parent.GetComponent<SpriteRenderer>().color = closest.GetComponent<SpriteRenderer>().color; // get enemy color
        // parent.GetComponent<Animator>().enabled = !parent.GetComponent<Animator>().enabled;

        if (closest.GetComponent<Animator>() != null) // TODO: check if enemy has an animation to use
        {
            Debug.Log("Using enemy animations now");
            parent.GetComponent<Animator>().runtimeAnimatorController = closest.GetComponent<Animator>().runtimeAnimatorController;
        } else // if no enemy animation, then player will use enemy sprite with no animation
        {
            Debug.Log("Enemy doesn't have an animation");
            parent.GetComponent<Animator>().enabled = !parent.GetComponent<Animator>().enabled;
        }

        // TODO: ? Move the player's position to in front of that possessed object's position? TODO: Adjust later and for out-of-bounds
        parent.transform.position = (closest.transform.position + new Vector3(0, -1.2f, 0));

        // TODO: add enemy abilities to player
        // TODO: destroy the enemy object when possessed

        // TODO: Quit possession early button
        if (Input.GetKeyDown(quitKey))
        {
            Debug.Log("Possession deactivated early");

        }
        return false; // TODO: change later

    }

    /* Deactivate is called when the skill is cooldown
    *  Unposses the enemy
    */
    public void Deactivate(GameObject parent)
    {
        Debug.Log("Possession deactivated");
        parent.GetComponent<SpriteRenderer>().sprite = playerSprite; // changes player's sprite back to what it was orignally
        parent.GetComponent<SpriteRenderer>().color = Color.white; // get enemy color
        parent.GetComponent<Animator>().enabled = true; // reenable player animations

        // TODO: remove enemy abilities from player
    }

    private void checkPossessable() // adds valid possessable objects to possessable list
    {
        // TODO: determine what's possessable
        // For now, this is unused
        /*foreach (GameObject obj in possessable)
        {
            float distance = (parent.transform.position - obj.transform.position).sqrMagnitude;
            if(closest == null || distance < minDistance)
            {
                closest = obj;
                minDistance = distance;
            }
        }*/
    }
    private GameObject GetClosestEnemy(List<GameObject> enemies, Transform fromThis)
    { // returns the closest enemy to the player
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = fromThis.position;
        foreach (GameObject potentialTarget in enemies)
        {
            Transform potentialTargetTransform = potentialTarget.transform;
            Vector3 directionToTarget = potentialTargetTransform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        return bestTarget;
    }
} // end of file

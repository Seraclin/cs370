using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * TLDR: Viego passive but with Yuumi duration
 * This is the possession skill for the Player only.
 * Put this script onto a separate collider object that is child to player
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
    [SerializeField] public GameObject player; // player Game object


    [SerializeField] public Sprite abilityIcon; // icon for ability
    [SerializeField] public string abilityName; // name of the ability
    [SerializeField] public string abilityDescription; // description of ability
    // [SerializeField] public Collider2D possessCollider; // possession collider, TODO: add
    [SerializeField] public int stamina; // a resource linked to possesion? possession HP?
    [SerializeField] public Sprite indicator; // something to indicate you are possessing something? TODO
    [SerializeField] public float cooldown; // cd for how when you can possess again after unpossessing


    // local variable fields for posession status
    public float radius; // possession circular radius, currently using this for testing
    public float duration; // max duration you can possess something? Or current possession duration
    private float cooldownTime; // tracking current cooldown time
    // public bool isPossessing; // true, if you are currently "possessing" something, use AbilityState active instead

    // Old: Player original information to store for when you unpossess, denoted with zero 0
    private Sprite sprite0; // player sprite
    private Animator anim0; // player animation controller
    private AbilityHolder[] abil0 = new AbilityHolder[3]; //ability size of 3, just in case
    // private List<AbilityHolder> abilOther; // list any other abilities
    private int hpMax0; // get old max HP, prob uneeded

    // New: possession information, denoted with 1
    private List<GameObject> possessable = new List<GameObject>(); // tracks what possessable objects are in-range
    private GameObject closest = null; // closest thing to possess
    private AbilityHolder [] abilNew; // array tracks what abilities are granted during possession, for now only assumes a max of two abilities
    private Sprite sprite1;
    private Animator anim1;

    enum AbilityState // possession is either ready, being activated, or on cooldown
    {
        ready,
        active,
        cooldown
    }
    AbilityState state = AbilityState.ready; // default ability state is ready


    void Start()
    {
        duration = 0;
        // initialize player defaults
        abil0 = GetComponents<AbilityHolder>();
        player = GameObject.FindWithTag("Player");
        anim0 = gameObject.transform.parent.gameObject.GetComponentInParent<Animator>(); // gets the parent game object



    }

    void Update()
    {
        switch (state) // check AbilityState and switch accordingly
        {
            case AbilityState.ready:
                if (Input.GetKeyDown(key))
                {
                    if (possessable.Count > 0 && activate(player))
                    {
                        // activate ability on the GameObject that the script is attached to, true if successful
                        state = AbilityState.active;
                        Debug.Log("Possessing new Entity");
                    }
                    else // no entities able to possess in range
                    {
                        Debug.Log("No Possessable Entities in Range");
                        // TODO: display error text on screen
                    }
                }
                break;
            case AbilityState.active:
                // what to do while ability is active (i.e. we're possessing something)
                duration += Time.deltaTime; // time how long we have been possessing

                if (Input.GetKeyDown(key)) // player is currently possessing something, quit early
                {
                    // TODO: quit possession early
                    // player = gameObject.transform.parent.gameObject; // gets the parent game object
                    Deactivate(player);
                    state = AbilityState.cooldown;
                    cooldownTime = cooldown;
                }

                if(stamina < 0) // possession HP / stamina is used up, go on cooldown
                {
                    // TODO: modify playerHealth to take account stamina
                    Deactivate(player);
                    state = AbilityState.cooldown;
                    cooldownTime = cooldown;
                }
                break;
            case AbilityState.cooldown:
                if (cooldownTime > 0) // still on cooldown
                {
                    cooldownTime -= Time.deltaTime; // decrement cooldown
                    Debug.Log("Possession still on cooldown. Need to wait: " + cooldownTime);
                }
                else // ability cooldown done, go back to ready
                {
                    state = AbilityState.ready;
                }
                break;
        }
        
        
    }

    private void OnTriggerEnter2D(Collider2D collision) // in possessable range
    {
        if(collision.tag == "Enemy")
        {
            Debug.Log("Possessable object in range");
            possessable.Add(collision.gameObject);
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision) // leaves possessable range
    {
        if (collision.tag == "Enemy" && possessable.Contains(collision.gameObject))
        {
            possessable.Remove(collision.gameObject);
        }
    }

    /* Possess the closest isPossessable object, if possible
     * Returns true if successfully possess, otherwise returns false
     * Max range is the collider possessCollider
     */
    public bool activate(GameObject parent)
    {
        // Vector3 playerPosition = parent.transform.position; // player's position
        float minDistance = 0;
        if(possessable.Count == 0) // if list is empty
        {
            return false;
        }

        // GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // TODO: remove this later and use onTriggerEnter
        Debug.Log("All possessable objects:" + possessable.ToString());
        GameObject closestObj = GetClosestPossessable(possessable, parent.transform);
        if(closestObj == null) // another redundant check for null exception
        {
            return false;
        }
        closest = closestObj;
        // TODO: determine possession range/priority, currently possesses closest enemy
        // TODO: remove reliance on "Enemy" tag and add 'posessable' variable for things that you can possess?
        
        
        // Turn player sprite to the game object sprite, might mess up animations
        parent.GetComponent<SpriteRenderer>().sprite = closest.GetComponent<SpriteRenderer>().sprite; // changes player's sprite to the possessed object's sprite
        parent.GetComponent<SpriteRenderer>().color = closest.GetComponent<SpriteRenderer>().color; // get enemy color

        if (closest.GetComponent<Animator>() != null) // check if enemy has an animation to use
        {
            parent.GetComponent<Animator>().runtimeAnimatorController = closest.GetComponent<Animator>().runtimeAnimatorController; // change animator controller           
        } else
        {
            Debug.Log("Enemy doesn't have an animation!");
            parent.GetComponent<Animator>().enabled = !parent.GetComponent<Animator>().enabled;
        }


        // TODO: add multiple enemy abilities to player
        abilNew = closest.GetComponents<AbilityHolder>();
        AbilityHolder[] abilCurr = GetComponents<AbilityHolder>();
        for (int i = 0; i < abilCurr.Length; i++)
        {
            abilCurr[i].ability = abilNew[i].ability; 
        }
        Debug.Log("Closest enemy is "+closest.name+" with abilities: "+abilNew.ToString());

        // TODO: ? Move the player's position to in front of that possessed object's position? TODO: Adjust later and for out-of-bounds
        // parent.transform.position = (closest.transform.position + new Vector3(0, -1.2f, 0));
        // TODO: destroy the enemy object when possessed
        // TODO: add indicator of what you're currently possessing
        return true; // TODO: change later

    }

    /* Deactivate is called when the skill is cooldown
    *  Unposses the enemy and restore player's original appearence/abilties
    */
    public void Deactivate(GameObject parent)
    {
        Debug.Log("Possession deactivated");
        parent.GetComponent<SpriteRenderer>().sprite = sprite0; // changes player's sprite back to what it was orignally
        parent.GetComponent<SpriteRenderer>().color = Color.white; // get default color
        parent.GetComponent<Animator>().runtimeAnimatorController = anim0.runtimeAnimatorController; // reenable player animations

        // TODO: remove enemy abilities from player, and re-add old player abilites
        AbilityHolder [] abilsTemp = parent.GetComponents<AbilityHolder>();
        for(int i = 0; i < abilsTemp.Length; i++)
        {
            abilsTemp[i].ability = abil0[i].ability; // TODO: fix error: array out of bounds
        }

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
    private GameObject GetClosestPossessable(List<GameObject> posList, Transform fromThis)
    { // returns the closest enemy to the player by Euclidean distance
        GameObject bestTarget = null;
        if(posList.Count == 0)
        {
            return null;
        }
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = fromThis.position;
        foreach (GameObject potentialTarget in posList)
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

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
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
    [SerializeField] public GameObject indicator; // something to indicate you are possessing something? TODO
    [SerializeField] public float cooldown; // cd for how when you can possess again after unpossessing, 0.2 or higher is recommended as framerate will screw up lower times


    // local variable fields for posession status
    public float radius; // possession circular radius, currently using this for testing
    public float duration; // max duration you can possess something? Or current possession duration
    private float cooldownTime; // tracking current cooldown time
    [SerializeField] public Animator anim; // anim component to use
    private bool isPossessing = false; // true, if you are currently "possessing" something, use AbilityState active instead
    private GameObject indicatorObj; // delete this when not possessing anything

    // Old: Player original information to store for when you unpossess, denoted with zero 0
    private Sprite sprite0; // player sprite
    private RuntimeAnimatorController anim0; // player animator controller
    [SerializeField] Ability[] abil0 = new Ability[3]; //ability size of 3, drag original abilities via Inspector
    // private List<AbilityHolder> abilOther; // list any other abilities
    private int hpMax0; // get old max HP, prob uneeded

    // New: possession information, denoted with 1
    private List<GameObject> possessable = new List<GameObject>(); // tracks what possessable objects are in-range
    private GameObject closest = null; // closest thing to possess
    private Ability [] abilNew; // array tracks what abilities are granted during possession, for now only assumes a max of two abilities
    private Sprite sprite1;
    private RuntimeAnimatorController anim1; // enemy animator controller

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
        player = GameObject.FindWithTag("Player");
        anim = gameObject.transform.parent.gameObject.GetComponentInParent<Animator>(); // gets the parent gameobject's "Animator" component
        anim0 = gameObject.transform.parent.gameObject.GetComponentInParent<Animator>().runtimeAnimatorController; // the animator controller used for "Animator" component
        // anim0 = this.GetComponentInParent<Animator>(); // original animator


    }

    void Update()
    {
        switch (state) // check AbilityState and switch accordingly
        {
            case AbilityState.ready:
                if (Input.GetKeyDown(key))
                {
                    if (possessable.Count > 0 && !isPossessing && activate(player))
                    {
                        // activate ability on the GameObject that the script is attached to, true if successful
                        state = AbilityState.active;
                        Debug.Log("Possessing new Entity");
                        isPossessing = true;
                    }
                    else if (isPossessing)
                    {
                        Debug.Log("Already possessing an enemy");
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
                    // player = gameObject.transform.parent.gameObject; // gets the parent game object
                    Deactivate(player);
                    state = AbilityState.cooldown;
                    cooldownTime = cooldown;
                    isPossessing = false;
                    anim.SetBool("isPossessing", false);

                }

                if (stamina < 0) // possession HP / stamina is used up, go on cooldown
                {
                    // TODO: modify playerHealth to take account stamina, currently uses same HP as default form
                    Deactivate(player);
                    state = AbilityState.cooldown;
                    cooldownTime = cooldown;
                    anim.SetBool("isPossessing", false);
                    isPossessing = false;
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
        if(collision.tag == "Enemy" && collision.gameObject.GetComponent<Enemy>().isPossessable)
        {
            Debug.Log("Possessable object in range");
            possessable.Add(collision.gameObject);
        }
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && collision.gameObject.GetComponent<Enemy>().isPossessable && !possessable.Contains(collision.gameObject))
        {
            possessable.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision) // leaves possessable range
    {
        if (possessable.Contains(collision.gameObject))  // TODO: remove reliance on "Enemy" tag and add 'posessable' variable/tag for things that you can possess?
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

        // GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log("All possessable objects:" + possessable.ToString());
        GameObject closestObj = GetClosestPossessable(parent.transform);
        if (closestObj == null) // another redundant check for null exception
        {
            return false;
        }
        closest = closestObj;

        // TODO: get enemy abilities and assign to player; NOTE: order from GetComponents is not guaranteed so edit accordingly or label AbilityHolder scripts
        // closest = a2 = enemy and enemies use EnemyHolder script with ability attach, parent is the player = p2 and uses AbilityHolder script with ability attach 
        EnemyHolder[] abilEnem = closest.GetComponentsInChildren<EnemyHolder>();
        AbilityHolder[] abilPlayer = parent.GetComponents<AbilityHolder>();
        if (abilEnem.Length <= 0)
        {
            Debug.LogWarning("Enemy can't be possessed! No abilities on it!: "+closest.name);
            return false;
        }
        if (abilPlayer.Length > abilEnem.Length) // enemy has less ability slots than player; by default any leftover slots are filled in with whatever player had originally
        {
            Debug.Log("Ability on enemy is: " + abilEnem[0].ability);
            for (int i = 0; i < abilEnem.Length; i++)
            {
                // abilNew[i] = abilEnem[i].ability;
                abilPlayer[i].ability = abilEnem[i].ability;
            }
        }
        else if (abilEnem.Length >= abilPlayer.Length) // enemy has greater than or equal ability slots than player, only takes top 3 abilities on enemy
        {
            for (int i = 0; i < abilPlayer.Length; i++)
            {
                // abilNew[i] = abilEnem[i].ability;
                abilPlayer[i].ability = abilEnem[i].ability; // TODO: probably should scale cooldowns accordingly for player enjoyability
            }
        }

        // Change player sprite/anims to the target game object sprite/animation, might mess up animations
        anim.SetBool("isPossessing", true); // plays possession animation, TODO: add a better possession animation, it currently just uses the death animation

        // Move the player's position to in front of that possessed object's position? Potential clipping issues
        gameObject.transform.parent.position = (closest.transform.position);

        StartCoroutine("changeAnims"); // delay to see possession animtion with coroutine
                                       // 
        // add indicator of what you're currently possessing
        // public static Object Instantiate(Object original, Vector3 position, Quaternion rotation, Transform parent);
        // indicatorObj = Instantiate(indicator, (gameObject.transform.position + new Vector3(0, 1.0f, 0)), Quaternion.identity, gameObject.transform.parent);
        gameObject.transform.GetChild(0).gameObject.SetActive(true); // should show the indicator

        return true;

    }

    /*
     * Helper method to switch visual elements such sprite/animations from player -> closest gameObject
     */

    IEnumerator changeAnims()
    {
        player.GetComponent<PlayerController>().enabled = false; // don't let player move during this time
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f); // wait for transition
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length); // wait for possess animation
        player.GetComponent<SpriteRenderer>().sprite = closest.GetComponent<SpriteRenderer>().sprite; // changes player's sprite to the possessed object's sprite
        Color playerColor = player.GetComponent<SpriteRenderer>().color; // player color ref
        playerColor = closest.GetComponent<SpriteRenderer>().color; // get enemy color
        playerColor.a = 1.0f; // makes the player completely opaque
        player.GetComponent<SpriteRenderer>().color = playerColor; // set player color

        if (closest.GetComponent<Animator>() != null) // check if enemy has an animation to use
        {
            player.GetComponent<Animator>().runtimeAnimatorController = closest.GetComponent<Animator>().runtimeAnimatorController; // change animator controller           
        }
        else
        {
            Debug.LogWarning("'"+closest.name+"' Enemy doesn't have an animation!");
            player.GetComponent<Animator>().enabled = !player.GetComponent<Animator>().enabled;
        }
        // destroy the enemy corpse object when successfully possessed
        possessable.Remove(closest);
        Destroy(closest);
        player.GetComponent<PlayerController>().enabled = true; // let player move now
    }

    /* Deactivate is called when the skill is cooldown
    *  Unposses the enemy and restore player's original appearence/abilties
    */
    public void Deactivate(GameObject parent)
    {
        Debug.Log("Possession deactivated");
        parent.GetComponent<SpriteRenderer>().sprite = sprite0; // changes player's sprite back to what it was orignally
        parent.GetComponent<SpriteRenderer>().color = Color.white; // get default color
        parent.GetComponent<Animator>().runtimeAnimatorController = anim0; // reenable player animations
        // TODO: remove enemy abilities from player, and re-add old player abilites. Again GetComponents doesn't guarantee order so should specify order by putting index on abilityHolder class
        AbilityHolder [] abilsTemp = parent.GetComponents<AbilityHolder>();
        for(int i = 0; i < abilsTemp.Length; i++)
        {
            abilsTemp[i].ability = abil0[i];
        }
        gameObject.transform.GetChild(0).gameObject.SetActive(false); // should hide the indicator

    }

    private void checkPossessable() // adds valid possessable objects to possessable list, unused
    {
        // TODO: determine what's possessable, unused currently
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
    private GameObject GetClosestPossessable(Transform fromThis)
    { // returns the closest enemy to the player by Euclidean distance
        GameObject bestTarget = null;
        // Delete any null elements in possessable, e.g. corpse times out for possession
        for (int i = 0; i < possessable.Count; i++)
        {
            if (possessable[i] == null)
            {
                possessable.RemoveAt(i);
                i--;
            }
        }

        if (possessable.Count == 0) // check if any possessables
        {
            return null;
        }
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = fromThis.position;
        int count = 0;
        foreach (GameObject potentialTarget in possessable)
        {
            Transform potentialTargetTransform = potentialTarget.transform;
            Vector3 directionToTarget = potentialTargetTransform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr && potentialTarget != null)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        
        return bestTarget;
    }
} // end of file

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * This class mainly calls the Ability's activate() then
 * handles the logic for cooldowns provided by 
 * the Ability ScriptableObject attached to this script.
 */
public class AbilityHolder : MonoBehaviour
{
    [SerializeField] public Ability ability; // add an ability object in editor
    float cooldownTime; // this should be inherited from Ability

    float activeTime; // to keep track of how long ability lasts, can use Time.delta possibly

    // Start is called before the first frame update
    void Start()
    {
        // This is not needed.
    }
    enum AbilityState // an ability is either ready, being activated, or on cooldown
    {
        ready,
        active,
        cooldown
    }
    AbilityState state = AbilityState.ready; // default ability state is ready
    [SerializeField] public KeyCode key; // assign button in editor
    // Update is called once per frame
    void Update()
    {
        switch (state) // check AbilityState and switch accordingly
        {
            case AbilityState.ready:
                if (Input.GetKeyDown(key))
                {
                    ability.Activate(gameObject); // activate ability on the GameObject that the script is attached to
                    state = AbilityState.active;
                    activeTime = ability.activeTime;
                }
            break;
            case AbilityState.active:
                // what to do while ability is active or not
                if (activeTime > 0)
                { // TODO: might need to tweak this logic
                    activeTime -= Time.deltaTime;
                }
                else // ability timeup, go on cooldown
                {
                    state = AbilityState.cooldown;
                    cooldownTime = ability.cooldownTime;
                }
            break;
            case AbilityState.cooldown:
                if (cooldownTime > 0) // still on cooldown
                {
                    cooldownTime -= Time.deltaTime; // decrement cooldown
                }
                else // ability cooldown done, go back to ready
                {
                    state = AbilityState.ready;
                }
                break;
        }
        
    }
}

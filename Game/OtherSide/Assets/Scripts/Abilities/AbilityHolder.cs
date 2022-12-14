using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * This class mainly calls the Ability's activate() then
 * handles the logic for cooldowns provided by 
 * the Ability ScriptableObject attached to this script.
 */
public class AbilityHolder : Holder
{
    
    [SerializeField] public KeyCode key; // assign button in editor
    // Update is called once per frame
    void Update()
    {
        if (!ability.isPassive)
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
                    { // TODO: might need to tweak this logic, if you want to deactivate early
                        activeTime -= Time.deltaTime;
                    }
                    else // ability timeup, go on cooldown
                    {
                        ability.Deactivate(gameObject);
                        state = AbilityState.cooldown;
                        cooldownTime = ability.cooldownTime * cdCoefficient;
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
}

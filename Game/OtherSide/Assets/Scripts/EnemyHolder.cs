using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * This class mainly calls the Ability's activate() then
 * handles the logic for cooldowns provided by 
 * the Ability ScriptableObject attached to this script.
 */
public class EnemyHolder : Holder
{
    
    
    // Update is called once per frame
    void Update()
    {
        switch (state) // check AbilityState and switch accordingly
        {
            case AbilityState.ready:
                if (gameObject.GetComponent<Enemy>().player != null) 
                {
                    float dis;
                    dis = Vector3.Distance(gameObject.transform.position, gameObject.GetComponent<Enemy>().player.transform.position);
                    //Debug.Log(dis);
                    if (dis < ability.range && gameObject.GetComponent<Enemy>().player.GetComponent<SpriteRenderer>().color.a > 0.8f) // cast ability if player is detected
                    {
                        ability.Activate(gameObject); // activate ability on the GameObject that the script is attached to
                        state = AbilityState.active;
                        activeTime = ability.activeTime;
                        Debug.Log("Enemy Attack");
                    }
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

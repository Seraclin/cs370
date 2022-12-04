using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Poison is a secondary ability that makes a poisoned status
 * This ability (probably) should only work with poison thrower (or primary abilities related to poison)
 * Passive ability 
 * Right now, passive is just an ability with 0 cooldown
 */
[CreateAssetMenu(fileName = "Poison", menuName = "Scripts/Secondary/Poison", order = 2)]
public class Poison : Ability
{
    // Start is called before the first frame update
    public override void Activate(GameObject target)
    {
        if (!target.GetComponent<Status>().isPoisoned)
        {
            target.GetComponent<Status>().isPoisoned = true;
            target.GetComponent<Status>().poiDuration = activeTime;
            target.GetComponent<Status>().poiDamage = damage;
            target.GetComponent<Status>().poiCount = Mathf.RoundToInt(activeTime);
        }
        if (target.GetComponent<Status>().poiDuration < activeTime)
        {
            target.GetComponent<Status>().poiDuration = activeTime;
            target.GetComponent<Status>().poiCount = Mathf.RoundToInt(activeTime);
        }

    }

    /*
     * Deactivate is called when the skill is cooldown
    */
    public override void Deactivate(GameObject parent) // 
    {
        

    }
}

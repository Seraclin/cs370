using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Slow is a secondary ability that makes a slowed status
 * This ability (probably) should only work with iceball (or primary abilities related to ice)
 * Passive ability 
 * Right now, passive is just an ability with 0 cooldown
 */
[CreateAssetMenu(fileName = "Slow", menuName = "Scripts/Secondary/Slow", order = 3)]
public class Slow : Ability
{
    [SerializeField] float speedCoef;

    public override void Activate(GameObject target)
    {
        // Debug.Log("Slow activated");
        if (!target.GetComponent<Status>().isSlowed)
        {
            target.GetComponent<Status>().isSlowed = true;
            target.GetComponent<Status>().slowDuration = activeTime;
            target.GetComponent<Status>().speedCoef = speedCoef;
            // target.GetComponent<Status>().poiDamage = damage;
            // target.GetComponent<Status>().poiCount = Mathf.RoundToInt(activeTime);
        }
        if (target.GetComponent<Status>().slowDuration < activeTime)
        {
            target.GetComponent<Status>().slowDuration = activeTime;
        }

    }

    /*
     * Deactivate is called when the skill is cooldown
    */
    public override void Deactivate(GameObject parent) // 
    {


    }
}

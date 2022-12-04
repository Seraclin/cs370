using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Analyze is a secondary ability that increase the damage taken
 * This ability (probably) should only work with idk
 * Passive ability 
 * Right now, passive is just an ability with 0 cooldown
 */
[CreateAssetMenu(fileName = "Analyze", menuName = "Scripts/Secondary/Analyze", order = 4)]
public class Analyze : Ability
{
    [SerializeField] float dCoef;

    public override void Activate(GameObject target)
    {
        // Debug.Log("Slow activated");
        if (!target.GetComponent<Status>().isAnalyzed)
        {
            target.GetComponent<Status>().isAnalyzed = true;
            target.GetComponent<Status>().analyzeDuration = activeTime;
            target.GetComponent<Status>().dCoef = dCoef;
            // target.GetComponent<Status>().poiDamage = damage;
            // target.GetComponent<Status>().poiCount = Mathf.RoundToInt(activeTime);
        }
        if (target.GetComponent<Status>().analyzeDuration < activeTime)
        {
            target.GetComponent<Status>().analyzeDuration = activeTime;
        }
    }

    /*
     * Deactivate is called when the skill is cooldown
    */
    public override void Deactivate(GameObject parent) // 
    {


    }
}

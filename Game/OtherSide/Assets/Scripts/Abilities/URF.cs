using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * URF is a testing secondary ability, it will not be in the final game
 */
[CreateAssetMenu(fileName = "URF", menuName = "Scripts/Secondary/URF", order = 1)]
public class URF : Ability
{
    [SerializeField] public float cdReduce;
    // URF inherits attributes from Ability object
    Holder dAbility;
    float oriCoolDown;
    public override void Activate(GameObject parent)
    {
        Debug.Log("URF Ability activated");
        dAbility = parent.GetComponent<AbilityArray>().holderArray[0];
        oriCoolDown = dAbility.cdCoefficient;
        dAbility.cdCoefficient = oriCoolDown * cdReduce;
    }

    /* Deactivate is called when the skill is cooldown
    * destroy the melee object
    */
    public override void Deactivate(GameObject parent) // 
    {
        Debug.Log("URF Ability deactivated");

        dAbility.cdCoefficient = oriCoolDown;
    }



}

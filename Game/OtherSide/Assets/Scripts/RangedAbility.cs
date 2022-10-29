using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * RangedAbility ScriptableObject inherits attributes from the Ability class.
 * Default name is "New_RangedAbility", and you can fill out its attributes in the editor.
 * You can drag-and-drop this as an asset to a script reference such as
 * an "AbilityHolder" script to handle ability cooldown/behavior.
 */
[CreateAssetMenu(fileName = "New_RangedAbility", menuName = "Scripts/RangedAbility", order = 1)]
public class RangedAbility : Ability
{
    // RangedAbility inherits attributes from Ability object

    // Extra defined variables for RangedAbility
    public Sprite projectile; // sprite for the projectile
    public int projectileRange; // how far the projectile can travel in pixels
    public float projectileSpeed; // how fast the projectile travels across the range
    public float projectileSize; // how big the projectile is
    // public float projectileDuration; // how long the projectile exists, don't need this if we have an 'activeTime' in the base Ability class
    // Two options: calculate predetermined distance by having projectile travel from start to projectileRange at 'speed', or we can have it only last for a 'duration' and it travels at 'speed'

    public override void Activate(GameObject parent) // define ability behavior, parent is the GameObject the script is attached to
    {
        // TODO: Put code for shooting stuff from player's position, the "AbilityHolder" script should call this
        Debug.Log("Ranged Ability activated");
    }



}

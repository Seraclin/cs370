using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This is a template class that all should abilities inherit from.
 * Each ability must inherit and define these attributes.
 * You can think of 'Ability' as an asset and you can inherit properties from it to define new abilities.
 */
public class Ability : ScriptableObject
{
    [SerializeField] public Sprite abilityIcon; // icon for ability

    [SerializeField] public string abilityName; // name of the ability
    [SerializeField] public string abilityDescription; // description of ability
    // [SerializeField] public string abilityType; // type of ability (currently TBD)

    [SerializeField] public float cooldownTime; // ability cooldown in seconds
    [SerializeField] public float activeTime; // ability duration in seconds
    [SerializeField] public int damage; // ability damage to enemies
    [SerializeField] public int cost; // how much ability costs resource-wise, ints for now

    [SerializeField] public float knockback; // how much ability will knockback objects, 0 for no knockback


    // ability range depends on the ability
    // audio should be handled by SoundManager (?)
    // You can also add methods here
    public virtual void Activate(GameObject parent) { } // override this method for custom ability activation, maybe should return a bool

    public virtual void Deactivate(GameObject parent) { } // override this method for custom ability deactivation, maybe should return a bool
}

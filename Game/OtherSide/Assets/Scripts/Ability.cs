using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This is a template class that all should abilities inherit from.
 * Each ability must inherit and define these attributes.
 * You can think of 'Ability' as an asset and you can inherit properties from it to define new abilities.
 * Default name is "New_Ability", and you can fill out its attributes in the editor.
 * You can drag-and-drop this "New_Ability" asset to a script reference such as
 * an "AbilityHolder" script to handle ability logic/behanior.
 */
[CreateAssetMenu(fileName = "New_Ability", menuName = "Scripts/Ability", order = 1)]
public class Ability : ScriptableObject
{
    [SerializeField] public Sprite abilityIcon; // icon for ability

    [SerializeField] public string abilityName; // name of the ability
    [SerializeField] public string abilityDescription; // description of ability
    // [SerializeField] public string abilityType; // type of ability (currently TBD)

    [SerializeField] public float cooldown; // ability cooldown in seconds
    [SerializeField] public float damage; // ability damage to enemies
    [SerializeField] public int cost; // how much ability costs resource-wise, ints for now

    // ability range depends on the ability
    // audio should be handled by SoundManager (?)
    // You can also add methods here
    public virtual void Activate(GameObject parent) { } // override this method for custom ability activation

}

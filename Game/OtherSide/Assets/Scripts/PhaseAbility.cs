using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PhaseAbility", menuName = "Scripts/PhaseAbility", order = 3)]
public class PhaseAbility : Ability
{

    private SpriteRenderer objectRenderer;
    private Color original;
    public override void Activate(GameObject parent)
    {
        Debug.Log("Phase Ability activated");
        objectRenderer = parent.GetComponent<SpriteRenderer>();
        original = objectRenderer.color;
        objectRenderer.color = new Color(1f, 1f, 1f, .1f);
    }

    /* Deactivate is called when the skill is cooldown
    * destroy the melee object
    */
    public override void Deactivate(GameObject parent) // 
    {
        Debug.Log("Melee Ability deactivated");
        objectRenderer.color = original;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holder : MonoBehaviour
{
    [SerializeField] public Ability ability; // add an ability object in editor
    [SerializeField] public float cdCoefficient;
    internal float cooldownTime; // this should be inherited from Ability
    internal float activeTime; // to keep track of how long ability lasts, can use Time.delta possibly
    //[SerializeField] public KeyCode key; // assign button in editor
    public enum AbilityState // an ability is either ready, being activated, or on cooldown
    {
        ready,
        active,
        cooldown
    }
    public AbilityState state = AbilityState.ready; // default ability state is ready

}

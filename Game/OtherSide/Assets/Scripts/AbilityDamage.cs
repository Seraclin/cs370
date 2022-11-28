using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Damage logic for the ability hitting enemy or player.
 * TODO: needs a lot of work, needs to inherit attributes from Ability object, have Ability owner
 */
public class AbilityDamage : MonoBehaviour
{
    internal int damage; // TODO: replace with Ability object's damage
    // [SerializeField] float duration = 5; // duration of ability TODO: replace with Ability object's duration 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.tag != collision.gameObject.tag + "Ability")
        {


            if (collision.gameObject.tag == "Enemy")
            {
                Enemy eScript = collision.gameObject.GetComponent<Enemy>();
                if (!eScript.isDead && gameObject.transform.parent.gameObject.GetComponent<AbilityArray>().holderArray[1].ability.isPassive)
                {
                    gameObject.transform.parent.gameObject.GetComponent<AbilityArray>().holderArray[1].ability.Activate(gameObject.transform.parent.gameObject);
                }
                eScript.ChangeHealth(0 - damage);
                Destroy(gameObject, 0.2f);

            }
            else if (collision.gameObject.tag == "Wall")
            {
                Destroy(gameObject);
            }
            else if (collision.gameObject.tag == "Player")
            {
                // Debug.Log("Player hit, health reduce by " + damage);
                PlayerHealth eScript = collision.gameObject.GetComponent<PlayerHealth>();
                eScript.ChangeHealth(0 - damage);
                if (gameObject.transform.parent.gameObject.GetComponent<AbilityArray>().holderArray[1].ability.isPassive)
                {
                    gameObject.transform.parent.gameObject.GetComponent<AbilityArray>().holderArray[1].ability.Activate(gameObject.transform.parent.gameObject);
                }
   
                Destroy(gameObject, 0.2f);
            }

        }


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
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
                Destroy(gameObject);
                Debug.Log("Damage Enemy");
            } else if (collision.gameObject.tag == "Environment")
            {
                Destroy(gameObject);
                Debug.Log("Hit wall");
            }
        }
        
    }
}

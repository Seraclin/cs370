using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public bool isPoisoned, isSlowed;
    internal float poiDuration, slowDuration;
    internal int poiDamage;
    internal int poiCount;
    internal float speedCoef;
    private float oriSpeed;
    // private eScript obj;

    // Start is called before the first frame update
    void Start()
    {
        isPoisoned = false;
        isSlowed = false;
        poiDuration = 0;
        slowDuration = 0;
        poiCount = 0;
        if (gameObject.tag == "Enemy")
        {
            // obj = gameObject.GetComponent<Enemy>();
            oriSpeed = gameObject.GetComponent<Enemy>().speed;
        } else if (gameObject.tag == "Player")
        {
            // obj = gameObject.GetComponent<PlayerController>();
            oriSpeed = gameObject.GetComponent<PlayerController>().speed;
        }
    }

    void FixedUpdate()
    {
        if (isPoisoned)
        {
            if (poiDuration > 0)
            {
                poiDuration -= Time.deltaTime;
                if (poiDuration <= poiCount)
                {
                    if (gameObject.tag == "Enemy")
                    {
                        Enemy eScript = gameObject.GetComponent<Enemy>();
                        eScript.ChangeHealth(0 - poiDamage);

                    }
                    else if (gameObject.tag == "Player")
                    {
                        // Debug.Log("Player hit, health reduce by " + damage);
                        PlayerHealth eScript = gameObject.GetComponent<PlayerHealth>();
                        
                        eScript.ChangeHealth(0 - poiDamage);
                    }
                    poiCount--;
                }
            } else
            {
                isPoisoned = false;
                poiCount = 0;
            }
        }

        if (isSlowed)
        {
            if (slowDuration > 0)
            {  
                if (gameObject.tag == "Enemy")
                {
                    // obj = gameObject.GetComponent<Enemy>();
                    if (gameObject.GetComponent<Enemy>().speed != oriSpeed)
                    {
                        gameObject.GetComponent<Enemy>().speed *= speedCoef;
                    }
                    
                }    
                else if (gameObject.tag == "Player")
                {
                    // obj = gameObject.GetComponent<PlayerController>();
                    if (gameObject.GetComponent<PlayerController>().speed != oriSpeed)
                    {
                        gameObject.GetComponent<PlayerController>().speed *= speedCoef;
                    }
                }
                slowDuration -= Time.deltaTime;
            } else
            {
                isSlowed = false;
                if (gameObject.tag == "Enemy")
                {
                    // obj = gameObject.GetComponent<Enemy>();
                    gameObject.GetComponent<Enemy>().speed = oriSpeed;
                }
                else if (gameObject.tag == "Player")
                {
                    // obj = gameObject.GetComponent<PlayerController>();
                    gameObject.GetComponent<PlayerController>().speed = oriSpeed;
                }
            }
        }
    }


}

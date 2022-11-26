using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public bool isPoisoned, isSlowed;
    internal float poiDuration, sloDuration;
    internal int poiDamage;
    internal int poiCount;

    // Start is called before the first frame update
    void Start()
    {
        isPoisoned = false;
        isSlowed = false;
        poiDuration = 0;
        sloDuration = 0;
        poiCount = 0;
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
    }


}

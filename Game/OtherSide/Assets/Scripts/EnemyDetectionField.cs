using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionField : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player detected");
            gameObject.transform.parent.GetComponent<Enemy>().player = collision.gameObject;

        }

    }
}

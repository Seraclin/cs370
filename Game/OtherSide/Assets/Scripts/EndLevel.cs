using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    public GameObject EndLevelUI;
    void OnTriggerEnter2D(Collider2D end)
    {
        if(end.gameObject.tag == "Player")
        {
            Debug.Log("Level Complete");
            EndLevelUI.SetActive(true);
        }
    }
}

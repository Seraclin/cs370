using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpwaner : MonoBehaviour
{
    public Transform[] spwanPoints; 
    public GameObject[] enemyPrefabs; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Instantiate(enemyPrefabs[0], spwanPoints[0].position, transform.rotation); 
        }
    }
}

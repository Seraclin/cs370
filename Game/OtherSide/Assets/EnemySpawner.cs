using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class EnemySpawner : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public void Start()
    {
       
        for (int i = 0; i<transform.childCount; i++)
        {
            
       
            GameObject child = transform.GetChild(i).gameObject;
            Debug.Log(child.name);
            GameObject newEnemy = PhotonNetwork.Instantiate(child.name, child.transform.position, Quaternion.identity, 0);
            if (PhotonNetwork.IsMasterClient)
            {
                newEnemy.transform.parent = this.transform;
                newEnemy.name = newEnemy.name.Substring(0, newEnemy.name.Length - 7);
            }

            Destroy(child);

        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemy(Vector3 pos, GameObject type)
    {
        Debug.Log("IsSpawning");

    }
}

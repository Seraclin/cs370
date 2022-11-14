using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject GameCanvas;
    public GameObject SceneCamera;
    // Start is called before the first frame update
    void Awake()
    {
        GameCanvas.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnPlayer()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0,0,0),Quaternion.identity,0);
        GameCanvas.SetActive(false);
        SceneCamera.SetActive(false);
    }
}

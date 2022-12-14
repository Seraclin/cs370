using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public GameObject GameCanvas;
    public GameObject SceneCamera;
    public Text LobbyText;
    [SerializeField] PhotonView pv;
    Vector3 position; //spawn player on game Manager Obj
    // Start is called before the first frame update
    void Awake()
    {
        GameCanvas.SetActive(true);
        LobbyText.text = "";
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            LobbyText.text += player.NickName + "\n";
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer){
        Debug.Log(newPlayer.NickName + "Has Entered Room");
        LobbyText.text = "";
        foreach (Player player in PhotonNetwork.PlayerList) {
            LobbyText.text += player.NickName + "\n";
        }
        }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnPlayers()
    {
        pv.RPC("SpawnPlayer", RpcTarget.All);
    }
    [PunRPC] public void SpawnPlayer()
    {
        position = transform.position;
        PhotonNetwork.Instantiate(playerPrefab.name, position,Quaternion.identity,0);
        GameCanvas.SetActive(false);
        SceneCamera.SetActive(false);
        PhotonNetwork.CurrentRoom.IsOpen = false;
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class MenuController : MonoBehaviourPunCallbacks
{
    [SerializeField] private string versionName = "0.1";
    [SerializeField] private GameObject usernameMenu;
    [SerializeField] private GameObject onlineMenu;
    [SerializeField] private GameObject connectPanel;

    [SerializeField] private InputField usernameInput;
    [SerializeField] private InputField createGameInput;
    [SerializeField] private InputField joinGameInput;

    [SerializeField] private GameObject startBtn;
    // Start is called before the first frame update
    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    private void Start()
    {
        onlineMenu.SetActive(false);
    }

    public void MultiplayerSelected()
    {
        onlineMenu.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("Connected");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeUsernameInput()
    {
        if (usernameInput.text.Length < 3 || usernameInput.text.Length > 10)
        {
            startBtn.SetActive(false);

        }
        else
        {
            startBtn.SetActive(true);
        }
    }

    public void SetUsername()
    {
        usernameMenu.SetActive(false);
        PhotonNetwork.NickName = usernameInput.text;
    }
}

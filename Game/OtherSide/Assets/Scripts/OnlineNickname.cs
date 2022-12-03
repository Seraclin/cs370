using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Photon.Pun;
using Photon.Realtime;

public class OnlineNickname : MonoBehaviourPunCallbacks
{
    public PhotonView pv;
    public Text txt;
    // Start is called before the first frame update
    void Start()
    {
        if(pv.IsMine) {
            txt.text = PhotonNetwork.NickName;
        }
        else
        {
            txt.text = pv.Owner.NickName;
        }
            
        
     


    }

 
}

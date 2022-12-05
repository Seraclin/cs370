using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;

public class MapRuntimeGenerator : MonoBehaviour
{
    public UnityEvent OnStart;
    public PhotonView pv;
    // Start is called before the first frame update
    void Start()
    {

        if (PhotonNetwork.IsMasterClient)
        OnStart?.Invoke();
    }

}

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

    [SerializeField] GameObject Generator1;
    
    [SerializeField] GameObject Generator3;
    [SerializeField] GameObject Generator4;
    [SerializeField] GameObject Generator5;
    [SerializeField] GameObject Generator6;
    [SerializeField] GameObject Generator7;
    [SerializeField] GameObject Generator8;
    [SerializeField] GameObject Generator9;
    // Start is called before the first frame update
    void Start()
    {

       
        
    }
    private void Awake()
    {
        pv.RPC("RunEverything", RpcTarget.OthersBuffered);

        
        
        

    }

    [PunRPC]
    void RunEverything()
    {
        OnStart?.Invoke();
    }
}

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
        if (PhotonNetwork.IsMasterClient)
        {
            OnStart?.Invoke();
        }
        if (!PhotonNetwork.IsMasterClient)
        {
            Generator1.SetActive(false);
           
            Generator3.SetActive(false);
            Generator4.SetActive(false);
            Generator5.SetActive(false);
            Generator6.SetActive(false);
            Generator7.SetActive(false);
            Generator8.SetActive(false);
            Generator9.SetActive(false);


        }

    }
}

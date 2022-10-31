using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animatortest : MonoBehaviour
{

    Animator anim;
    [SerializeField] float speed;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetFloat("Speed", speed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

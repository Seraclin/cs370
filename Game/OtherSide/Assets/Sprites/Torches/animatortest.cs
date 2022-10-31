using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animatortest : MonoBehaviour
{

    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetFloat("Speed", 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

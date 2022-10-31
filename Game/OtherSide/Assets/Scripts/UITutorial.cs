using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITutorial : MonoBehaviour
{
    public GameObject uiObject;
    [SerializeField] public string textui;
    private string text;
    


    // Start is called before the first frame update
    void Start()
    {
        //text = uiObject.GetComponent<TMPro.TextMeshProUGUI>().text;
        
    }
    void OnTriggerEnter2D (Collider2D player)
    {
        if (player.gameObject.tag == "Player")
        {
            Debug.Log("Entered");
            uiObject.GetComponent<TMPro.TextMeshProUGUI>().text = textui;
            uiObject.SetActive(true);
        }
    }
    void OnTriggerExit2D (Collider2D player)
    {
        if (player.gameObject.tag == "Player")
        {
            uiObject.SetActive(false);
        }
    }

}

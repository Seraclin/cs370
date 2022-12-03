using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UITutorial_typewriter : MonoBehaviour
{
    // public GameObject uiObject;
    // private string text;

    /* Similar to UITutorial, but prints with a typewriter effect
     * Attach script to Gameobject that has Trigger
     * Used for displaying Tutorial information - Sam
     * Adapted from: https://github.com/rioter00/UnityExamples/blob/master/typewriterUI.cs
     * Want to get creative? Try a Unicode leading character(https://unicode-table.com/en/blocks/block-elements/)
     */

    [SerializeField] public string textui; // text to be displayed
    [SerializeField] public GameObject uiObject; // Gameobject that has TextMeshPro component

    [SerializeField] float delayBeforeStart = 0f; // when to start typing text, set to 0f for immediate typing
    [SerializeField] float timeBtwChars = 0.04f; // time it takes to type each char
    [SerializeField] string leadingChar = ""; // your cursor
    [SerializeField] bool leadingCharBeforeDelay = false;

    // Text _text; // don't use Text, use TextMeshPro
    TMP_Text _tmpProText;
    string writer;

    // typewrite() is called when object is set active
    // private bool inText = false; // to track if user is still in trigger field
    void typewrite()
    {
        // _tmpProText = uiObject.GetComponent<TMP_Text>()!;
        
        if (_tmpProText != null)
        {
            writer = _tmpProText.text;
            // _tmpProText.text = "";

            StartCoroutine("TypeWriterTMP");
        }
    }
    IEnumerator TypeWriterTMP()
    {
        _tmpProText.text = leadingCharBeforeDelay ? leadingChar : "";

        yield return new WaitForSeconds(delayBeforeStart);

        foreach (char c in writer)
        {
            if (_tmpProText.text.Length > 0)
            {
                _tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
            }
            _tmpProText.text += c;
            _tmpProText.text += leadingChar;
            yield return new WaitForSeconds(timeBtwChars);
        }

        if (leadingChar != "")
        {
            _tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
        }
    }

    void OnTriggerEnter2D(Collider2D player)
    {
        if (player.gameObject.tag == "Player")
        {
            // Debug.Log("Player has entered text field for Tutorial");
            _tmpProText = uiObject.GetComponent<TMP_Text>();
            _tmpProText.text = textui;
            uiObject.SetActive(true);
            typewrite();
        }
    }
    void OnTriggerExit2D(Collider2D player)
    {
        if (player.gameObject.tag == "Player")
        {
            // Debug.Log("Player has left text field for Tutorial");
            StopCoroutine("TypeWriterTMP");
            uiObject.SetActive(false);
        }
    }

}

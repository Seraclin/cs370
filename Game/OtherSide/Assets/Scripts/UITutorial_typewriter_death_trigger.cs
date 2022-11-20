using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UITutorial_typewriter_death_trigger : MonoBehaviour
{
    // public GameObject uiObject;
    // private string text;

    /* Similar to UITutorial, but prints with a typewriter effect. This prints two different things depending if the condition is met or not.
     * Attach script to Gameobject that has Trigger
     * Used for displaying Tutorial information - Sam
     * Adapted from: https://github.com/rioter00/UnityExamples/blob/master/typewriterUI.cs
     * Want to get creative? Try a Unicode leading character(https://unicode-table.com/en/blocks/block-elements/)
     */

    [SerializeField] public string[] textui = new string[2]; // text to be displayed, intially displays [0] then [1] if the condition is met
    [SerializeField] public GameObject enemy; // Enemy refernce for condition

    [SerializeField] public GameObject uiObject; // Gameobject that has TextMeshPro component

    [SerializeField] float delayBeforeStart = 0f; // when to start typing text, set to 0f for immediate typing
    [SerializeField] float timeBtwChars = 0.06f; // time it takes to type each char
    [SerializeField] string leadingChar = ""; // your cursor
    [SerializeField] bool leadingCharBeforeDelay = false;

    [SerializeField] float duration = 20f; // how long text last after trigger


    // Text _text; // don't use Text, use TextMeshPro
    TMP_Text _tmpProText;
    string writer;

    private bool nextPrompt = false;
    private int count = 0;

    void Start()
    {

    }
    void Update()
    {
        if (!nextPrompt && enemy.GetComponent<Enemy>().isPossessable == true)
        {
            nextPrompt = true;
        }
    }
    // typewrite() is called when object is set active
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
        // yield return new WaitForSeconds(duration - (timeBtwChars  * _tmpProText.text.Length));
    }
    void OnTriggerEnter2D(Collider2D player)
    {
        if (player.gameObject.tag == "Player")
        {
            // Debug.Log("Player has entered text field for Tutorial");
            _tmpProText = uiObject.GetComponent<TMP_Text>();
            if (nextPrompt) // condition has been met
            {
                _tmpProText.text = textui[1];

            }
            else
            {
                _tmpProText.text = textui[0];
            }

            uiObject.SetActive(true);
            typewrite();
        }
    }
    void OnTriggerStay2D(Collider2D player)
    {
        if (player.gameObject.tag == "Player" && nextPrompt && count == 0) // reprint instructions for when player changing conditonal state while still in colllider
        {
            // Debug.LogWarning("changing print statement to match condition");
            StopCoroutine("TypeWriterTMP");

            // Change text when condition is met
            _tmpProText = uiObject.GetComponent<TMP_Text>();
            _tmpProText.text = textui[1];
            uiObject.SetActive(true);
            typewrite();
            count = 1; // stop infinite loop of typing
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TryAgain : MonoBehaviour
{
   public void RestartGame()
    {
        // SceneManager.LoadScene("MainSinglePlayer");
        // Unhard-coded try again -Sam
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void startSinglePlayer()
    {
        SceneManager.LoadScene("MainSinglePlayer");
    }
    public void returntoStart()
    {
        AudioManager theme = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        // TODO: need to fix bgm music to not play when at main menu
        SceneManager.LoadScene("StartingScene");
    }
    public void startTutorial()
    { // start tutorial button
        SceneManager.LoadScene("Tutorial");
    }


}

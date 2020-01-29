using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// This script manages the game play GUI
/// </summary>

public class GUIManager : MonoBehaviour {

    //ref to the score
    public Text inGameScoreText;


	// Use this for initialization

	// Update is called once per frame
	void Update ()
    {
        //we check for the game manager
        if (GameManager.singleton != null)
        {
            //and keep updating score value
            inGameScoreText.text = GameManager.singleton.currentScore.ToString();
        }

        
	}

    //ref method to retry button
    public void RetryButton()
    {
        //Application.LoadLevel("GamePlay"); // if you are using unity below 5.3 version
        //when player press retry button the game play scene is loaded and the game over bool is made false
        SceneManager.LoadScene("GamePlay");//use this for 5.3 version
        //we make it false because  we need to play the game again
        GameManager.singleton.isGameOver = false;
    }




}

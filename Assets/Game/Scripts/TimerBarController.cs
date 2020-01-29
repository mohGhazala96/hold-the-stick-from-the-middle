using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script manages our time for each question
/// </summary>

public class TimerBarController : MonoBehaviour {

    public static TimerBarController instance;

    //ref to the images which will display the time in fill type
    public Transform fillBar;
    //ref to the fill amount or bar 
    [HideInInspector]public float currentAmount;
    //ref to the time
    private float timeT;
	private bool reset = false;

	// Use this for initialization
	
    public void init() {
        timeT = GameManager.singleton.timeForQuestion;

        GameManager.singleton.isGameOver = false;
        reset = true;
        currentAmount = 1;
    }

    void Awake()
    {
        MakeInstance();
    }

    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    void Update()
    {
        //we reduces the time when quesition is asked with respect to game time
        currentAmount  -= (timeT) * Time.deltaTime;

        fillBar.GetComponent<Image>().fillAmount = currentAmount;

		if ((currentAmount <= 0&& reset) ||  GameManager.singleton.currentScore == 3)
        {
            //if the fill become zero , means the time is over we declare game over
            GameManager.singleton.isGameOver = true;
			
			GameManager.singleton.gameOver ();
			reset = false;
            print("aaaaaaaa");
            if(MathsAndAnswerScript.canPlay == true)
            {
                GameController.singleton.questionIndex++;
                GameController.singleton.ShowQuestion();
            }
            MathsAndAnswerScript.canPlay = false;
        }

    }

}

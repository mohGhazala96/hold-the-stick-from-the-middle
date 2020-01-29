using UnityEngine;
using System.Collections;
using System.IO; // this is required for input and output of data
using System;
using System.Runtime.Serialization.Formatters.Binary;//this is required to convert data into binary

public class GameManager : MonoBehaviour {

    //we make static so in games only one script is name as this
    public static GameManager singleton;

    //variable of gamedata
	public Canvas mathsCanvas;
    //data not to store on device
    public float timeForQuestion;
    public int currentScore;
    public bool isGameOver;

    public int currentMode;
	public GameObject mathObject;
    public GameObject timeBar;
    //data to store on device
    public int hiScore;
    public bool isGameStartedFirstTime;

	IEnumerator changeObjectVisibility (Canvas o, bool visible)
	{
		CanvasGroup canvasGroup = o.GetComponent<CanvasGroup> ();
		float visibleValue;
		if (visible) {
			visibleValue = -0.1f;
			canvasGroup.interactable = false;

		} else {
			o.enabled = true;

			visibleValue = 0.1f;

		}

		float time = Time.time + 1;
		float counter = Time.time;

		while (counter <= time) {
			canvasGroup.alpha += visibleValue;
			if (canvasGroup.alpha == 0 || canvasGroup.alpha == 1) {
				break;
			}
			counter += Time.deltaTime;
			yield return null;

		}
		if (!visible) {
			canvasGroup.interactable = true;
		} else
			o.enabled = false;


		yield return null;
	}


    //it is call only once in a scene
    void Start()
    {
        MakeSingleton();
    }
	public void init(){
		currentScore = 0;
		StartCoroutine (changeObjectVisibility( mathsCanvas.GetComponent<Canvas>(),false));
		mathObject.GetComponent<MathsAndAnswerScript> ().init ();
        timeBar.GetComponent<TimerBarController>().init();

	}
	public void gameOver(){
		StartCoroutine (changeObjectVisibility( mathsCanvas.GetComponent<Canvas>(),true));
    }
    void MakeSingleton()
    {
        if (singleton != null)
        {
            Destroy(gameObject);
        }
        else
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);

        }
    }


}


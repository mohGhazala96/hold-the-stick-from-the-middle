using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

using ArabicSupport;
using UnityEngine.UI.Extensions;

public class GameController : MonoBehaviour
{
    public static GameController singleton;

    public SimpleObjectPool answerButtonObjectPool;
    public Text questionText;
    public Text scoreDisplay;
    public GameObject starPlot;
    public Text timeRemainingDisplay;
    public Transform answerButtonParent;

    public GameObject questionDisplay;
    public GameObject roundEndDisplay;
    public Text highScoreDisplay;

    private DataController dataController;
    private RoundData currentRoundData;
    private QuestionData[] questionPool;

    private bool isRoundActive = false;
    private float timeRemaining;
    private int playerScore;
    public int questionIndex;
    private List<GameObject> answerButtonGameObjects = new List<GameObject>();
    private UIPolygon starPolygon;

    public GameObject MathCanvas;
    public GameObject MathObject;
    
    void Start()
    {
        MakeSingleton();
        dataController = FindObjectOfType<DataController>();                              // Store a reference to the DataController so we can request the data we need for this round
        currentRoundData = dataController.GetCurrentRoundData();                            // Ask the DataController for the data for the current round. At the moment, we only have one round - but we could extend this
        questionPool = currentRoundData.questions;                                          // Take a copy of the questions so we could shuffle the pool or drop questions from it without affecting the original RoundData object

        starPolygon = starPlot.GetComponent<UIPolygon>();
        timeRemaining = currentRoundData.timeLimitInSeconds;                                // Set the time limit for this round based on the RoundData object
        UpdateTimeRemainingDisplay();
        playerScore = 0;
        questionIndex = 0;
        print("here");
        ShowQuestion();
        isRoundActive = true;
    }

    void Update()
    {
        if (isRoundActive)
        {
            //timeRemaining -= Time.deltaTime;                                                // If the round is active, subtract the time since Update() was last called from timeRemaining
            UpdateTimeRemainingDisplay();

            if (timeRemaining <= 0f)                                                     // If timeRemaining is 0 or less, the round ends
            {
                EndRound();
            }
        }
    }

    IEnumerator changeObjectVisibility(GameObject o, bool visible)
    {
        CanvasGroup canvasGroup = o.GetComponent<CanvasGroup>();
        float visibleValue;
        if (visible)
        {
            visibleValue = -0.1f;
            canvasGroup.interactable = false;

        }
        else
        {
            o.SetActive(true);

            visibleValue = 0.1f;

        }

        float time = Time.time + 1;
        float counter = Time.time;

        while (counter <= time)
        {
            canvasGroup.alpha += visibleValue;
            if (canvasGroup.alpha == 0 || canvasGroup.alpha == 1)
            {
                break;
            }
            counter += Time.deltaTime;
            yield return null;

        }
        if (!visible)
        {
            canvasGroup.interactable = true;
        }
        else
            o.SetActive(false);


        yield return null;
    }

    public void ShowQuestion()
    {
        RemoveAnswerButtons();
        

        QuestionData questionData = questionPool[questionIndex];                            // Get the QuestionData for the current question
        questionText.text = ArabicFixer.Fix(questionData.title);                                      // Update questionText with the correct text
        Debug.Log(questionData.title);

        if (questionData.type == "miniGame") 
        {
            questionText.text = ArabicFixer.Fix(questionData.title);
            RemoveAnswerButtons();
            StartCoroutine(changeObjectVisibility(MathCanvas, false));
            MathObject.GetComponent<GameManager>().init();
               
        }
        else
        {
            Debug.Log(questionData.answers.Length);
            for (int i = 0; i < questionData.answers.Length; i++)                               // For every AnswerData in the current QuestionData...
            {
                GameObject answerButtonGameObject = answerButtonObjectPool.GetObject();         // Spawn an AnswerButton from the object pool
                answerButtonGameObjects.Add(answerButtonGameObject);
                answerButtonGameObject.transform.SetParent(answerButtonParent);
                answerButtonGameObject.transform.localScale = Vector3.one;
                answerButtonGameObject.transform.position.Set(answerButtonGameObject.transform.position.x, answerButtonGameObject.transform.position.y, 0);

                AnswerButton answerButton = answerButtonGameObject.GetComponent<AnswerButton>();

                answerButton.SetUp(questionData.answers[i]);                                    // Pass the AnswerData to the AnswerButton so the AnswerButton knows what text to display and whether it is the correct answer
            }
        }
        
    }

    void RemoveAnswerButtons()
    {
        while (answerButtonGameObjects.Count > 0)                                            // Return all spawned AnswerButtons to the object pool
        {
            answerButtonObjectPool.ReturnObject(answerButtonGameObjects[0]);
            answerButtonGameObjects.RemoveAt(0);
        }
    }
    public void AnswerButtonClicked(AnswerData data)
    {
        starPolygon.VerticesDistances[0] += (data.effects[0] / 40f);
        starPolygon.VerticesDistances[1] += (data.effects[1] / 40f);
        starPolygon.VerticesDistances[2] += (data.effects[2] / 40f);
        starPolygon.VerticesDistances[3] += (data.effects[3] / 40f);
        starPolygon.VerticesDistances[4] += (data.effects[4] / 40f);
        
        
        print("shrug");

        starPolygon.Redraw();

        if (questionPool.Length > questionIndex + 1)                                          // If there are more questions, show the next question
        {
            questionIndex++;
            ShowQuestion();
        }
        else                                                                                // If there are no more questions, the round ends
        {
            // EndRound();
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");


        }
    }

    private void UpdateTimeRemainingDisplay()
    {
        timeRemainingDisplay.text = Mathf.Round(timeRemaining).ToString();
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

    public void EndRound()
    {
        isRoundActive = false;

        dataController.SubmitNewPlayerScore(playerScore);
        highScoreDisplay.text = dataController.GetHighestPlayerScore().ToString();

        questionDisplay.SetActive(false);
        roundEndDisplay.SetActive(true);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MenuScreen");
    }
}
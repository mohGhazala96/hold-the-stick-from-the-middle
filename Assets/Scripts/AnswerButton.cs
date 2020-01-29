using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ArabicSupport;

public class AnswerButton : MonoBehaviour 
{
	public Text answerText;

	private GameController gameController;
	private AnswerData answerData;

	void Start()
	{
		gameController = FindObjectOfType<GameController>();
	}

	public void SetUp(AnswerData data)
	{
		answerData = data;
		answerText.text = ArabicFixer.Fix(answerData.text);
    }

    public void HandleClick()
	{
		gameController.AnswerButtonClicked(answerData);
	}
}

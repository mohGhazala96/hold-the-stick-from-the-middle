using UnityEngine;

public class MenuScreenController : MonoBehaviour
{
	private  static bool AudioBegin = false; 

	void Awake()
	{
		
	}


	public void StartGame()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
	}
}
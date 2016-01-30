using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMeta : MonoBehaviour
{
	[SerializeField] private ScoreController _scoreController;

	// Use this for initialization
	void Start () {
	
	}

	//TODO: timer
	//show instructions?
	//during a game it should be the same
	
	//score?
	//post game scene?

	[ContextMenu("Test Reload game")]
	void ReloadGame()
	{
		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			SceneManager.UnloadScene(i);
		}

		SceneManager.LoadScene(0);
	}

}

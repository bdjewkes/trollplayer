using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMeta : MonoBehaviour
{
	[SerializeField] private int _score;
	[SerializeField] private ScoreController _scoreController;

	[SerializeField] private TextMeshPro _timer;

	[SerializeField] private readonly float _totalTimeInSession = 30f;

	// Use this for initialization
	private void Start()
	{
		StartCoroutine(countDownTime());
	}

	//TODO: timer
	//show instructions?
	//during a game it should be the same

	//score?
	//post game scene?

	private IEnumerator countDownTime()
	{
		var startTime = Time.time;
		var startTimeInMilliseconds = Time.time*1000f;
		while (Time.time < startTimeInMilliseconds + _totalTimeInSession)
		{
			_timer.SetText(string.Format("{0:0.00}", Time.time - startTime));
			yield return null;
		}
		//TODO: coroutine to celebrate ending
		ReloadGame();
	}

	[ContextMenu("Test Reload game")]
	private void ReloadGame()
	{
		for (var i = 0; i < SceneManager.sceneCount; i++)
		{
			SceneManager.UnloadScene(i);
		}

		SceneManager.LoadScene(0);
	}
}
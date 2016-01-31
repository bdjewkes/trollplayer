using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMeta : MonoBehaviour
{
	private int _score = 0;
	[SerializeField] private ScoreController _scoreController;

	[SerializeField] private TextMeshPro _timer;

	[SerializeField] private readonly float _totalTimeInSession = 90f;

	// Use this for initialization
	private void Start()
	{
		_scoreController.SetScore(_score);
		StartCoroutine(countDownTime());
	}

	//show instructions?
	//during a game it should be the same

	//TODO: post game scene?
	[ContextMenu("Add to score")]
	public void AddToScore()
	{
		_score++;
		_scoreController.SetScore(_score);
	}

	private IEnumerator countDownTime()
	{
		var startTime = Time.time;
		var startTimeInMilliseconds = Time.time*1000f;
		while (Time.time < startTimeInMilliseconds + _totalTimeInSession)
		{
			_timer.SetText(string.Format("{0:0.00}s", _totalTimeInSession - (Time.time - startTime)));
			yield return null;
		}
		//TODO: celebrate ending ( coroutine? animation?)
		ReloadGame();
	}

	[ContextMenu("Test Reload game")]
	public void ReloadGame()
	{
		for (var i = 0; i < SceneManager.sceneCount; i++)
		{
			SceneManager.UnloadScene(i);
		}

		SceneManager.LoadScene(0);
	}
}
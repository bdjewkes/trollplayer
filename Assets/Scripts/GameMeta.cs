using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMeta : MonoBehaviour
{
	private int _score = 0;
	[SerializeField] private ScoreController _scoreController;

	[SerializeField] private TextMeshPro _timer;

	[SerializeField] private readonly float _totalTimeInSession = 120f;

	// Use this for initialization
	private void Start()
	{
		_scoreController.SetScore(0);
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
		var timeLeft = _totalTimeInSession;
		while (timeLeft > 0)
		{
			_timer.SetText(string.Format("{0:0.00}s", timeLeft));
			yield return null;

			timeLeft -= Time.deltaTime;
		}
		Debug.Log("timer done, ending game");
		//TODO: celebrate ending ( coroutine? animation?)
		FindObjectOfType<AdditiveLoad>().ReloadGame();
	}

	
	//TAP TO CONTINUE
}
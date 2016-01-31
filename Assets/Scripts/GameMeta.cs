using System.Collections;
using TMPro;
using UnityEngine;

public class GameMeta : MonoBehaviour
{
	[SerializeField] private readonly float _totalTimeInSession = 120f;
	private int _score;
	[SerializeField] private ScoreController _scoreController;

	[SerializeField] private TextMeshPro _timer;
	public bool gameIsDone;

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
		gameIsDone = true;
		//FindObjectOfType<AdditiveLoad>().ReloadGame();
	}


	//TAP TO CONTINUE
}
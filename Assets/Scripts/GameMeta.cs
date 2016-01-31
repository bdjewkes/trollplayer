using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameMeta : MonoBehaviour
{
	[Serializable]
	public class RoundDescription
	{
		public int targetGold;
		public float RoundTime;
	}

	private RoundDescription[] _roundsDescriptions = null;
	[SerializeField] private float _totalTimeInSession = 120f;
	private int _score;
	[SerializeField] private ScoreController _scoreController;

	[SerializeField] private TextMeshPro _timer;
	public bool gameIsDone;

	// Use this for initialization
	private void Start()
	{
		if(_roundsDescriptions == null)
		{
			_roundsDescriptions = new RoundDescription[]
			{
				new RoundDescription() { targetGold = 1,RoundTime = 90f},
				new RoundDescription() { targetGold = 3,RoundTime = 60f},
				new RoundDescription() { targetGold = 5,RoundTime = 30f },
			};
        }
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

	public void RoundFinishedDueToFailure()
	{
		_roundFailed = true;
	}

	private bool _roundFailed = false;
	private IEnumerator countDownTime()
	{
		foreach (var roundDescription in _roundsDescriptions)
		{
			yield return StartCoroutine(handleRound(roundDescription));
		}

		//TODO: celebrate ending ( coroutine? animation?)
		gameIsDone = true;
		//FindObjectOfType<AdditiveLoad>().ReloadGame();
	}

	private IEnumerator handleRound(RoundDescription round)
	{
		_roundFailed = false;
		var timeLeft = round.RoundTime;
		while(timeLeft > 0 && !_roundFailed)
		{
			_timer.SetText(string.Format("{0:0.00}s", timeLeft));
			yield return null;

			timeLeft -= Time.deltaTime;
		}
		if(timeLeft <= 0)
			Debug.Log("timer done, ending round");
		if(_roundFailed)
			Debug.Log("round failed, ending round");
	}





	//TAP TO CONTINUE
}
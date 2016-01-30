﻿using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ScoreController : MonoBehaviour
{
	private int _score;
	[SerializeField] private NumberScore _numberScore;

	[SerializeField] private GoldScore _goldScore;
	public void ResetScore()
	{
		SetScore(0);
	}

	public void SetScore(int score)
	{
		_score = 0;
		_numberScore.SetScore(0);
		_goldScore.SetScore(0);
	}
}

public class NumberScore
{
	[SerializeField] TextMeshPro text;


	public void SetScore(int score)
	{
		text.SetText(score.ToString());
	}
}

public class GoldScore
{
	[SerializeField] List<GameObject> goldGameObjects;

	public void SetScore(int newScore)
	{
		if(newScore >= goldGameObjects.Count)
		{
			Debug.LogError("Score is more than is supproted by gold");
		}
		for (int i = 0;  i < goldGameObjects.Count; i++)
		{
			bool shouldBeActive = (i < newScore);
            goldGameObjects[i].SetActive(shouldBeActive);
		}
	}
}

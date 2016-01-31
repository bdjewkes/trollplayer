using System;
using System.Collections;
using SpryFox.Common;
using UnityEngine;

public class StationSubmit : Station
{
	protected override void Awake()
	{
		base.Awake();
		//ShowFailureFX(false);
	}

	public override IEnumerator PerformAction(Substance substance)
	{
		yield return new WaitForSeconds(timeToReact);
		var successfulReaction = substance.State == constant;

		if(successfulReaction)
		{
			Jot.Out("Submission successful!");
			successEnding.objectToEnableWhenStatusHappens.SetActive(true);
		} else
		{
			failureEnding.timesEndingtimeEncounteredBeforeResetting--;
			failureEnding.objectToEnableWhenStatusHappens.SetActive(true);
		}
			

		yield return new WaitForSeconds(timeToPlayFX);

		if(!successfulReaction)
		{
			failureEnding.objectToEnableWhenStatusHappens.SetActive(false);
			
		}
		if(failureEnding.timesEndingtimeEncounteredBeforeResetting <= 0)
		{
			//change the state of the reaction vessel back to zero?
			//reset game?

			FindObjectOfType<GameMeta>().gameIsDone = true;
		}
		if(successfulReaction)
		{
			FindObjectOfType<GameMeta>().AddToScore();
			substance.ResetState();
		}
		ShowReactionFX(false);
	}

	//public GameObject[] FailureFX;

	#region ending logic

	[SerializeField] private EndingSequence successEnding;
	[SerializeField] private EndingSequence failureEnding;

	[Serializable]
	public class EndingSequence
	{
		public GameObject objectToEnableWhenStatusHappens;
		//public Particle successParticle;
		//public AudioSource soundToPlay;
		public int timesEndingtimeEncounteredBeforeResetting = 1;
	}

	#endregion ending logic
}
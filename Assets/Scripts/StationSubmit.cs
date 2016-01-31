using System;
using System.Collections;
using SpryFox.Common;
using UnityEngine;

public class StationSubmit : Station
{
    public GameObject goldPrefab;
    public Vector3 goldForce = new Vector3(0, 100, -1);

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
			//successEnding.objectToEnableWhenStatusHappens.SetActive(true);
            var gold = (GameObject)Instantiate(goldPrefab, transform.position, UnityEngine.Random.rotationUniform);
            gold.GetComponent<Rigidbody>().AddForce(goldForce, ForceMode.VelocityChange);

            var spawned = (GameObject)Instantiate(successEnding.objectToSpawnWhenStatusHappens, transform.position, Quaternion.identity);
            Destroy(spawned, 1f);
		} else
		{
			failureEnding.timesEndingtimeEncounteredBeforeResetting--;
			//failureEnding.objectToSpawnWhenStatusHappens.SetActive(true);
		}
			

		yield return new WaitForSeconds(timeToPlayFX);

		if(!successfulReaction)
		{
			//failureEnding.objectToSpawnWhenStatusHappens.SetActive(false);
			
		}
		if(failureEnding.timesEndingtimeEncounteredBeforeResetting <= 0)
		{
			//change the state of the reaction vessel back to zero?
			//reset game?
			FindObjectOfType<AdditiveLoad>().ReloadGame();
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
		public GameObject objectToSpawnWhenStatusHappens;
		public AudioSource soundToPlay;
		public int timesEndingtimeEncounteredBeforeResetting = 1;
	}

	#endregion ending logic
}
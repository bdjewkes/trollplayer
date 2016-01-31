using System;
using UnityEngine;

public class OuttroController : MonoBehaviour {
	public Action OnOuttroDone;

	[ContextMenu("manual fake outtro end")]
	public void OuttroDone()
	{
		if(OnOuttroDone != null)
		{
			OnOuttroDone();
		}
	}
}

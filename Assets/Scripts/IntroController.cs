using System;
using UnityEngine;

public class IntroController : MonoBehaviour
{
	public Action OnIntroDone;

	[ContextMenu("manual fake intro end")]
	public void IntroDone()
	{
		if(OnIntroDone != null)
		{
			OnIntroDone();
		}
	}
}

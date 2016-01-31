using System;
using UnityEngine;
using System.Collections;

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

using UnityEngine;
using System.Collections;

public class OuttroScaffolding : MonoBehaviour
{
	[SerializeField] private float timeToWait = 2f;
	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds(timeToWait);
		FindObjectOfType<OuttroController>().OuttroDone();
	}
	
}

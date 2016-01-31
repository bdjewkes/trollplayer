using UnityEngine;
using System.Collections;

public class Gold : MonoBehaviour {
    public AudioSource sound;

    public void OnCollisionEnter(Collision c) {
        Debug.Log("gold");
        sound.Play();
    }
}

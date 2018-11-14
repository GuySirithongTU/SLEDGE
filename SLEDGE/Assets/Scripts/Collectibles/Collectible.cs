using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {

    [SerializeField] private AudioSource collectSound;

	public void Collect()
    {
        collectSound.Play();

        Destroy(gameObject);
    }
}

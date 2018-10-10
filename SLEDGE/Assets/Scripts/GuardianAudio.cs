using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianAudio : MonoBehaviour {

    [SerializeField] private AudioSource stepSound;
    [SerializeField] private AudioSource impactSound;

    public void playStepSound()
    {
        if(FindObjectOfType<GuardianController>().getIsGrounded() && FindObjectOfType<GuardianController>().GetComponent<Rigidbody>().velocity.magnitude >= 1f) {
            stepSound.Play();
        }
    }

    public void playImpactSound()
    {
        impactSound.Play();
    }
}

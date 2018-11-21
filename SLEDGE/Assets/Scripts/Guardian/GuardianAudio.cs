using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianAudio : MonoBehaviour {

    [SerializeField] private AudioSource stepSound;
    [SerializeField] private AudioSource impactSound;
    [SerializeField] private AudioSource landSound;

    private void Start()
    {
        GetComponentInParent<GuardianController>().landEvent += playLandSound;
    }

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

    public void playLandSound()
    {
        landSound.Play();
    }
}

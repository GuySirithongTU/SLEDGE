using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialVolume : MonoBehaviour {


    [SerializeField] private Camera mainCamera;
    [SerializeField] private Animator tutorialUIAnimator;
    [SerializeField] private Transform focusTransform;
    
    private bool tutorialEnded = false;

    private static TutorialVolume lastVolume;

    private void Update()
    {
        if(this == lastVolume) {
            tutorialUIAnimator.transform.position = mainCamera.WorldToScreenPoint(focusTransform.position);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(!tutorialEnded && collider.CompareTag("Guardian")) {
            tutorialUIAnimator.gameObject.SetActive(true);

            tutorialUIAnimator.SetBool("showing", true);

            lastVolume = this;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if(!tutorialEnded && collider.CompareTag("Guardian")) {
            tutorialUIAnimator.SetBool("showing", false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialVolume : MonoBehaviour {


    [SerializeField] private Camera mainCamera;
    [SerializeField] private Animator tutorialUIAnimator;
    [SerializeField] private Transform focusTransform;
    private Collider destinationCollider;
    
    private bool tutorialUIIsActive = false;

    private void Start()
    {
        destinationCollider = GetComponentInChildren<Collider>();
    }

    private void Update()
    {
        if(tutorialUIIsActive) {
            tutorialUIAnimator.transform.position = mainCamera.WorldToScreenPoint(focusTransform.position);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Guardian")) {
            tutorialUIIsActive = true;
            tutorialUIAnimator.gameObject.SetActive(true);
        }
    }

    public void OnTutorialEnd()
    {
        tutorialUIAnimator.SetTrigger("end");
    }
}

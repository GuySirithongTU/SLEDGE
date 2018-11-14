using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDestinationVolume : MonoBehaviour {

	private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Guardian")) {
            transform.GetComponentInParent<TutorialVolume>().OnTutorialEnd();
        }
    }
}

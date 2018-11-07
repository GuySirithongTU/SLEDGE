using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateGuardianCheck : MonoBehaviour {

    [SerializeField] private Gate gate;

	private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Guardian")) {
            gate.OnGuardianEnter();
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if(collider.CompareTag("Guardian")) {
            gate.OnGuardianExit();
        }
    }
}

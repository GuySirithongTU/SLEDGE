using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManager : MonoBehaviour {

    [SerializeField] private float frontZ = -3.0f;
    [SerializeField] private float backZ = -2.0f;

    private bool guardianIsInFront = true;

    private GameObject guardian;

    private void Start()
    {
        guardian = FindObjectOfType<GuardianController>().gameObject;
    }

    private void Update()
    {
        if(guardianIsInFront && !guardian.GetComponent<GuardianController>().getIsPlatformAttached()) {
            guardian.transform.position = new Vector3(guardian.transform.position.x, guardian.transform.position.y, frontZ);
        } else if (!guardianIsInFront && !guardian.GetComponent<GuardianController>().getIsPlatformAttached()) {
            guardian.transform.position = new Vector3(guardian.transform.position.x, guardian.transform.position.y, backZ);
        }

        
    }

    private void OnGuardianMoveFront()
    {
        guardianIsInFront = false;

        MeshRenderer[] meshes = FindObjectsOfType<MeshRenderer>();
        foreach (MeshRenderer i in meshes) {
            if (i.gameObject.layer == 10) {
                i.material.color = new Color(i.material.color.r, i.material.color.g, i.material.color.b, 0.0f);
            }
        }
    }

    private void OnGuardianMoveBack()
    {
        guardianIsInFront = true;

        MeshRenderer[] meshes = FindObjectsOfType<MeshRenderer>();
        foreach (MeshRenderer i in meshes) {
            if (i.gameObject.layer == 10) {
                i.material.color = new Color(i.material.color.r, i.material.color.g, i.material.color.b, 1.0f);
            }
        }
    }
}

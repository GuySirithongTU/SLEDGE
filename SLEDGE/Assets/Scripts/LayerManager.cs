using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManager : MonoBehaviour {

    [SerializeField] private float frontZ = -4.0f;
    [SerializeField] private float backZ = -2.0f;

    private bool guardianIsInFront = true;

    private GameObject guardian;

    private void Start()
    {
        guardian = FindObjectOfType<GuardianController>().gameObject;
    }

    private void Update()
    {
        // Move guardian's layer if threshold is crossed.
        if (guardian.transform.position.z < frontZ + 0.3f) {
            OnGuardianMoveFront();
        } else if(guardian.transform.position.z > backZ - 0.3f) {
            OnGuardianMoveBack();
        }

        // Snap guardian to layer's z if not being attached to a platform.
        if (guardianIsInFront && !guardian.GetComponent<GuardianController>().getIsPlatformAttached()) {
            guardian.transform.position = new Vector3(guardian.transform.position.x, guardian.transform.position.y, frontZ);
        } else if (!guardianIsInFront && !guardian.GetComponent<GuardianController>().getIsPlatformAttached()) {
            guardian.transform.position = new Vector3(guardian.transform.position.x, guardian.transform.position.y, backZ);
        }
    }

    private void OnGuardianMoveFront()
    {
        guardianIsInFront = true;

        MeshRenderer[] meshes = FindObjectsOfType<MeshRenderer>();
        foreach (MeshRenderer i in meshes) {
            if (i.gameObject.layer == 10) {
                i.material.color = new Color(i.material.color.r, i.material.color.g, i.material.color.b, 1.0f);
            }
        }
    }

    private void OnGuardianMoveBack()
    {
        guardianIsInFront = false;

        MeshRenderer[] meshes = FindObjectsOfType<MeshRenderer>();
        foreach (MeshRenderer i in meshes) {
            if (i.gameObject.layer == 10) {
                i.material.color = new Color(i.material.color.r, i.material.color.g, i.material.color.b, 0.5f);
            }
        }
    }

    public bool getGuardianIsInFront()
    {
        return guardianIsInFront;
    }
}

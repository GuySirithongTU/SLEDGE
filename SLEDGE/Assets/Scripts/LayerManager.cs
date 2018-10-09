using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class LayerManager : MonoBehaviour {

    [SerializeField] private float frontZ = -4.0f;
    [SerializeField] private float backZ = -2.0f;

    private bool guardianIsInFront = true;

    private GameObject guardian;

    [SerializeField] private PostProcessingProfile frontLayer;
    [SerializeField] private PostProcessingProfile backLayer;

    [SerializeField] private MeshRenderer frontPlane;

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
        if (guardianIsInFront && !guardian.GetComponent<GuardianController>().getPlatformRotating()) {
            guardian.transform.position = new Vector3(guardian.transform.position.x, guardian.transform.position.y, frontZ);
        } else if (!guardianIsInFront && !guardian.GetComponent<GuardianController>().getPlatformRotating()) {
            guardian.transform.position = new Vector3(guardian.transform.position.x, guardian.transform.position.y, backZ);
        }

        // Set alpha for front camera render texture.
        float alpha = - ((guardian.transform.position.z - frontZ) * backZ / frontZ) + 1;
        frontPlane.material.SetColor("_Color", new Color(1f, 1f, 1f, Mathf.Clamp01(alpha)));
    }

    private void OnGuardianMoveFront()
    {
        guardianIsInFront = true;
    }

    private void OnGuardianMoveBack()
    {
        guardianIsInFront = false;
    }

    public bool getGuardianIsInFront()
    {
        return guardianIsInFront;
    }
}

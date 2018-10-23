using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraVolume : MonoBehaviour {

    [SerializeField] private float cameraZ = -20f;

    private Transform cameraAnchor;

    private void Start()
    {
        cameraAnchor = transform.GetChild(0);
    }

    public float getCameraZ()
    {
        return cameraZ;
    }

    public Transform getCameraAnchor()
    {
        return cameraAnchor;
    }
}

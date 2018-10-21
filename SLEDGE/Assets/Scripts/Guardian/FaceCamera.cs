using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour {

    [SerializeField] private Vector3 cameraDirection = new Vector3(0f, 0f, -1f);

    private void LateUpdate()
    {
        transform.LookAt((transform.position - cameraDirection));
    }
}

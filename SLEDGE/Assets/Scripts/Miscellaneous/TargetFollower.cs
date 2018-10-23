using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFollower : MonoBehaviour {

    [SerializeField] private float ease = 0.2f;
    [SerializeField] private Vector3 offset = new Vector3(0.0f, 0.0f, -10.0f);
    [SerializeField] private float parallax = 1.0f;
    [SerializeField] private bool lockZ = true;
    [SerializeField] private bool isCamera = false;
    [SerializeField] private float zValue = -10f;

    private Vector3 velocity;
    private CameraVolume cameraVolume = null;

    [SerializeField] private Transform target;

    private void Start()
    {
        if (lockZ) {
            transform.position = new Vector3(target.position.x * parallax, target.position.y * parallax, zValue - offset.z) + offset;
        } else {
            transform.position = new Vector3(target.position.x * parallax, target.position.y * parallax, target.position.z) + offset;
        }

        FindObjectOfType<GuardianController>().enterCameraVolumeEvent += OnEnterCameraVolume;
        FindObjectOfType<GuardianController>().exitCameraVolumeEvent += OnExitCameraVolume;
    }

    private void FixedUpdate()
    {
        Vector3 targetPosition;

        if(isCamera && cameraVolume != null) {
            //Debug.Log("x");
            targetPosition = cameraVolume.getCameraAnchor().position;
            targetPosition = new Vector3(targetPosition.x, targetPosition.y, cameraVolume.getCameraZ());
        } else if(lockZ) {
            targetPosition = new Vector3(target.position.x * parallax + offset.x, target.position.y * parallax + offset.y, zValue);
        } else {
            targetPosition = new Vector3(target.position.x * parallax, target.position.y * parallax, target.position.z) + offset;
        }

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, ease);
    }

    private void OnEnterCameraVolume(CameraVolume newCameraVolume)
    {
        cameraVolume = newCameraVolume;
    }

    private void OnExitCameraVolume()
    {
        cameraVolume = null;
    }
}

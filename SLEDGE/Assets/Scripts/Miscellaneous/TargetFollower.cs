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
    [SerializeField] private float lockVertical = 0.5f;
    [SerializeField] private float lockHorizontal = 3f;

    private Vector3 velocity;
    private CameraVolume cameraVolume = null;
    private bool justExitedCameraVolume = false;

    [SerializeField] private Transform target;

    private void Start()
    {
        if (lockZ) {
            transform.position = new Vector3(target.position.x * parallax, target.position.y * parallax, zValue - offset.z) + offset;
        } else {
            transform.position = new Vector3(target.position.x * parallax, target.position.y * parallax, target.position.z) + offset;
        }

        if(FindObjectsOfType<GuardianController>().Length > 0) {
            FindObjectOfType<GuardianController>().enterCameraVolumeEvent += OnEnterCameraVolume;
            FindObjectOfType<GuardianController>().exitCameraVolumeEvent += OnExitCameraVolume;
        }
    }

    private void FixedUpdate()
    {
        Vector3 targetPosition;

        if(isCamera && cameraVolume != null) {
            targetPosition = cameraVolume.getCameraAnchor().position;
            targetPosition = new Vector3(targetPosition.x, targetPosition.y, cameraVolume.getCameraZ());
        } else if(lockZ) {
            targetPosition = new Vector3(target.position.x * parallax + offset.x, target.position.y * parallax + offset.y, zValue);
        } else {
            targetPosition = new Vector3(target.position.x * parallax, target.position.y * parallax, target.position.z) + offset;
        }

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, ease);
    }

    private void LateUpdate()
    {
        if (isCamera && cameraVolume == null && !justExitedCameraVolume) {
            Vector3 targetPosition = new Vector3(target.position.x * parallax + offset.x, target.position.y * parallax + offset.y, zValue);

            if (transform.position.y > targetPosition.y + lockVertical) {
                transform.position = new Vector3(transform.position.x, targetPosition.y + lockVertical, transform.position.z);
            }

            if (transform.position.y < targetPosition.y - lockVertical) {
                transform.position = new Vector3(transform.position.x, targetPosition.y + lockVertical, transform.position.z);
            }

            if (transform.position.x > targetPosition.x + lockHorizontal) {
                transform.position = new Vector3(targetPosition.x - lockHorizontal, transform.position.y, transform.position.z);
            }

            if (transform.position.x < targetPosition.x - lockHorizontal) {
                transform.position = new Vector3(targetPosition.x + lockHorizontal, transform.position.y, transform.position.z);
            }
        }
    }

    private void OnEnterCameraVolume(CameraVolume newCameraVolume)
    {
        cameraVolume = newCameraVolume;
    }

    private void OnExitCameraVolume()
    {
        cameraVolume = null;
        StartCoroutine(UpdateJustExitedCameraVolume());
    }

    private IEnumerator UpdateJustExitedCameraVolume()
    {
        justExitedCameraVolume = true;
        Vector3 targetPosition = new Vector3(target.position.x * parallax + offset.x, target.position.y * parallax + offset.y, zValue);
        
        while(transform.position.y > targetPosition.y + lockVertical ||
            transform.position.y < targetPosition.y - lockVertical ||
            transform.position.x > targetPosition.x + lockHorizontal ||
            transform.position.x < targetPosition.x - lockHorizontal) {
            
            yield return null;
            targetPosition = new Vector3(target.position.x * parallax + offset.x, target.position.y * parallax + offset.y, zValue);
        }

        justExitedCameraVolume = false;
    }
}

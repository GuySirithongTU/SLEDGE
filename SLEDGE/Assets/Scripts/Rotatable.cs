using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotatable : MonoBehaviour
{

    // FIELD VARIABLES
    private enum axis { x, y, z };                              // Axes for field.
    [SerializeField] private axis rotationAxis = axis.y;        // Axis of rotation.
    [SerializeField] private float rotationSpeed = 1f;          // Speed of rotation.
    [SerializeField] private float rotationSteps = 180f;        // Step of rotation in degrees.

    // PRIVATE VARIABLES
    private bool isRotating = false;        // Rotation coroutine is running.
    private Vector3 rotationAxisVector;     // Rotation axis as a vector.

    public event Action RotateStartEvent;
    public event Action RotateEndEvent;

    private void Awake()
    {
        // Initialize rotationAxisVector.
        switch (rotationAxis) {
            case axis.x:
                rotationAxisVector = Vector3.right; break;
            case axis.y:
                rotationAxisVector = Vector3.up; break;
            case axis.z:
                rotationAxisVector = Vector3.forward; break;
            default:
                rotationAxisVector = Vector3.up; break;
        }
    }
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)) {
            StartCoroutine(Rotate(true));
        }
        if (Input.GetKeyDown(KeyCode.O)) {
            StartCoroutine(Rotate(false));
        }
    }

    private IEnumerator Rotate(bool directionPositive)
    {
        // If rotating, end coroutine.
        if (!isRotating) {
            // Update rotating status.
            if (RotateStartEvent != null) {
                RotateStartEvent.Invoke();
            }
            isRotating = true;

            // Records degrees rotated from the start of the rotation.
            float degreesRotated = 0.0f;

            yield return null;

            // Rotate each update while degrees to rotate is less than the rotation step.
            while (degreesRotated < rotationSteps) {

                // Degrees to rotation this update.
                float degreesToRotate = (rotationSteps - degreesRotated) * rotationSpeed * Time.deltaTime;
                degreesToRotate = Mathf.Clamp(degreesToRotate, 1.0f, 360.0f);

                // Rotate the platform.
                Quaternion rotation;
                if (directionPositive) {
                    rotation = Quaternion.AngleAxis(degreesToRotate, rotationAxisVector);
                } else {
                    rotation = Quaternion.AngleAxis(-degreesToRotate, rotationAxisVector);
                }
                transform.Rotate(rotation.eulerAngles);

                // Update degreesRotated.
                degreesRotated += degreesToRotate;

                // If overshot step, rotate back to destination.
                if (degreesRotated > rotationSteps) {
                    Quaternion rotationFinal = Quaternion.AngleAxis(degreesRotated - rotationSteps, rotationAxisVector);

                    if (directionPositive) {
                        transform.Rotate(-rotationFinal.eulerAngles);
                    } else {
                        transform.Rotate(rotationFinal.eulerAngles);
                    }
                }

                // Wait for next update.
                yield return null;
            }

            // Update rotating status.
            if(RotateEndEvent != null) {
                RotateEndEvent.Invoke();
            }
            isRotating = false;
        }
    }

    public void OnHammerLand(HammerMoveset.hammerModes hammerMode)
    {
        // Guardian is not on the platform
        if(!FindObjectOfType<GuardianController>().getIsPlatformAttached()) {
            // For z axis platforms.
            if (rotationAxis == axis.z && hammerMode == HammerMoveset.hammerModes.ground) {
                // Guardian is facing right.
                if (FindObjectOfType<GuardianController>().getFacingRight()) {
                    StartCoroutine(Rotate(true));
                }
                // Guardian is facing left.
                else {
                    StartCoroutine(Rotate(false));
                }
            }

            // For y axis platforms.
            else if (rotationAxis == axis.y) {
                // If guardian is in front layer and swings away from the camera.
                if(FindObjectOfType<LayerManager>().getGuardianIsInFront() && hammerMode == HammerMoveset.hammerModes.away) {
                    // Guardian is facing right.
                    if (FindObjectOfType<GuardianController>().getFacingRight()) {
                        StartCoroutine(Rotate(true));
                    }
                    // Guardian is facing left.
                    else {
                        StartCoroutine(Rotate(false));
                    }
                }
                // If guardian is in back layer and swings toward the camera.
                else if (!FindObjectOfType<LayerManager>().getGuardianIsInFront() && hammerMode == HammerMoveset.hammerModes.toward) {
                    // Guardian is facing right.
                    if (FindObjectOfType<GuardianController>().getFacingRight()) {
                        StartCoroutine(Rotate(false));
                    }
                    // Guardian is facing left.
                    else {
                        StartCoroutine(Rotate(true));
                    }
                }
                
            }

            // For x axis platforms
            else if(rotationAxis == axis.x && hammerMode == HammerMoveset.hammerModes.ground) {
                // If guardian is in front layer.
                if (FindObjectOfType<LayerManager>().getGuardianIsInFront()) {
                    StartCoroutine(Rotate(false));
                } else {
                    StartCoroutine(Rotate(true));
                }
            }
        }
    }
}
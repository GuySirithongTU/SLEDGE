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
        if (Input.GetKeyDown(KeyCode.P) && !isRotating) {
            StartCoroutine(Rotate(true));
        }

        if (Input.GetKeyDown(KeyCode.O) && !isRotating) {
            StartCoroutine(Rotate(false));
        }
    }

    private IEnumerator Rotate(bool directionPositive)
    {
        // Update rotating status.
        isRotating = true;

        // Records degrees rotated from the start of the rotation.
        float degreesRotated = 0.0f;

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
        isRotating = false;
    }
}
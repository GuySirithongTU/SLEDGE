using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotatable : MonoBehaviour {

	private enum axis { x, y, z };
    [SerializeField] private axis rotationAxis = axis.y;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float rotationSteps = 180f;
    
    private float currentAngle;
    private bool isRotating = false;

    private void Awake()
    {
        UpdateCurrentAngle();

        StartCoroutine(Rotate(true));
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P) && !isRotating) {
            StartCoroutine(Rotate(true));
        }

        if (Input.GetKeyDown(KeyCode.O) && !isRotating) {
            StartCoroutine(Rotate(false));
        }
    }

    private IEnumerator Rotate(bool directionPositive)
    {
        isRotating = true;

        float targetAngle;

        if (directionPositive) {
            targetAngle = currentAngle + rotationSteps;
        } else {
            targetAngle = currentAngle - rotationSteps;
        }
        /*
        while(targetAngle < -180.0f || targetAngle > 180.0f) {
            if(targetAngle < -180.0f) {
                targetAngle += 360.0f;
            } else if(targetAngle > 180.0f) {
                targetAngle -= 360.0f;
            }
        }
        */
        Debug.Log(targetAngle);

        while(Mathf.Abs(targetAngle - currentAngle) > 1f) {
            transform.Rotate(Vector3.up, (targetAngle - currentAngle) * Mathf.Clamp01(rotationSpeed * Time.deltaTime));
            UpdateCurrentAngle();

            yield return null;
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, targetAngle, transform.eulerAngles.z);
        UpdateCurrentAngle();

        isRotating = false;
    }

    private void UpdateCurrentAngle()
    {
        switch (rotationAxis) {
            case axis.x:
                currentAngle = transform.eulerAngles.x; break;
            case axis.y:
                currentAngle = transform.eulerAngles.y; break;
            case axis.z:
                currentAngle = transform.eulerAngles.z; break;
        }
    }
}

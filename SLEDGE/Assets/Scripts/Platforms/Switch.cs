using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour {

    [SerializeField] private float switchResistance = 1.0f;
    [SerializeField] private Transform pressedCheck;

    private bool isPressed = false;

    private Rigidbody switchRigidbody;

    public UnityEvent switchPressEvent;

    private void Awake()
    {
        switchRigidbody = transform.GetChild(0).GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Debug.Log(isPressed);
        if(!isPressed) {
            switchRigidbody.AddForce(Vector3.up * switchResistance, ForceMode.Acceleration);

            if(Vector3.Distance(transform.GetChild(0).transform.position, pressedCheck.position) <= 0.05f) {
                transform.GetChild(0).transform.position = pressedCheck.position;
                isPressed = true;

                if(switchPressEvent != null) {
                    switchPressEvent.Invoke();
                }
            }
        }
    }
}

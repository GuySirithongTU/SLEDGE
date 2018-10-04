using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour {

    [SerializeField] private float moveSpeed = 500f;

    private bool isMoving = false;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        // Initialize references to compoenents.
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // If not moving, platform is kinematic. Else, platform is not kinematic.
        if (!isMoving) {
            _rigidbody.isKinematic = true;
        } else {
            _rigidbody.isKinematic = false;
        }
    }

    private IEnumerator Move(bool movingRight)
    {
        isMoving = true;
        
        int iteration = 0;
        while (isMoving) {

            // Set rigidbody velocity.
            if (movingRight) {
                _rigidbody.velocity = new Vector3(moveSpeed * Time.deltaTime, _rigidbody.velocity.y, _rigidbody.velocity.z);
            } else {
                _rigidbody.velocity = new Vector3(-moveSpeed * Time.deltaTime, _rigidbody.velocity.y, _rigidbody.velocity.z);
            }

            // Wait for next update.
            yield return null;
            
            // Every 4 iteration, if velocity is low enough, stop moving.
            if ((iteration % 4 == 3) && (_rigidbody.velocity.magnitude < 0.005f)) { 
                isMoving = false;
                iteration = 0;
            }

            iteration++;
        }
    }

    public void OnHammerLand()
    {
        // If not moving, start moving.
        if(!isMoving) {
            if (FindObjectOfType<GuardianController>().getFacingRight()) {
                StartCoroutine(Move(true));
            } else {
                StartCoroutine(Move(false));
            }
        }
    }
}

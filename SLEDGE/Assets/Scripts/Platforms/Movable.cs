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
        //Debug.Log(isMoving);
    }

    private IEnumerator Move(bool movingRight)
    {
        isMoving = true;

        yield return null;
        if(movingRight) {
            _rigidbody.AddForce(Vector3.right * moveSpeed, ForceMode.Impulse);
        } else {
            _rigidbody.AddForce(Vector3.left * moveSpeed, ForceMode.Impulse);
        }

        bool stoppedMoving = false;
        float stoppedTime = Time.time;
        
        while (isMoving) {
            /*
            // Set rigidbody velocity.
            _rigidbody.velocity = new Vector3(moveSpeed, _rigidbody.velocity.y, _rigidbody.velocity.z);
            /*if (movingRight) {
                _rigidbody.velocity = new Vector3(velocityX, _rigidbody.velocity.y, _rigidbody.velocity.z);
            } else {
                _rigidbody.velocity = new Vector3(-velocityX, _rigidbody.velocity.y, _rigidbody.velocity.z);
            }
            yield return new WaitForSeconds(1f);
            // Wait for next update.
            */
            //yield return null;

            
            if(_rigidbody.velocity.magnitude < 0.005f && !stoppedMoving) {
                stoppedTime = Time.time;
                stoppedMoving = true;
            }

            if(_rigidbody.velocity.magnitude >= 0.005f) {
                stoppedTime = Time.time;
                stoppedMoving = false;
            }

            if (stoppedTime + 1f < Time.time && stoppedMoving) { 
                isMoving = false;
            }
            
            yield return null;
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

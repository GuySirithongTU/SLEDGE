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
        if(Input.GetKeyDown(KeyCode.L)) {
        }

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
        
        float lastMoving = Time.time;
        
        while (isMoving) {
            
            if(_rigidbody.velocity.magnitude >= 0.005f) {
                lastMoving = Time.time;
            }

            if (lastMoving + 1f <= Time.time) {
                isMoving = false;
                Debug.Log("Stop");
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

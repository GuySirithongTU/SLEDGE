using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianController : MonoBehaviour
{
    private float walk = 0.0f;
    [SerializeField] private float walkSpeed = 5.0f;
    [Range(0, 0.5f)] [SerializeField] private float walkSmooth = 0.05f;
    
    private bool jump = false;
    [SerializeField] private float jumpForce = 100.0f;
    [SerializeField] private float jumpFallMultiplier = 2.0f;

    private bool isGrounded = false;
    [SerializeField] private Transform groundCheck;
    const float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundCheckMask;
    private Vector3 velocity = Vector3.zero;

    private bool facingRight = true;
    private bool wasGrounded = false;
    private bool justJumped = false;
    private bool isPlatformAttached = false;
    private bool isSwinging = false;
    private bool platformRotating = false;
    private bool goalReached = false;
    
    private Rigidbody _rigidbody;

    public event Action jumpEvent;
    public event Action landEvent;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        GetComponent<HammerMoveset>().swingStartEvent += OnSwingingStart;
        GetComponent<HammerMoveset>().swingEndEvent += OnSwingingEnd;

        Rotatable[] rotatables = FindObjectsOfType<Rotatable>();
        foreach(Rotatable rotatable in rotatables) {
            rotatable.RotateStartEvent += OnPlatformStartRotating;
            rotatable.RotateEndEvent += OnPlatformStopRotating;
        }

        PlatformAttach[] platformAttaches = FindObjectsOfType<PlatformAttach>();
        foreach(PlatformAttach platformAttach in platformAttaches) {
            platformAttach.attachEvent += OnPlatformAttach;
            platformAttach.detachEvent += OnPlatformDetach;
        }

        FindObjectOfType<Goal>().reachGoalEvent += OnReachGoal;
    }

    private void Update()
    {
        walk = Input.GetAxisRaw("Horizontal") * walkSpeed;
        jump = Input.GetButton("Jump");
        
        CheckGround();
        
        if (_rigidbody.velocity.magnitude >= 0.1f) {
            transform.GetChild(0).GetComponent<Animator>().SetBool("isWalking", true);
            //transform.GetChild(1).GetComponent<Animator>().SetBool("isWalking", true);
        } else {
            transform.GetChild(0).GetComponent<Animator>().SetBool("isWalking", false);
            //transform.GetChild(1).GetComponent<Animator>().SetBool("isWalking", false);
        }

        if (!isSwinging && !platformRotating && !goalReached) {
            Walk();
            Jump();

            if ((walk > 0.0f && !facingRight) || (walk < 0.0f && facingRight)) {
                Flip();
            }
        }


    }

    private void CheckGround() {

        Collider[] hitColliders = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, groundCheckMask);
        if(hitColliders.Length > 0) {
            isGrounded = true;

            if(!wasGrounded) {
                landEvent.Invoke();
                _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y, 0f);
            }

            wasGrounded = true;
        } else {
            isGrounded = false;

            if(wasGrounded) {
                jumpEvent.Invoke();
            }

            wasGrounded = false;
        }
    }

    private void Walk()
    {
        Vector3 targetVelocity = new Vector3(walk, _rigidbody.velocity.y, _rigidbody.velocity.z);
        _rigidbody.velocity = Vector3.SmoothDamp(_rigidbody.velocity, targetVelocity, ref velocity, walkSmooth);
    }

    private void Jump()
    {
        // Initial force.
        if(jump && isGrounded && !justJumped) {
            StartCoroutine(JustJumpStart());
            isGrounded = false;
            _rigidbody.AddForce(new Vector3(0.0f, jumpForce, 0.0f));
        }

        // Falling multiplier
        if((!jump && !isGrounded) || velocity.y < 0.0f) {
            _rigidbody.velocity += Vector3.up * Physics.gravity.y * (jumpFallMultiplier - 1) * Time.deltaTime;
        }
    }

    private IEnumerator JustJumpStart()
    {
        justJumped = true;

        yield return new WaitForSeconds(0.1f);

        justJumped = false;
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    public bool getFacingRight()
    {
        return facingRight;
    }

    private void OnSwingingStart()
    {
        isSwinging = true;
        _rigidbody.velocity = Vector3.zero;
    }

    private void OnSwingingEnd()
    {
        isSwinging = false;
    }

    private void OnPlatformAttach()
    {
        isPlatformAttached = true;
    }

    private void OnPlatformDetach()
    {
        isPlatformAttached = false;
    }

    private void OnPlatformStartRotating()
    {
        platformRotating = true;
        _rigidbody.velocity = Vector3.zero;
    }

    private void OnPlatformStopRotating()
    {
        platformRotating = false;
    }

    public void OnReachGoal()
    {
        goalReached = true;
        _rigidbody.velocity = Vector3.zero;
    }
    
    public bool getIsGrounded()
    {
        return isGrounded;
    }

    public bool getIsPlatformAttached()
    {
        return isPlatformAttached;
    }

    public bool getPlatformRotating()
    {
        return platformRotating;
    }
}

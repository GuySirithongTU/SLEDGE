using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerMoveset : MonoBehaviour {

	public enum hammerModes { ground, toward, away };
    private hammerModes hammerMode;

    private bool isAirbourne;
    private bool isSwinging;

    [SerializeField] private Transform hammerGroundCheck;
    [SerializeField] private Transform hammerFrontCheck;
    [SerializeField] private Transform hammerBackCheck;
    private const float hammerCheckRadius = 0.1f;
    [SerializeField] private LayerMask rotatablePlatformCheckMask;
    [SerializeField] private LayerMask staticPlatformCheckMask;

    private Animator[] _animators;

    public event Action swingStartEvent;
    public event Action swingEndEvent;

    private void Awake()
    {
        //Initialize hammer mode.
        hammerMode = hammerModes.ground;

        isSwinging = false;
    }

    private void Start()
    {
        GetComponent<GuardianController>().jumpEvent += OnJump;
        GetComponent<GuardianController>().landEvent += OnLand;
        
        _animators = gameObject.GetComponentsInChildren<Animator>();
    }

    private void Update()
    {
        UpdateMode();
        StartCoroutine(Swing());

        //Debug.Log(hammerMode);

    }

    private IEnumerator Swing()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !isAirbourne && !isSwinging) {
            isSwinging = true;
            if (swingStartEvent != null) {
                swingStartEvent.Invoke();
            }
            _animators[0].SetTrigger("Swing");
            _animators[1].SetTrigger("Swing");

            yield return new WaitForSeconds(0.1f);

            ///// GROUND
            if (hammerMode == hammerModes.ground) {
                // For ground while on a static platform, check ground transform for rotatables.
                if (!GetComponent<GuardianController>().getIsPlatformAttached()) {
                    Collider[] hitColliders = Physics.OverlapSphere(hammerGroundCheck.position, hammerCheckRadius, rotatablePlatformCheckMask);
                    if (hitColliders.Length > 0) {
                        if (hitColliders[0].GetComponent<Rotatable>() != null) {
                            hitColliders[0].GetComponent<Rotatable>().OnHammerLand(hammerMode, false);
                        }
                    }
                }
                // For ground while on a rotatable platform, check ground transform for statics.
                else {
                    Collider[] hitColliders = Physics.OverlapSphere(hammerGroundCheck.position, hammerCheckRadius, staticPlatformCheckMask);
                    if (hitColliders.Length > 0) {
                        if (transform.parent.GetComponent<Rotatable>() != null) {
                            transform.parent.GetComponent<Rotatable>().OnHammerLand(hammerMode, true);
                        }
                    }
                }
            ///// AWAY
            } else if (hammerMode == hammerModes.away) {
                // For away while on a static platform, check back transform for rotatables.
                if (!GetComponent<GuardianController>().getIsPlatformAttached()) {
                    Collider[] hitColliders = Physics.OverlapSphere(hammerBackCheck.position, hammerCheckRadius, rotatablePlatformCheckMask);
                    if (hitColliders.Length > 0) {
                        if (hitColliders[0].GetComponent<Rotatable>() != null) {
                            hitColliders[0].GetComponent<Rotatable>().OnHammerLand(hammerMode, false);
                        }
                    }
                }

                // For away while on a rotatable platform, check back transform for statics.
                else {
                    Collider[] hitColliders = Physics.OverlapSphere(hammerBackCheck.position, hammerCheckRadius, staticPlatformCheckMask);
                    if (hitColliders.Length > 0) {
                        if (transform.parent.GetComponent<Rotatable>() != null) {
                            transform.parent.GetComponent<Rotatable>().OnHammerLand(hammerMode, true);
                        }
                    }
                }
            ///// TOWARD
            } else if (hammerMode == hammerModes.toward) {
                // For toward while on a static platform, check front transform for rotatables.
                if (!GetComponent<GuardianController>().getIsPlatformAttached()) {
                    // For toward, check front transform.
                    Collider[] hitColliders = Physics.OverlapSphere(hammerFrontCheck.position, hammerCheckRadius, rotatablePlatformCheckMask);
                    if (hitColliders.Length > 0) {
                        if (hitColliders[0].GetComponent<Rotatable>() != null) {
                            hitColliders[0].GetComponent<Rotatable>().OnHammerLand(hammerMode, false);
                        }
                    }
                }
                // For toward while on a rotatable platform, check front transform for statics.
                else {
                    Collider[] hitColliders = Physics.OverlapSphere(hammerFrontCheck.position, hammerCheckRadius, staticPlatformCheckMask);
                    if (hitColliders.Length > 0) {
                        if (transform.parent.GetComponent<Rotatable>() != null) {
                            transform.parent.GetComponent<Rotatable>().OnHammerLand(hammerMode, true);
                        }
                    }
                }
            }

            yield return new WaitForSeconds(0.4f);
            
            isSwinging = false;
            if (swingEndEvent != null) {
                swingEndEvent.Invoke();
            }
        }
    }
    
    private void UpdateMode()
    {
        // Ground mode.
        if(Input.GetKeyDown(KeyCode.DownArrow)) {
            hammerMode = hammerModes.ground;
        }

        // Toward mode.
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            hammerMode = hammerModes.toward;
        }

        // Away mode.
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            hammerMode = hammerModes.away;
        }
    }

    private void OnJump()
    {
        isAirbourne = true;
    }

    private void OnLand()
    {
        isAirbourne = false;
    }
}
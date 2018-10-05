using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class HammerMoveset : MonoBehaviour {

	public enum hammerModes { ground, toward, away };
    private hammerModes hammerMode;

    private bool isAirbourne;
    private bool isSwinging;

    [SerializeField] private Transform hammerGroundCheck;
    [SerializeField] private Transform hammerFrontCheck;
    [SerializeField] private Transform hammerBackCheck;
    [SerializeField] private Transform hammerSideCheck;
    private const float hammerCheckRadius = 0.1f;
    [SerializeField] private LayerMask rotatablePlatformCheckMask;
    [SerializeField] private LayerMask staticPlatformCheckMask;
    [SerializeField] private LayerMask movablePlatformCheckMask;

    [SerializeField] private ParticleSystem impactParticle;

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


            Collider[] hitColliders;
            ///// GROUND
            if (hammerMode == hammerModes.ground) {
                // For ground while on a static platform, check ground transform for rotatables.
                if (!GetComponent<GuardianController>().getIsPlatformAttached()) {
                    hitColliders = Physics.OverlapSphere(hammerGroundCheck.position, hammerCheckRadius, rotatablePlatformCheckMask);
                    if (hitColliders.Length > 0) {
                        if (hitColliders[0].GetComponent<Rotatable>() != null) {
                            hitColliders[0].GetComponent<Rotatable>().OnHammerLand(hammerMode, false);

                            InstantiateImpactParticle(hitColliders, false);
                            cameraShake();
                        }
                    }
                }
                // For ground while on a rotatable platform, check ground transform for statics.
                else {
                    hitColliders = Physics.OverlapSphere(hammerGroundCheck.position, hammerCheckRadius, staticPlatformCheckMask);
                    if (hitColliders.Length > 0) {
                        if (transform.parent.GetComponent<Rotatable>() != null) {
                            transform.parent.GetComponent<Rotatable>().OnHammerLand(hammerMode, true);

                            InstantiateImpactParticle(hitColliders, false);
                            cameraShake();
                        }
                    }
                }
            ///// AWAY
            } else if (hammerMode == hammerModes.away) {
                // For away while on a static platform, check back transform for rotatables.
                if (!GetComponent<GuardianController>().getIsPlatformAttached()) {
                    hitColliders = Physics.OverlapSphere(hammerBackCheck.position, hammerCheckRadius, rotatablePlatformCheckMask);
                    if (hitColliders.Length > 0) {
                        if (hitColliders[0].GetComponent<Rotatable>() != null) {
                            hitColliders[0].GetComponent<Rotatable>().OnHammerLand(hammerMode, false);

                            InstantiateImpactParticle(hitColliders, false);
                            cameraShake();
                        }
                    }
                }

                // For away while on a rotatable platform, check back transform for statics.
                else {
                    hitColliders = Physics.OverlapSphere(hammerBackCheck.position, hammerCheckRadius, staticPlatformCheckMask);
                    if (hitColliders.Length > 0) {
                        if (transform.parent.GetComponent<Rotatable>() != null) {
                            transform.parent.GetComponent<Rotatable>().OnHammerLand(hammerMode, true);

                            InstantiateImpactParticle(hitColliders, true);
                            cameraShake();
                        }
                    }
                }
            ///// TOWARD
            } else if (hammerMode == hammerModes.toward) {
                // For toward while on a static platform, check front transform for rotatables.
                if (!GetComponent<GuardianController>().getIsPlatformAttached()) {
                    // For toward, check front transform.
                    hitColliders = Physics.OverlapSphere(hammerFrontCheck.position, hammerCheckRadius, rotatablePlatformCheckMask);
                    if (hitColliders.Length > 0) {
                        if (hitColliders[0].GetComponent<Rotatable>() != null) {
                            hitColliders[0].GetComponent<Rotatable>().OnHammerLand(hammerMode, false);

                            InstantiateImpactParticle(hitColliders, false);
                            cameraShake();
                        }
                    }
                }
                // For toward while on a rotatable platform, check front transform for statics.
                else {
                    hitColliders = Physics.OverlapSphere(hammerFrontCheck.position, hammerCheckRadius, staticPlatformCheckMask);
                    if (hitColliders.Length > 0) {
                        if (transform.parent.GetComponent<Rotatable>() != null) {
                            transform.parent.GetComponent<Rotatable>().OnHammerLand(hammerMode, true);

                            InstantiateImpactParticle(hitColliders, true);
                            cameraShake();
                        }
                    }
                }
            }

            ///// MOVABLE PLATFORMS
            if (hammerMode == hammerModes.ground) {
                hitColliders = Physics.OverlapSphere(hammerGroundCheck.position, hammerCheckRadius, movablePlatformCheckMask);
            } else {
                hitColliders = Physics.OverlapSphere(hammerSideCheck.position, hammerCheckRadius, movablePlatformCheckMask);
            }
            if(hitColliders.Length > 0 && hitColliders[0].GetComponent<Movable>() != null) {
                hitColliders[0].GetComponent<Movable>().OnHammerLand();

                InstantiateImpactParticle(hitColliders, false);
                cameraShake();
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

    private void InstantiateImpactParticle(Collider[] hitColliders, bool isOnPlatform)
    {
        ParticleSystem impact = Instantiate(impactParticle, hammerSideCheck);
        ParticleSystem.MainModule mainModule = impact.main;

        if(!isOnPlatform) {
            mainModule.startColor = hitColliders[0].transform.GetChild(0).transform.GetChild(0).GetComponent<MeshRenderer>().material.color;
        } else {
            mainModule.startColor = hitColliders[0].transform.GetComponent<MeshRenderer>().material.color;
        }
    }

    private void cameraShake()
    {
        CameraShaker.Instance.ShakeOnce(4f, 4f, 0f, 1f);
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
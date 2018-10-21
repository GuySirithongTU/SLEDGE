using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class HammerMoveset : MonoBehaviour {

	public enum hammerModes { ground, toward, away };

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
        StartCoroutine(Swing());

        //Debug.Log(hammerMode);

    }

    private IEnumerator Swing()
    {
        ///// GROUND
        if (Input.GetKeyDown(KeyCode.DownArrow) && !isAirbourne && !isSwinging) {
            isSwinging = true;
            if (swingStartEvent != null) {
                swingStartEvent.Invoke();
            }
            _animators[0].SetTrigger("swingGround");
            //_animators[1].SetTrigger("swingGround");

            yield return new WaitForSeconds(0.1f);

            Collider[] hitColliders;
            // For ground while on a static platform, check ground transform for rotatables.
            if (!GetComponent<GuardianController>().getIsPlatformAttached()) {
                hitColliders = Physics.OverlapSphere(hammerGroundCheck.position, hammerCheckRadius, rotatablePlatformCheckMask);
                if (hitColliders.Length > 0) {
                    if (hitColliders[0].GetComponent<Rotatable>() != null) {
                        hitColliders[0].GetComponent<Rotatable>().OnHammerLand(hammerModes.ground, false);

                        InstantiateImpactParticle(hitColliders, false);
                        cameraShake();
                        FindObjectOfType<GuardianAudio>().playImpactSound();
                    }
                }
            }
            // For ground while on a rotatable platform, check ground transform for statics.
            else {
                hitColliders = Physics.OverlapSphere(hammerGroundCheck.position, hammerCheckRadius, staticPlatformCheckMask);
                if (hitColliders.Length > 0) {
                    if (transform.parent.GetComponent<Rotatable>() != null) {
                        transform.parent.GetComponent<Rotatable>().OnHammerLand(hammerModes.ground, true);

                        InstantiateImpactParticle(hitColliders, false);
                        cameraShake();
                        FindObjectOfType<GuardianAudio>().playImpactSound();
                    }
                }
            }

            // For movable platforms.
            hitColliders = Physics.OverlapSphere(hammerSideCheck.position, hammerCheckRadius, movablePlatformCheckMask);
            if (hitColliders.Length > 0 && hitColliders[0].GetComponent<Movable>() != null) {
                hitColliders[0].GetComponent<Movable>().OnHammerLand();

                InstantiateImpactParticle(hitColliders, false);
                cameraShake();
                FindObjectOfType<GuardianAudio>().playImpactSound();
            }

            yield return new WaitForSeconds(0.4f);

            isSwinging = false;
            if (swingEndEvent != null) {
                swingEndEvent.Invoke();
            }
        }

        ///// AWAY
        if (Input.GetKeyDown(KeyCode.RightArrow) && !isAirbourne && !isSwinging) {
            isSwinging = true;
            if (swingStartEvent != null) {
                swingStartEvent.Invoke();
            }
            _animators[0].SetTrigger("swingAway");
            //_animators[1].SetTrigger("swingAway");

            yield return new WaitForSeconds(0.1f);

            Collider[] hitColliders;
            // For away while on a static platform, check back transform for rotatables.
            if (!GetComponent<GuardianController>().getIsPlatformAttached()) {
                hitColliders = Physics.OverlapSphere(hammerBackCheck.position, hammerCheckRadius, rotatablePlatformCheckMask);
                if (hitColliders.Length > 0) {
                    if (hitColliders[0].GetComponent<Rotatable>() != null) {
                        hitColliders[0].GetComponent<Rotatable>().OnHammerLand(hammerModes.away, false);

                        InstantiateImpactParticle(hitColliders, false);
                        cameraShake();
                        FindObjectOfType<GuardianAudio>().playImpactSound();
                    }
                }
            }

            // For away while on a rotatable platform, check back transform for statics.
            else {
                hitColliders = Physics.OverlapSphere(hammerBackCheck.position, hammerCheckRadius, staticPlatformCheckMask);
                if (hitColliders.Length > 0) {
                    if (transform.parent.GetComponent<Rotatable>() != null) {
                        transform.parent.GetComponent<Rotatable>().OnHammerLand(hammerModes.away, true);

                        InstantiateImpactParticle(hitColliders, true);
                        cameraShake();
                        FindObjectOfType<GuardianAudio>().playImpactSound();
                    }
                }
            }

            // For movable platforms.
            hitColliders = Physics.OverlapSphere(hammerSideCheck.position, hammerCheckRadius, movablePlatformCheckMask);
            if (hitColliders.Length > 0 && hitColliders[0].GetComponent<Movable>() != null) {
                hitColliders[0].GetComponent<Movable>().OnHammerLand();

                InstantiateImpactParticle(hitColliders, false);
                cameraShake();
                FindObjectOfType<GuardianAudio>().playImpactSound();
            }

            yield return new WaitForSeconds(0.4f);

            isSwinging = false;
            if (swingEndEvent != null) {
                swingEndEvent.Invoke();
            }
        }

        ///// TOWARD
        if (Input.GetKeyDown(KeyCode.LeftArrow) && !isAirbourne && !isSwinging) {
            isSwinging = true;
            if (swingStartEvent != null) {
                swingStartEvent.Invoke();
            }
            _animators[0].SetTrigger("swingToward");
            //_animators[1].SetTrigger("swingToward");

            yield return new WaitForSeconds(0.1f);

            Collider[] hitColliders;
            // For toward while on a static platform, check front transform for rotatables.
            if (!GetComponent<GuardianController>().getIsPlatformAttached()) {
                // For toward, check front transform.
                hitColliders = Physics.OverlapSphere(hammerFrontCheck.position, hammerCheckRadius, rotatablePlatformCheckMask);
                if (hitColliders.Length > 0) {
                    if (hitColliders[0].GetComponent<Rotatable>() != null) {
                        hitColliders[0].GetComponent<Rotatable>().OnHammerLand(hammerModes.toward, false);

                        InstantiateImpactParticle(hitColliders, false);
                        cameraShake();
                        FindObjectOfType<GuardianAudio>().playImpactSound();
                    }
                }
            }
            // For toward while on a rotatable platform, check front transform for statics.
            else {
                hitColliders = Physics.OverlapSphere(hammerFrontCheck.position, hammerCheckRadius, staticPlatformCheckMask);
                if (hitColliders.Length > 0) {
                    if (transform.parent.GetComponent<Rotatable>() != null) {
                        transform.parent.GetComponent<Rotatable>().OnHammerLand(hammerModes.toward, true);

                        InstantiateImpactParticle(hitColliders, true);
                        cameraShake();
                        FindObjectOfType<GuardianAudio>().playImpactSound();
                    }
                }
            }

            // For movable platforms.
            hitColliders = Physics.OverlapSphere(hammerSideCheck.position, hammerCheckRadius, movablePlatformCheckMask);
            if (hitColliders.Length > 0 && hitColliders[0].GetComponent<Movable>() != null) {
                hitColliders[0].GetComponent<Movable>().OnHammerLand();

                InstantiateImpactParticle(hitColliders, false);
                cameraShake();
                FindObjectOfType<GuardianAudio>().playImpactSound();
            }

            yield return new WaitForSeconds(0.4f);

            isSwinging = false;
            if (swingEndEvent != null) {
                swingEndEvent.Invoke();
            }
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
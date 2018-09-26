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
    [SerializeField] private LayerMask platformCheckMask;

    private Animator _animator;

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

        _animator = gameObject.GetComponentInChildren<Animator>();
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
            _animator.SetTrigger("Swing");

            yield return new WaitForSeconds(0.1f);

            if(hammerMode == hammerModes.ground) {
                // For hammer mode ground, check ground transform.
                Collider[] hitColliders = Physics.OverlapSphere(hammerGroundCheck.position, hammerCheckRadius, platformCheckMask);
                if (hitColliders.Length > 0) {
                    if (hitColliders[0].GetComponent<Rotatable>() != null) {
                        hitColliders[0].GetComponent<Rotatable>().OnHammerLand(hammerMode);
                    }
                }
            } else if(hammerMode == hammerModes.away) {
                // For away, check back transform.
                Collider[] hitColliders = Physics.OverlapSphere(hammerBackCheck.position, hammerCheckRadius, platformCheckMask);
                if (hitColliders.Length > 0) {
                    if (hitColliders[0].GetComponent<Rotatable>() != null) {
                        hitColliders[0].GetComponent<Rotatable>().OnHammerLand(hammerMode);
                    }
                }
            } else if (hammerMode == hammerModes.toward) {
                // For toward, check front transform.
                Collider[] hitColliders = Physics.OverlapSphere(hammerFrontCheck.position, hammerCheckRadius, platformCheckMask);
                if (hitColliders.Length > 0) {
                    if (hitColliders[0].GetComponent<Rotatable>() != null) {
                        hitColliders[0].GetComponent<Rotatable>().OnHammerLand(hammerMode);
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
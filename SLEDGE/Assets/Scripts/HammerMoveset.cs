using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerMoveset : MonoBehaviour {

	private enum hammerModeList { ground, toward, away };
    private hammerModeList hammerMode;

    private bool isAirbourne;
    private bool isSwinging;

    [SerializeField] private Transform hammerGroundCheck;
    [SerializeField] private Transform hammerFrontCheck;

    private Animator _animator;

    public event Action swingStartEvent;
    public event Action swingEndEvent;

    private void Awake()
    {
        //Initialize hammer mode.
        hammerMode = hammerModeList.ground;

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

            yield return new WaitForSeconds(0.5f);
            
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
            hammerMode = hammerModeList.ground;
        }

        // Toward mode.
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            hammerMode = hammerModeList.toward;
        }

        // Away mode.
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            hammerMode = hammerModeList.away;
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
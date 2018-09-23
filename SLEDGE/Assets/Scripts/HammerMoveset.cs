using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerMoveset : MonoBehaviour {

	private enum hammerModeList { ground, toward, away };
    private hammerModeList hammerMode;

    private bool isSwinging;

    [SerializeField] private Transform hammerGroundCheck;
    [SerializeField] private Transform hammerFrontCheck;

    private Animator _animator;

    private void Awake()
    {
        //Initialize hammer mode.
        hammerMode = hammerModeList.ground;

        isSwinging = false;
    }

    private void Start()
    {
        _animator = gameObject.GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        UpdateMode();

        Swing();

        Debug.Log(hammerMode);

    }

    private void Swing()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !isSwinging) {
            isSwinging = true;

            _animator.SetTrigger("Swing");

            isSwinging = false;
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

}

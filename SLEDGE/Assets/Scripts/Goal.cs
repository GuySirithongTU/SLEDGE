using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    [SerializeField] private Animator _animator;

    public event Action reachGoalEvent;

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("Guardian")) {
            _animator.SetTrigger("goalReached");

            if(reachGoalEvent != null) {
                reachGoalEvent.Invoke();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFollower : MonoBehaviour {

    [SerializeField] private float ease = 0.5f;
    [SerializeField] private Vector3 offset = new Vector3(0.0f, 0.0f, -10.0f);
    [SerializeField] private float parallax = 1.0f;

    private Vector3 velocity;

    public Transform target;

    private void Start()
    {
        transform.position = new Vector3(target.position.x * parallax, target.position.y * parallax, target.position.z) + offset;
    }

    private void FixedUpdate()
    {
        Vector3 targetPosition = new Vector3(target.position.x * parallax, target.position.y * parallax, target.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition + offset, ref velocity, ease);
    }
}

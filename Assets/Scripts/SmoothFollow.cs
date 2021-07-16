using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    private Vector3 _offset;
    [SerializeField] private float _lagTime;

    private void Start()
    {
        _offset = transform.position - _target.position;
    }

    void Update()
    {
        var targetPosition = _target.position + _offset;
        var vel = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref vel, _lagTime);
    }
}
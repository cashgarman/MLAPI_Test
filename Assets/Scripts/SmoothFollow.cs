using System;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    private Vector3 _offset;
    [SerializeField] private float _smoothFactor;
    private Vector3 _direction;
    public float _zoom;
    [SerializeField] private float _minZoomDistance = 1f;
    [SerializeField] private float _maxZoomDistance = 20f;
    [SerializeField] private float _zoomSensitivity = 15f;

    private void Start()
    {
        _offset = _direction = transform.position - _target.position;
        _direction.Normalize();
        _zoom = _offset.magnitude;
    }

    private void Update()
    {
        var mouseWheel = Input.GetAxis("Mouse ScrollWheel") * _zoomSensitivity;
        _zoom = Mathf.Clamp(_zoom + mouseWheel, _minZoomDistance, _maxZoomDistance);
    }

    void LateUpdate()
    {
        var targetPosition = _target.position + _offset - _direction * _zoom;
        transform.position = Vector3.Lerp(transform.position, targetPosition, _smoothFactor);
    }
}
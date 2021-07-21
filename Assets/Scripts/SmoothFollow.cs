using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    private Vector3 _offset;
    [SerializeField] private float _smoothFactor;

    private void Start()
    {
        _offset = transform.position - _target.position;
    }

    void LateUpdate()
    {
        var targetPosition = _target.position + _offset;
        var vel = Vector3.zero;
        transform.position = Vector3.Lerp(transform.position, targetPosition, _smoothFactor);
    }
}
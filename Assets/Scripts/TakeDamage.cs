using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TakeDamage : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Image _sliderFill;
    [SerializeField] private Color _flashColour;
    [SerializeField] private AnimationCurve _flashCurve;
    [SerializeField] private float _animationDuration = 0.2f;
    
    private Color _initialColour;

    private void Start()
    {
        _initialColour = _sliderFill.color;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _slider.DOValue(_slider.value - .1f, _animationDuration);
            
            _sliderFill
                .DOColor(_flashColour, _animationDuration)
                .SetEase(_flashCurve)
                .OnComplete(() => _sliderFill.color = _initialColour);
        }
    }
}

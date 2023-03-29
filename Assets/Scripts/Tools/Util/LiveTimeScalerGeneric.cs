using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class LiveTimeScalerGeneric : MonoBehaviour
{
    [SerializeField, Range(0, 10f)]
    private float _timeScale = 1;
    [SerializeField, Range(.01f, 1f), Tooltip("Keypad +/- to increment or decrement")]
    private float _stepOfIncrement = .2f;

    public float TimeMultiplier
    {
        get => _timeScale; set
        {
            _timeScale = value;

#if UNITY_EDITOR
            Time.timeScale = Mathf.Max(_timeScale, 0);
#else
            Time.timeScale = Mathf.Clamp(_timeScale, .2f, 3);
#endif
        }
    }
    private void Start()
    {
        TimeMultiplier = _timeScale;
        //TimeMultiplier = 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            TimeMultiplier += _stepOfIncrement;
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            TimeMultiplier -= _stepOfIncrement;
        }
        ResetTime();
        
    }
    private void ResetTime()
    {
        if (Input.GetKeyDown(KeyCode.KeypadMultiply))
            TimeMultiplier = 1;
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockInitialTransform : MonoBehaviour
{
    private Vector3 _localRot;
    private Vector3 _localPos;

    private void Awake()
    {
        _localRot = transform.localEulerAngles;
        _localPos = transform.localPosition;
    }

    private void LateUpdate()
    {
        transform.localEulerAngles = _localRot;
        transform.localPosition = _localPos;
    }
}

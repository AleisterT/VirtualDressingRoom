using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollission : MonoBehaviour
{
    [SerializeField] private Transform swordTransform;

    private bool _validPosition = true;
    private Vector3 _lastValidPosition;
    private Quaternion _lastValidRotation;
    public bool forceShow;

    private void OnTriggerStay(Collider other)
    {
        _validPosition = false;
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (swordTransform.GetComponentInParent<SwordGrab>() != null)
        {
            swordTransform.gameObject.SetActive(true);
            return;
        }

        if (forceShow)
        {
            swordTransform.gameObject.SetActive(true);
            return;
        }

        if (_validPosition)
        {
            // _lastValidPosition = transform.position;
            // _lastValidRotation = transform.rotation;
        }
        swordTransform.gameObject.SetActive(_validPosition);
        _validPosition = true;
        // swordTransform.position = _lastValidPosition;
        // swordTransform.rotation = _lastValidRotation;
    }
}

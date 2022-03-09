using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstraintBones : MonoBehaviour
{
    [SerializeField] private bool constraintX;
    [SerializeField] private bool constraintY;
    [SerializeField] private bool constraintZ;

    [SerializeField] private Vector3 maxLocalRotation;
    [SerializeField] private Vector3 minLocalRotation;
    
    // Update is called once per frame
    void LateUpdate()
    {
        var localRotation = transform.localEulerAngles;
        if (constraintX)
        {
            localRotation.x = Mathf.Clamp(localRotation.x, minLocalRotation.x, maxLocalRotation.x);
        }
        if (constraintY)
        {
            localRotation.y = Mathf.Clamp(localRotation.y, minLocalRotation.y, maxLocalRotation.y);
        }
        if (constraintZ)
        {
            localRotation.z = Mathf.Clamp(localRotation.z, minLocalRotation.z, maxLocalRotation.z);
        }

        transform.localEulerAngles = localRotation;
    }

    [ContextMenu("Copy max")]
    private void CopyMax()
    {
        maxLocalRotation = transform.localEulerAngles;
    }

    [ContextMenu("Copy min")]
    private void CopyMin()
    {
        minLocalRotation = transform.localEulerAngles;
    }
}

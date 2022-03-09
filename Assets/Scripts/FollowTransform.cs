using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform lookTarget;
    [SerializeField] private FollowTransform nextComponent;
    [SerializeField] private bool isRoot;

    // Update is called once per frame
    void LateUpdate()
    {
        if (!isRoot)
        {
            return;
        }
        UpdateTransformChain();
    }

    public void UpdateTransformChain()
    {
        transform.position = target.position;
        transform.rotation = target.rotation;
        // transform.right = Vector3.up;
        // if (lookTarget != null)
        // {
        //     transform.LookAt(lookTarget,Vector3.up);
        // }
        if (nextComponent != null)
        {
            nextComponent.UpdateTransformChain();
        }
    }
}

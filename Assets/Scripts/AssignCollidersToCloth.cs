using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignCollidersToCloth : MonoBehaviour
{
    [SerializeField] private GameObject secondRoot;
    [SerializeField] private GameObject targetRoot;
    [SerializeField] private Cloth cloth;
    
    [ContextMenu("Assign colliders")]
    private void AssignColliders()
    {
        cloth.capsuleColliders = targetRoot.GetComponentsInChildren<CapsuleCollider>();
    }

    [ContextMenu("Copy colliders")]
    private void CopyColliders()
    {
        CopyColliderRecursive(targetRoot.transform,secondRoot.transform);
    }

    private void CopyColliderRecursive(Transform originalElement, Transform copyElement)
    {
        var capsule = originalElement.GetComponent<CapsuleCollider>();
        if (capsule != null)
        {
            var capsuleCopy = copyElement.GetComponent<CapsuleCollider>();
            if (capsuleCopy == null)
            {
                capsuleCopy = copyElement.gameObject.AddComponent<CapsuleCollider>();
            }

            capsuleCopy.center = capsule.center;
            capsuleCopy.direction = capsule.direction;
            capsuleCopy.height = capsule.height;
            capsuleCopy.radius = capsule.radius;
        }

        for (int i = 0; i < originalElement.childCount; ++i)
        {
            CopyColliderRecursive(originalElement.GetChild(i),copyElement.GetChild(i));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ShoulderArmorRotation : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float maxHeightDiff;
    [SerializeField] private float minHeightDiff;
    [SerializeField] private float maxRotation;
    [SerializeField] private float minRotation;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var heightDiff = target.position.y - transform.position.y;
        var localEuler = transform.localEulerAngles;
        var diffNormalized = (Mathf.Clamp(heightDiff,minHeightDiff,maxHeightDiff) - minHeightDiff)/(maxHeightDiff - minHeightDiff);
        localEuler.y = Mathf.LerpAngle(minRotation, maxRotation, diffNormalized);
        transform.localEulerAngles = localEuler;
    }
}

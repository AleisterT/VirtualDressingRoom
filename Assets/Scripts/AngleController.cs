using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleController : MonoBehaviour
{
    [SerializeField] private KinectManager kinectManager;
    [SerializeField] private Transform cameraTransform;

    private int StartNumSamples = 60;
    private int numSamples = 60;
    private float averageAngle;
    
    void Start()
    {
        numSamples = StartNumSamples;
        kinectManager.autoHeightAngle = KinectManager.AutoHeightAngle.AutoUpdate;
    }

    // Update is called once per frame
    void Update()
    {
        if (numSamples > 0)
        {
            averageAngle += cameraTransform.localEulerAngles.x;
            --numSamples;
        }
        if (numSamples == 0)
        {
            averageAngle = averageAngle / StartNumSamples;
            --numSamples;
            cameraTransform.localEulerAngles = new Vector3(averageAngle,0,0);
            gameObject.SetActive(false);
        }
    }
}

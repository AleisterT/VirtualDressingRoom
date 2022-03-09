using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwordGrab : MonoBehaviour
{
    [SerializeField] private SwordCollission swordCollission;
    [SerializeField] private float interactionDelaySeconds = 1f;
    [SerializeField] private UserDetectionController userDetectionController;
    [SerializeField] private float interactionRadius = 0.2f;
    [SerializeField] private Image swordGrabIndicator;
    [SerializeField] private Transform swordTransform;
    [SerializeField] private Transform beltTransform;
    [SerializeField] private Transform handTransform;

    private float _lastInteractionEndTime;
    
    // Start is called before the first frame update
    void Start()
    {
        userDetectionController.UserLeft -= UserLeftHandler;
        userDetectionController.UserLeft += UserLeftHandler;
    }

    private void OnDestroy()
    {
        userDetectionController.UserLeft -= UserLeftHandler;
    }

    private void UserLeftHandler()
    {
        swordTransform.SetParent(beltTransform,false);
        swordGrabIndicator.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        swordCollission.forceShow = false;
        swordGrabIndicator.transform.position = beltTransform.position;
        if (swordTransform.parent == beltTransform)
        {
            if (Vector3.Distance(beltTransform.position, handTransform.position) < interactionRadius)
            {
                if (Time.time >= _lastInteractionEndTime + interactionDelaySeconds)
                {
                    swordGrabIndicator.fillAmount += Time.deltaTime;
                }
            }
            else
            {
                swordGrabIndicator.fillAmount = 0;
            }
            if (swordGrabIndicator.fillAmount >= 1f)
            {
                swordTransform.SetParent(handTransform,false);
                swordGrabIndicator.fillAmount = 0;
                _lastInteractionEndTime = Time.time;
            }
        }
        
        if (swordTransform.parent == handTransform)
        {
            
            if (Vector3.Distance(beltTransform.position, handTransform.position) < interactionRadius)
            {
                swordCollission.forceShow = true;
                if (Time.time >= _lastInteractionEndTime + interactionDelaySeconds)
                {
                    swordGrabIndicator.fillAmount += Time.deltaTime;
                }
            }
            else
            {
                swordGrabIndicator.fillAmount = 0;
            }
            if (swordGrabIndicator.fillAmount >= 1f)
            {
                swordTransform.SetParent(beltTransform,false);
                swordGrabIndicator.fillAmount = 0;
                _lastInteractionEndTime = Time.time;
            }
        }
    }
}
